# from socket import *
#
#
# class SocketConnection:
#     def __init__(self):   #socket的连接创建
#         self.HOST = ''
#         self.PORT = 3000
#         self.BUFSIZ = 1024
#         self.addr = (self.HOST, self.PORT)
#         self.Socket = socket(AF_INET, SOCK_STREAM)
#
#
#     def start_wait(self):
#         self.Socket.setsockopt(SOL_SOCKET, SO_REUSEADDR, 1)
#         self.Socket.bind(self.addr)
#         self.Socket.listen(5)
#         print("监听中...")
#         self.Conn, self.clien_addr = self.Socket.accept()
#         # self.Conn.sendall('okay'.encode())
#         # self.Conn.sendall('1'.encode())
#
#     def sent_mode(self):
#         self.Conn.sendall(str(1,1,1).encode())
#     def sent_posision(self):
#         pass
#     def sent_chess_result(self):
#         pass
#
#
#
# if __name__ == '__main__':
#     socket = SocketConnection()
#     socket.start_wait()
#     socket.Conn.sendall('00042two2'.encode())
#     #for i in range(10):
#         string = input('请输入：')
#         #
#         socket.Conn.sendall((str(len(string)).zfill(4) +'4'+string).encode())
#         #socket.Conn.sendall('00054'.encode())
#        # socket.Conn.sendall(string.encode())
#         print("发送完毕！")
# def write_file(fpath,content ):
#     while True:
#         try:
#             f = open(fpath,'w')
#             f.write(content)
#             f.close()
#             break
#         except:
#
#             print('0')
# mode = 1
# write_file('msg.txt','mod1mode')

#
# import cv2
# import  numpy as np
# import os
# import random
#
#
# filelist  = os.listdir('./JP')
#
# def contrast_demo(img1, c, b):  # 亮度就是每个像素所有通道都加上b
#     rows, cols, chunnel = img1.shape
#     blank = np.zeros([rows, cols, chunnel], img1.dtype)  # np.zeros(img1.shape, dtype=uint8)
#     dst = cv2.addWeighted(img1, c, blank, 1-c, b)
#     return dst
# def add_demo(img1,c, b):
#     rows, cols, chunnel = img1.shape
#     white = np.ones_like(img1)*255
#     dst = cv2.addWeighted(img1, c, white, 1-c, b)
#     return dst
#
# for file in filelist:
#     img = cv2.imread(file)
#     rannum = random.choice([0.5,0.6,0.7,0.75,0.8,0.85,0.9])
#     #img1 = np.zeros((540, 540,3), np.uint8)
#     img1 = contrast_demo(img,rannum, 0)
#     cv2.imwrite('OO/'+ file, img1)
#     #img2 = add_demo(img,0.8,0)
# print('已完成一半！')
# for file in filelist:
#     img3 = cv2.imread(file)
#     rann = random.choice([0.5,0.6,0.7,0.75,0.8,0.85,0.9])
#     img3 = add_demo(img3,rann,0)
#     cv2.imwrite('ADD/add'+file, img3)
#
# print('已完成！')

sta = read_file('status.txt')
mod = read_file('mod.txt')
