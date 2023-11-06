import pytesseract
import cv2
import matplotlib.pyplot as plt
pytesseract.pytesseract.tesseract_cmd = 'C:\\Program Files\\Tesseract-OCR\\tesseract.exe'
path = 'watchoutame.jpeg'
# image=cv2.imread(path)
# rgb=cv2.cvtColor(image,cv2.COLOR_BGR2RGB)

# text=pytesseract.image_to_string(rgb,lang='kor+eng')
# print(text)   
text=pytesseract.image_to_string(path,lang='eng')
print(text) 
