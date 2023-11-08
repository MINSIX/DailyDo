import cv2
import numpy as np

img = cv2.imread('watchout.jpg')


methods=['cv2.TM_CCOEFF_NORMED','cv2.TM_CCORR_NORMED','cv2.TM_SQDIFF_NORMED']
                        

for i, method_name in enumerate(methods):
   
    image='1.jpg'
    template=cv2.imread(image)

    th,tw=template.shape[:2]
    cv2.imshow('template',template)
    img_draw=img.copy()
    method=eval(method_name)

    res=cv2.matchTemplate(img,template,method)
    min_val, max_val, min_loc, max_loc = cv2.minMaxLoc(res)
    print(method_name,min_val,max_val,min_loc,max_loc)

    if method in [cv2.TM_SQDIFF, cv2.TM_SQDIFF_NORMED]:
        top_left= min_loc
        match_val = min_val
    else:
        top_left=max_loc
        match_val = max_val
    
    bottom_right=(top_left[0]+tw,top_left[1]+th)
    cv2.rectangle(img_draw,top_left,bottom_right,(0,0,255),2)


    
cv2.imshow(method_name,img_draw)
cv2.waitKey(0)
cv2.destroyAllWindows()


