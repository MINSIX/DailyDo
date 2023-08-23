import os
import torch
import torch.nn as nn
from torch.utils.data import DataLoader
from torchvision import transforms
from spp_layer import spatial_pyramid_pool
from torch.utils.data import Dataset
import PIL.Image as Image
import json
import cv2
import numpy as np
class CNNWithSPP(nn.Module):
    def __init__(self, num_classes):
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
# Define data paths
data_root = 'D:/sppnet/justme/test/images'
model_path = 'trained_model.pth'  # Path to your trained model
label_file = 'D:/sppnet/justme/test/label.json'

# Define data transformations
data_transform = transforms.Compose([
    transforms.Resize((224, 224)),
    transforms.ToTensor(),
    transforms.Normalize(mean=[0.485, 0.456, 0.406], std=[0.229, 0.224, 0.225]),
])

# Initialize the model
device = torch.device('cuda')
with open(label_file, 'r') as f:
    coco_labels = json.load(f)
num_classes = len(coco_labels)

model = CNNWithSPP(num_classes)  # Replace with your model initialization code

model.load_state_dict(torch.load(model_path, map_location=device))
model.to(device)
model.eval()

# Load test dataset
test_dataset = CustomDataset(data_root, label_file=label_file,transform=data_transform)
test_dataloader = DataLoader(test_dataset, batch_size=1, shuffle=False)

# Evaluate the model
correct = 0
total = 0

# with torch.no_grad():
#     for i, (images, labels) in enumerate(test_dataloader):
#         images = images.to(device)
#         outputs = model(images)
#         _, predicted = torch.max(outputs.data, 1)
        
#         # Save the image with bounding boxes
#         result_image = cv2.imread(test_dataset.image_paths[i])  # Load image using OpenCV
#         height, width, _ = result_image.shape
#         for pred_label in predicted:
#             class_name = coco_labels['categories'][pred_label.item()]['name']  # Convert tensor to integer using .item()
#             annotations = [ann for ann in coco_labels['annotations'] if ann['image_id'] == test_dataset.coco_labels['images'][i]['id'] and ann['category_id'] == pred_label.item()]
#             if annotations:
#                 bbox = annotations[0]['bbox']  # Assuming 'bbox' key contains bounding box info
#                 x_min, y_min, bbox_width, bbox_height = bbox
#                 x_max = x_min + bbox_width
#                 y_max = y_min + bbox_height
#                 x_min = int(x_min * width)
#                 y_min = int(y_min * height)
#                 x_max = int(x_max * width)
#                 y_max = int(y_max * height)
#                 cv2.rectangle(result_image, (x_min, y_min), (x_max, y_max), (0, 0, 255), 2)
#                 cv2.putText(result_image, class_name, (x_min, y_min - 10), cv2.FONT_HERSHEY_SIMPLEX, 0.5, (0, 0, 255), 2)
        
#         result_image_path = os.path.join(result_path, f"result_{i}.jpg")
#         cv2.imwrite(result_image_path, result_image)os.makedirs(result_path, exist_ok=True)
import matplotlib.pyplot as plt
result_path='D:/sppnet/justme/test/result/MM133images'
os.makedirs(result_path,exist_ok=True)
with torch.no_grad():
    for i, (images, labels) in enumerate(test_dataloader):
        images = images.to(device)
        outputs = model(images)
        _, predicted = torch.max(outputs.data, 1)
        
        # Load image using PIL
        image_path = test_dataset.image_paths[i]
        image = Image.open(image_path).convert("RGB")
        
        # Draw bounding boxes and labels using Matplotlib
        plt.figure(figsize=(10, 8))
        plt.imshow(np.array(image))  # Convert PIL Image to numpy array for Matplotlib
        current_axis = plt.gca()
        
        annotations = [ann for ann in coco_labels['annotations'] if ann['image_id'] == test_dataset.coco_labels['images'][i]['id']]
        
        for annotation in annotations:
            category_id = annotation['category_id']
            class_name = coco_labels['categories'][category_id]['name']
            bbox = annotation['bbox']
            x, y, w, h = bbox
            rect = plt.Rectangle((x, y), w, h, fill=False, color='red', linewidth=2)
            current_axis.add_patch(rect)
            current_axis.text(x, y, class_name, bbox={'facecolor':'red', 'alpha':0.5})
        
        plt.axis('off')
        result_image_path = os.path.join(result_path, f"result_{i}.png")
        plt.savefig(result_image_path, bbox_inches='tight', pad_inches=0.0, format='png')
        plt.close()
