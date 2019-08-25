import pyrealsense2 as rs
import cv2
import numpy as np


def access_pixels(image):
    ##相当于取反 例如白的变成黑的，黑的变成白的，
    for row in range(image.shape[0]):
        for col in range(image.shape[1]):
            pv = image[row, col]
            image[row, col] = 255 - pv  # 相当于取反 例如白的变成黑的，黑的变成白的，
    return image

class TargetRecognition():
    def __init__(self,minarea = 3000,threshold_value = 199):
        self.minarea=minarea
        self.threshold_value =threshold_value
        self.tuplelist = []

    def take_img(self):
        pass


    def target(self,color_img, depth_img):
            # self.color_img,self.depth_img=self.take_img()
            frame = color_img
            ret, depth_img2 = cv2.threshold(depth_img, self.threshold_value, 255, cv2.THRESH_BINARY)  #二值化
            depth_img = access_pixels(depth_img2)
            imge, contours, hierarchy = cv2.findContours(depth_img, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)
            cv2.imshow('depth',depth_img)
            area = []
            cX = []
            cY = []
            i = 0
            hasobj = False
            try:
                for c in contours:
                    if cv2.contourArea(c) > self.minarea//20:

                        M = cv2.moments(c)
                        y = int(M["m01"] / M["m00"])
                        if y < 500:
                            area.append(cv2.contourArea(c))
                            cX.append(int(M["m10"] / M["m00"]))
                            cY.append(int(M["m01"] / M["m00"]))
                            # 画出中点
                            cv2.circle(color_img, (cX[-1]*4, cY[-1]*4), 7, (255, 255, 255), -1)
                            cv2.putText(color_img, "obj" + str(i), (cX[-1]*4 - 20, cY[-1]*4 - 20),
                                        cv2.FONT_HERSHEY_SIMPLEX, 0.5, (255, 255, 255), 2)
                            i = i + 1
                            hasobj=True
                            self.tuplelist.append((cX[-1]*4, cY[-1]*4))
            except:
                print("无目标")
            return color_img











