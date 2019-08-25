# encoding:utf-8
import urllib
from urllib import request,parse
import ssl, json
import base64
from Kinect import KINECT
import cv2
from threading import Thread
import time
from face_identification import face_identif
from face_register import FaceRegister
import  re,os
import numpy as np


from flask import Flask, render_template, Response, flash, session, current_app
from flask_bootstrap import Bootstrap

pic_file_path='pic\\'

app = Flask(__name__)
app.config['SECRET_KEY'] = os.urandom(24)
#app.config['PERMANENT_SESSION_LIFETIME'] = timedelta(hours=50)
bootstrap=Bootstrap(app)


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

url = "https://aip.baidubce.com/rest/2.0/face/v3/detect"


def FaceDetect(pic, token):
    s=''
    # 二进制方式打开图片文件
    context = ssl._create_unverified_context()
    global haspic
    try:
        f = open(pic, 'rb')
        # f = open('image.jpg', 'rb')
        img = base64.b64encode(f.read())
        # image.color_imge_take().tobytes()
        f.close()

        params = {"image": img,"image_type": "BASE64","face_field": "age,beauty,face_shape,gender,emotion",
                  "max_face_num": 1}
        params = urllib.parse.urlencode(params).encode(encoding='UTF8')

        request_url = url + "?access_token=" + token
        request = urllib.request.Request(url=request_url, data=params)
        request.add_header('Content-Type', 'application/x-www-form-urlencoded')
        response = urllib.request.urlopen(request, context=context)
        content = response.read()
        if content:
            js = json.loads(content)
            if "SUCCESS" in js["error_msg"]:
                for item in js['result']['face_list']:
                    if 'female' in str(item['gender']):
                        if item['age']<=14:
                            sx = u'小妹妹'
                        if item['age']>14 and item['age']<=28:
                            sx = u'姐姐'
                        if item['age']>28 and item['age']<=55:
                            sx = u'阿姨'
                        if item['age']>55 :
                            sx = u'奶奶'

                    else:
                        if item['age'] <= 14:
                            sx = u'小弟弟'
                        if item['age']>14 and item['age']<28:
                            sx = u'哥哥'
                        if item['age']>28 and item['age']<=55:
                            sx = u'叔叔'
                        if item['age']>55 :
                            sx = u'爷爷'
                    if 'angry' in str(item['emotion']):
                        em = u'你现在很愤怒'
                    elif 'disgust' in str(item['emotion']):
                        em = u'你现在很厌恶'
                    elif 'fear' in str(item['emotion']):
                        em = u'你现在很恐惧'
                    elif 'happy' in str(item['emotion']):
                        em = u'你现在很高兴'
                    elif 'sad' in str(item['emotion']):
                        em = u'你现在很伤心'
                    elif 'surprise' in str(item['emotion']):
                        em = u'你现在很惊讶'
                    else:
                        em = u'表情很平静'
                    if 'square' in str(item['face_shape']):
                        fs = u'正方形'
                    elif 'triangle' in str(item['face_shape']):
                        fs = u'三角形'
                    elif 'oval' in str(item['face_shape']):
                        fs = u'椭圆'
                    elif 'heart' in str(item['face_shape']):
                        fs = u'心形'
                    else:
                        fs = u'圆形'
                    face_score=item['beauty']+30
                    if  face_score>95: face_score=94.6
                    s=u'%s你好, 你是%s脸,颜值为 %2.2f 分, %s.' % (sx,fs, face_score,em)
                    print(s)
            else:
                print("未识别到人脸")
        os.remove(pic)
        haspic=False
        return s
    except:return None

def read_txt(filename):
    content="0"
    try:
        f = open(filename, "r")  # 设置文件对象
        content = f.read()  # 将txt文件的所有内容读入到字符串str中
        f.close()  # 将文件关闭
    except:
        print("读TXT出错")
    return content


ifsave1=False
ifsave2=False
ifsave3=False
ifimage=False
# image=np.zeros((480, 480, 3), np.uint8)
on_off ='off'
haspic=False
haspic3=False
user_id=int(read_txt(pic_file_path+"0present_user_id.txt"))

