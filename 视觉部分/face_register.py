# encoding:utf-8
import urllib
from urllib import request,parse
import ssl, json
import base64
import time
import numpy as np  
import cv2  
import os  
import shutil  


class FR():
    def __init__(self):
        self.cap = cv2.VideoCapture(0)

        # 设置视频参数 set camera
        self.cap.set(3, 480)

        # 人脸截图的计数器 the counter for screen shoot
        self.cnt_ss = 0

        # 存储人脸的文件夹 the folder to save faces
        self.current_face_dir = ""
        self.path_photos_from_camera = "pic/"


    # 新建保存人脸图像文件和数据CSV文件夹
    # mkdir for saving photos and csv
    def pre_work_mkdir(self):
        # 新建文件夹 / make folders to save faces images and csv
        if os.path.isdir(self.path_photos_from_camera):
            pass
        else:
            os.mkdir(self.path_photos_from_camera)
            self.pre_work_mkdir()


    ##### optional/可选, 默认关闭 #####
    # 删除之前存的人脸数据文件夹
    # delete the old data of faces
    def pre_work_del_old_face_folders(self):
        # 删除之前存的人脸数据文件夹
        # 删除 "/data_faces_from_camera/person_x/"...
        folders_rd = os.listdir(self.path_photos_from_camera)
        for i in range(len(folders_rd)):
            shutil.rmtree(self.path_photos_from_camera + folders_rd[i])

        if os.path.isfile("data/features_all.csv"):
            os.remove("data/features_all.csv")


    # 这里在每次程序录入之前, 删掉之前存的人脸数据
    # 如果这里打开，每次进行人脸录入的时候都会删掉之前的人脸图像文件夹 person_1/,person_2/,person_3/...
    # If enable this function, it will delete all the old data in dir person_1/,person_2/,/person_3/...
    # pre_work_del_old_face_folders()
    ##################################

    def importent(self):
        # 如果有之前录入的人脸 / if the old folders exists
        # 在之前 person_x 的序号按照 person_x+1 开始录入 / start from person_x+1
        if os.listdir("pic/"):
            # 获取已录入的最后一个人脸序号 / get the num of latest person
            person_list = os.listdir("pic/")
            person_num_list = []
            for person in person_list:
                person_num_list.append(int(person.split('_')[-1]))
            person_cnt = max(person_num_list)

        # 如果第一次存储或者没有之前录入的人脸, 按照 person_1 开始录入
        # start from person_1
        else:
            person_cnt = 0

        # 之后用来控制是否保存图像的 flag / the flag to control if save
        save_flag = 1

        # 之后用来检查是否先按 'n' 再按 's' / the flag to check if press 'n' before 's'
        press_n_flag = 0

    def face_reg(self):
            while self.cap.isOpened():
                flag, img_rd = self.cap.read()
                # print(img_rd.shape)
                # It should be 480 height * 640 width

                kk = cv2.waitKey(1)

                img_gray = cv2.cvtColor(img_rd, cv2.COLOR_RGB2GRAY)

                # 人脸数 faces
                faces = 1

                # 待会要写的字体 / font to write
                font = cv2.FONT_HERSHEY_COMPLEX

                # 按下 'n' 新建存储人脸的文件夹 / press 'n' to create the folders for saving faces
                if kk == ord('n'):
                    current_face_dir = self.path_photos_from_camera + "person_" + str(self.person_cnt)
                    os.makedirs(current_face_dir)

                    cnt_ss = 0  # 将人脸计数器清零 / clear the cnt of faces
                    press_n_flag = 1  # 已经按下 'n' / have pressed 'n'

                # 检测到人脸 / if face detected
                if len(faces) != 0:
                    # 矩形框 / show the rectangle box
                    for k, d in enumerate(faces):
                        # 计算矩形大小
                        # we need to compute the width and height of the box
                        # (x,y), (宽度width, 高度height)
                        pos_start = tuple([d.left(), d.top()])
                        pos_end = tuple([d.right(), d.bottom()])

                        # 计算矩形框大小 / compute the size of rectangle box
                        height = (d.bottom() - d.top())
                        width = (d.right() - d.left())

                        hh = int(height / 2)
                        ww = int(width / 2)

                        # 设置颜色 / the color of rectangle of faces detected
                        color_rectangle = (255, 255, 255)

                        # 判断人脸矩形框是否超出 480x640
                        if (d.right() + ww) > 640 or (d.bottom() + hh > 480) or (d.left() - ww < 0) or (d.top() - hh < 0):
                            cv2.putText(img_rd, "OUT OF RANGE", (20, 300), font, 0.8, (0, 0, 255), 1, cv2.LINE_AA)
                            color_rectangle = (0, 0, 255)
                            save_flag = 0
                            if kk == ord('s'):
                                pass

                        else:
                            color_rectangle = (255, 255, 255)
                            save_flag = 1

                        cv2.rectangle(img_rd,
                                      tuple([d.left() - ww, d.top() - hh]),
                                      tuple([d.right() + ww, d.bottom() + hh]),
                                      color_rectangle, 2)

                        # 根据人脸大小生成空的图像 / create blank image according to the size of face detected
                        im_blank = np.zeros((int(height * 2), width * 2, 3), np.uint8)

                        if save_flag:
                            # 按下 's' 保存摄像头中的人脸到本地 / press 's' to save faces into local images
                            if kk == ord('s'):
                                # 检查有没有先按'n'新建文件夹 / check if you have pressed 'n'
                                if press_n_flag:
                                    cnt_ss += 1
                                    for ii in range(height * 2):
                                        for jj in range(width * 2):
                                            im_blank[ii][jj] = img_rd[d.top() - hh + ii][d.left() - ww + jj]
                                    cv2.imwrite(current_face_dir + "/img_face_" + str(cnt_ss) + ".jpg", im_blank)

                                else:
                                    pass

                # 显示人脸数 / show the numbers of faces detected
                cv2.putText(img_rd, "Faces: " + str(len(faces)), (20, 100), font, 0.8, (0, 255, 0), 1, cv2.LINE_AA)

                # 添加说明 / add some statements
                cv2.putText(img_rd, "Face Register", (20, 40), font, 1, (0, 0, 0), 1, cv2.LINE_AA)
                cv2.putText(img_rd, "N: New face folder", (20, 350), font, 0.8, (0, 0, 0), 1, cv2.LINE_AA)
                cv2.putText(img_rd, "S: Save current face", (20, 400), font, 0.8, (0, 0, 0), 1, cv2.LINE_AA)
                cv2.putText(img_rd, "Q: Quit", (20, 450), font, 0.8, (0, 0, 0), 1, cv2.LINE_AA)

                # 按下 'q' 键退出 / press 'q' to exit
                if kk == ord('q'):
                    break

                # 如果需要摄像头窗口大小可调 / uncomment this line if you want the camera window is resizeable
                # cv2.namedWindow("camera", 0)

                cv2.imshow("camera", img_rd)

            # 释放摄像头 / release camera
            self.cap.release()

            cv2.destroyAllWindows()


