import easyocr
import cv2
from matplotlib import pyplot as plt
import numpy as np
from PIL import ImageFont,ImageDraw,Image

IMAGE_PATH = 'watchout.jpg'

reader= easyocr.Reader(['ko'])
result=reader.readtext(IMAGE_PATH)
print(result)


font=ImageFont.truetype("font/gulim.ttc",15)
img=Image.open(IMAGE_PATH)


for detection in result:
        
    top_left=tuple([int(val) for val in detection[0][0]])
    bottom_right=tuple([int(val) for val in detection[0][2]])
    text=detection[1]
    draw=ImageDraw.Draw(img)
    draw.rectangle((top_left,bottom_right),outline=(0,0,0),width=3)
    draw.text((top_left[0]+3,top_left[1]-18),text,(0,0,0),font=font)


plt.figure(figsize=(10,10))
plt.imshow(img)
plt.show()