import os
import json
import torch
import torch.nn as nn
import torch.optim as optim
from torch.utils.data import DataLoader
from torchvision import transforms
from spp_layer import spatial_pyramid_pool
from torch.utils.data import Dataset
import PIL.Image as Image
device = torch.device('cuda')
class CNNWithSPP(nn.Module):
    def __init__(self, num_classes,batch_size):
        super(CNNWithSPP, self).__init__()
        
        # Convolutional layers
        self.conv_layers = nn.Sequential(
            nn.Conv2d(in_channels=3, out_channels=16, kernel_size=3, padding=1),
            nn.ReLU(),
            nn.MaxPool2d(kernel_size=2, stride=2),
            nn.Conv2d(in_channels=16, out_channels=32, kernel_size=3, padding=1),
            nn.ReLU(),
            nn.MaxPool2d(kernel_size=2, stride=2),
            nn.Conv2d(in_channels=32, out_channels=64, kernel_size=3, padding=1),
            nn.ReLU(),
            nn.MaxPool2d(kernel_size=2, stride=2)
        )
        
        # Fully connected layers with SPP
        self.fc_layers = nn.Sequential(
            nn.Linear(1344, num_classes),  # Assuming 4 levels in SPP
            nn.ReLU(),
      
        )
        
    def forward(self, x):
        x = self.conv_layers(x)
        
        # Calculate SPP
        spp_features = spatial_pyramid_pool(x, x.size(0), [x.size(2), x.size(3)], out_pool_size=[4, 2, 1])
        
        # Fully connected layers
        spp_features = spp_features.view(spp_features.size(0), -1)  # Flatten the SPP features
        output = self.fc_layers(spp_features)
        return output

# Initialize the model
class CustomDataset(Dataset):
    def __init__(self, data_root, label_file, transform=None):
        self.data_root = data_root
        self.transform = transform

        # Load COCO JSON labels
        with open(label_file, 'r') as f:
            self.coco_labels = json.load(f)
        
        self.image_paths = []
        self.labels = []
        for image_info in self.coco_labels['images']:  # Iterate over images
            image_path = os.path.join(self.data_root, image_info['file_name'])  # Use 'file_name' instead of 'filename'
            self.image_paths.append(image_path)

        for ann in self.coco_labels['annotations']:  # Iterate over annotations
            label = ann['category_id']  # Use 'category_id' instead of 'label'
            self.labels.append(label)

    def __len__(self):
        return len(self.image_paths)

    def __getitem__(self, idx):
        image_path = self.image_paths[idx]
        label = self.labels[idx]

        image = Image.open(image_path).convert("RGB")
        if self.transform:
            image = self.transform(image)

        return image, label
# Define paths and hyperparameters
data_root = 'D:/sppnet/justme/train/images'
label_file = 'D:/sppnet/justme/train/label.json'
batch_size = 32
num_epochs = 20
learning_rate = 0.001

# Load COCO JSON labels
with open(label_file, 'r') as f:
    coco_labels = json.load(f)
num_classes = len(coco_labels)

# Define data transformations
data_transform = transforms.Compose([
    transforms.Resize((224, 224)),  # Resize images to fit model input size
    transforms.ToTensor(),
    transforms.Normalize(mean=[0.485, 0.456, 0.406], std=[0.229, 0.224, 0.225]),  # Normalize images
])

# Initialize model and optimizer
model = CNNWithSPP(num_classes,batch_size).to(device)  # Replace with your model initialization code
optimizer = optim.Adam(model.parameters(), lr=learning_rate)
criterion = nn.CrossEntropyLoss()  # Assuming a classification task

# Load dataset
dataset = CustomDataset(data_root, label_file, transform=data_transform)
dataloader = DataLoader(dataset, batch_size=batch_size, shuffle=True)

# Training loop


model.train()

for epoch in range(num_epochs):
    for batch_idx, (images, labels) in enumerate(dataloader):
        images, labels = images.to(device), labels.to(device)
        
        optimizer.zero_grad()
        outputs = model(images)
        
        loss = criterion(outputs, labels)
        loss.backward()
        optimizer.step()
        
        if batch_idx % 10 == 0:
            print(f"Epoch [{epoch+1}/{num_epochs}], Batch [{batch_idx+1}/{len(dataloader)}], Loss: {loss.item():.4f}")

# Save trained model
saved_model_path = 'trained_model.pth'
torch.save(model.state_dict(), saved_model_path)
print(f"Trained model saved at {saved_model_path}")
