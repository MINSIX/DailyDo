# DailyDo
# Object Detection using Sppnet

yifanjiang19/sppnet-pytorch
  https://github.com/yifanjiang19/sppnet-pytorch.git

위 깃허브의 소스코드를 참고하여 spp_layer.py의 spatial_pyramid_pooling 함수를 이용해 train.py, testmany.py를 구현.
데이터는 roboflow에서 coco json형식으로 데이터를 가공하여서 사용하였음.


# SPPNET
sppnet은 기존의 R-CNN 또는 CNN에서 마지막 컨볼루션 레이어에 spatial pyramid pooling을 추가한 것을 말한다.
spatial pyramid pooling이란 다양한 공간적 크기의 피라미드로 입력 이미지를 분할하고, 각 영역에 대해 피라미드 풀링을 수행하는 것을 의미한다. 이로인해 입력 이미지의 크기에 상관없이 고정된 크기의 벡터를 얻을 수 있다는 장점을 지닌다.
또한 spp를 통해 객체의 부분 정보를 더 잘 포착할 수 있어서 객체의 특징을 효과적으로 학습할 수 있다.

# Error
train.py나 testmany.py를 작성하면서 다양한 오류가 있었다.

self.fc_layers = nn.Sequential(
            nn.Linear(1344, num_classes),  # Assuming 4 levels in SPP
            nn.ReLU(),)
            
위와 같이 레이어의 크기를 지정하는 부분에서 특정숫자가 아니면 안되는 경우가 있었고, 공부가 부족해 왜 특정숫자가 들어가는지 모르겠다.

더 많은 다양한 오류가 있었지만 대부분 인공지능에 대한 지식이 부족해서 생기는 오류였던 것 같다.