def WriteTxt(filename ,content):
    b = 1
    while b < 2:
        try:
            with open(filename, 'w') as f:  # 如果filename不存在会自动创建， 'w'表示写数据，写之前会清空文件中的原有数据！
                f.write(str(content))
                b = 3
            f.close()
        except:
            b = 1
            time.sleep(0.1)
            print("未能写入")

def GetToken():
    context = ssl._create_unverified_context()
    client_id = 'R0H8ynY7MVFoqYFjRrvGF4DD'
    client_secret = 'b8d0QKMv9uI0mhXEzCmBAPz3BLcNQQLF'
    host = 'https://aip.baidubce.com/oauth/2.0/token?grant_type=client_credentials&client_id=%s&client_secret=%s' % (
    client_id, client_secret)
    request = urllib.request.Request(host)
    request.add_header('Content-Type', 'application/json; charset=UTF-8')
    response = urllib.request.urlopen(request, context=context)
    content = response.read()
    if (content):
        js = json.loads(content)
        # return js['refresh_token']
        return js['access_token']
    return None

'''
人脸探测
'''

url = "https://aip.baidubce.com/rest/2.0/face/v3/faceset/user/add"



def FaceRegister(pic,user_id,user_info, token):
    try:
        s=''
        # 二进制方式打开图片文件
        context = ssl._create_unverified_context()
        f = open(pic, 'rb')
        img = base64.b64encode(f.read())
        # image.color_imge_take().tobytes()

        params = {"image": img,"image_type": "BASE64","group_id": "male","user_id":user_id,
                  "user_info": user_info}
        params = urllib.parse.urlencode(params).encode(encoding='UTF8')

        request_url = url + "?access_token=" + token
        request = urllib.request.Request(url=request_url, data=params)
        request.add_header('Content-Type', 'application/x-www-form-urlencoded')
        response = urllib.request.urlopen(request, context=context)
        content = response.read()
        if content:
            # print(content)
            js = json.loads(content)
            if "SUCCESS" in js["error_msg"]:
                print("注册成功")
                return "注册成功"
            else:
                print("注册失败")
                return "未识别到人脸"
    except:return "未识别到人脸"

def main():
    a=FR
    FaceRegister('100001.jpg','100001',a, GetToken())  # 图片名：pic 文件夹里的 1.jpg



if __name__ == "__main__":
    main()
