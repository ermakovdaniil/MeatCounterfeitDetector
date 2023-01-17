import numpy as np
import cv2
from matplotlib import pyplot as plt

def analyze_image(orig_path, counterfeit_path):

    #img1=cv2.imread(orig_path,4)
    #img2=cv2.imread(counterfeit_path,4)

    ## Initiate SIFT detector

    #sift=cv2.xfeatures2d.SIFT_create()

    ## find the keypoints and descriptors with SIFT

    #kp1, des1 = sift.detectAndCompute(img1,None)
    #kp2, des2 = sift.detectAndCompute(img2,None)

    ## BFMatcher with default params
    #bf = cv2.BFMatcher()
    #matches = bf.knnMatch(des1,des2, k=2)

    ## Apply ratio test
    #good = []
    #for m,n in matches:
    #    if m.distance < 0.75*n.distance:
    #        good.append([m])
    #        a=len(good)
    #        percent=(a*100)/len(kp2)
    #        print("{} % similarity".format(percent))
    #        if percent >= 75.00:
    #            print('Match Found')
    #        if percent < 75.00:
    #            print('Match not Found')

    #img3 = cv2.drawMatchesKnn(img1,kp1,img2,kp2,good,None,flags=2)
    #plt.imshow(img3),plt.show()
    x = 5
    print(x)

if __name__ == "__main__":
    analyze_image("C:\Users\Даня\source\repos\VKR_v2\VKR\resources\origImages","C:\Users\Даня\source\repos\VKR_v2\VKR\resources\resImages")