#!/usr/bin/python
# -*- coding: utf-8 -*-

from openni import openni2
import numpy as np
import cv2

class KINECT():
    def __init__(self):
        openni2.initialize()  # can also accept the path of the OpenNI redistribution
        self.dev = openni2.Device.open_any()
        print(self.dev.get_device_info())
        #depth_stream = dev.create_depth_stream()
        self.color_stream = self.dev.create_color_stream()
        #depth_stream.start()
        self.color_stream.start()
        self.cascade = cv2.CascadeClassifier("haarcascade_frontalface_default.xml")  ## 读入分类器数据

    def color_imge_take(self):
        # 显示RGB图像
        cframe = self.color_stream.read_frame()
        cframe_data = np.array(cframe.get_buffer_as_triplet()).reshape([480, 640, 3])
        R = cframe_data[:, 80:560, 0]
        G = cframe_data[:, 80:560, 1]
        B = cframe_data[:, 80:560, 2]
        cframe_data = np.transpose(np.array([B, G, R]), [1, 2, 0])
        # image=cv2.rectangle(cframe_data, (320-120,190-170), (320+120, 190+170), (0, 255, 0), 5)

        image=cframe_data.copy()
        c="None"
        a=image
        ifimage=False
        # print(cframe_data.shape)
        # cv2.imshow('color', image)
        img1 = np.asanyarray(image)
        cv2.waitKey(30)

        gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)

        faces = self.cascade.detectMultiScale(gray, scaleFactor=1.1, minNeighbors=5, minSize=(110, 110))
        if len(faces)>0:
            x=faces[0][0]
            y=faces[0][1]
            w=faces[0][2]
            h=faces[0][3]
            if x >100 and (x+w)<380:
                x1 = x - 10
                x2 = x + w + 10
                y1 = y - 40
                y2 = y + h + 10
                if x1<0: x1=0
                if x1>479:x1=479

                if y1<0: y1=0
                if y1>479:y1=479

                if x2<0: x2=0
                if x2>479:x2=479

                if y2<0: y2=0
                if y2>479:y2=479
                cv2.rectangle(image, (x1, y1), (x2, y2), (0, 255, 0), 2)
                a=image
                cropped = np.zeros((y2-y1, x2-x1, 3), np.uint8)
                cropped[:, :, 0] = img1[y1:y2, x1:x2, 0]
                cropped[:, :, 1] = img1[y1:y2, x1:x2,  1]
                cropped[:, :, 2] = img1[y1:y2, x1:x2,  2]
                # cv2.imshow('image', cropped)
                c=cropped
                ifimage=True
            else:ifimage=False
        return a,c,ifimage


    def depth_imge_take(self):
        # 显示深度图
        frame = self.depth_stream.read_frame()
        dframe_data = np.array(frame.get_buffer_as_triplet()).reshape([480, 640, 2])
        dpt1 = np.asarray(dframe_data[:, :, 0], dtype='float32')
        dpt2 = np.asarray(dframe_data[:, :, 1], dtype='float32')
        dpt2 *= 255
        dpt = dpt1 + dpt2
        cv2.imshow('dpt', dpt)
        cv2.waitKey(30)

    def color_stop(self):
        # self.depth_stream.stop()
        self.color_stream.stop()
        self.dev.close()






def main():
    image=KINECT()
    while True:
        image.color_imge_take()
    image.color_stop()


if __name__ == "__main__":
    main()