def upload_img():
    K = KINECT()
    global ifsave1,ifsave2,ifsave3,user_id,on_off,ifimage,haspic,haspic3
    ifsave1=False
    ifsave2 =False
    ifsave3=False

    while True:
        t=time.time()
        # try:
        if on_off =='on':
            break
        image,cropped,ifimage=K.color_imge_take()
        # result=FaceDetect(image, GetToken())  # 图片名：pic 文件夹里的 1.jpg
        cv2.imshow('image', image)
        cv2.waitKey(30)

        if ifsave1:
            ifsave1 = False
            if ifimage:
                cv2.imwrite(pic_file_path+'face_score.jpg', cropped)
                haspic=True
            else:
                print("未识别到人脸")
                WriteTxt('msg.txt', "未识别到人脸")
        if ifsave2 and ifimage:
            ifsave2=False
            user_id+=1
            cv2.imwrite(pic_file_path+str(user_id)+'.jpg', cropped)
            print(time.time() - t)
            WriteTxt(pic_file_path + "0present_user_id.txt", str(user_id))
        if ifsave3:
            ifsave3 = False
            if ifimage:
                cv2.imwrite(pic_file_path+'who.jpg', cropped)
                haspic3=True
            else:
                print("未识别到人脸")
                WriteTxt('msg.txt', "未识别到人脸")
        gl.set_value('vedio_pic', image)
        # ret, jpeg = cv2.imencode('.jpg', image)
        # yield (b'--frame\r\n'b'Content-Type: image/jpeg\r\n\r\n' + jpeg.tobytes() + b'\r\n\r\n')

        # except:print("出错了")


def face_fuc():
    t1 = Thread(target=upload_img)
    t1.start()
    time.sleep(2)
    # t2 = Thread(target=video_transport)
    # t2.start()
    global ifsave1,ifsave2,ifsave3,user_id,on_off,ifimage,haspic,haspic3

    while True:
        while True:
            on_off = read_txt('on_off.txt')
            temp= read_txt('face.txt')
            time.sleep(0.1)
            if on_off == 'on':
                break
            if not temp == '0':
                WriteTxt('face.txt', '0')
                break

        # temp = input("请输入模式：1为颜值检测，2为身份注册，3为身份认证")
        if on_off =='on':
            break
        if temp=='1':
            ifsave1= True
            time.sleep(0.06)
            counter=0
            while not haspic:
                time.sleep(0.15)
                counter+=1
                if counter>4:
                    break
            result1=FaceDetect(pic_file_path+"face_score.jpg", GetToken())  # 图片名：pic 文件夹里的 1.jpg
            WriteTxt('msg.txt', result1)

        if 'nameis' in temp:

            match_obj = re.match("nameis(\w+)", temp)  # 匹配中文开头
            if match_obj:
                name= match_obj.group(1)
            # for name in temp if name.startswith(('nameis'))
            ifsave2 = True
            time.sleep(0.06)
            if ifimage:
                time.sleep(0.2)
                result2=FaceRegister(pic_file_path+str(user_id)+'.jpg',str(user_id),name, GetToken())
                WriteTxt('msg.txt', result2)
            else:
                print("mei识别到人脸")
                WriteTxt('msg.txt', "未识别到人脸")

        if temp == '3':
            ifsave3 = True
            time.sleep(0.06)
            counter = 0
            while not haspic3:
                time.sleep(0.15)
                counter += 1
                if counter > 4:
                    break
            result3,haspic3=face_identif()
            WriteTxt('msg.txt', result3)


def video_transport():
    app.run(host='0.0.0.0', threaded=False, port=50000)


# 返回视频流响应
@app.route('/video_feed')
def video_feed():
    # global image
    # ret, jpeg = cv2.imencode('.jpg', image)
    return Response(upload_img(),
                    mimetype='multipart/x-mixed-replace; boundary=frame')

@app.route('/favicon.ico')
def favicon():
    return current_app.send_static_file('static/favicon.ico')

if __name__ == "__main__":
    face_fuc()


