import matplotlib.pyplot as plt
import keras_ocr

ocr_keras = keras_ocr.pipeline.Pipeline()
img_path = 'watchout.jpg'  # 이미지 파일 경로
img = keras_ocr.tools.read(img_path)  # 이미지 파일 읽기
result = ocr_keras.recognize([img])  # 이미지 OCR 수행
keras_ocr.tools.drawAnnotations(img, result[0])  # 주석 그리기
plt.imshow(img)
plt.axis('off')
plt.show()

# predict = ocr_keras.recognize(img_ocr)
# fig, axs = plt.subplots(nrows=len(img_ocr), figsize=(10, 20))
# for ax, image, predictions in zip(axs, img_ocr, predict):
#    		 keras_ocr.tools.drawAnnotations(image=image, 
#                                     predictions=predictions, 
#                                     ax=ax)							
# img = predict [1]
# for keras_text, box in img:
#     		print(keras_text)


