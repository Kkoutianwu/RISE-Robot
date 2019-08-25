from five_chess import Recognize
from online_rec import  Online_rec
import pyrealsense2 as rs
import numpy as np
import cv2 as cv
from gobang import  GUI
from target_recognition import  TargetRecognition
import threading
# from  .threading2 import Thread
from videomain import app,VideoCamera
from flask import Flask, render_template, Response, flash, session, current_app
from flask_bootstrap import Bootstrap
import urllib
from urllib import request,parse
import ssl, json
import base64
from Kinect import KINECT
from threading import Thread
import time
from face_identification import face_identif
from face_register import FaceRegister
import  re,os




##############################################################

app = Flask(__name__)
app.config['SECRET_KEY'] = os.urandom(24)
#app.config['PERMANENT_SESSION_LIFETIME'] = timedelta(hours=50)
bootstrap=Bootstrap(app)

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
                        fs = u'圆形'  #哈哈哈哈哈哈哈哈哈哈哈
                    if fs == u'圆形':face_score=item['beauty']+35
                    else:face_score=item['beauty']+30
                    if  face_score>95: face_score=94.6
                    s=u'%s你好, 你是%s脸,颜值为 %2.2f 分, %s.' % (sx,fs, face_score,em)
                    print(s)
            else:
                print("未识别到人脸")
        os.remove(pic)
        haspic=False
        return s
    except:return "未识别到人脸"

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
#
# def test_up_image():
#     global video_pic
#     im1 = cv.imread('me.jpg')
#     # im2 = cv.resize(im1,(480,480))
#     while True:
#         time.sleep()
#
#         video_pic=im1

def upload_img():

    global ifsave1,ifsave2,ifsave3,user_id,on_off,ifimage,haspic,haspic3, video_pic,K
    on_off="off"
    ifsave1=False
    ifsave2 =False
    ifsave3=False

    while True:
        t=time.time()
        try:
            if on_off =='on':
                print("upload is break")
                break
            image,cropped,ifimage=K.color_imge_take()
            # result=FaceDetect(image, GetToken())  # 图片名：pic 文件夹里的 1.jpg
            cv.imwrite(pic_file_path+'333.jpg', np.zeros((480, 480, 3), np.uint8))

            if ifsave1:
                ifsave1 = False
                if ifimage:
                    cv.imwrite(pic_file_path+'face_score.jpg', cropped)
                    print(time.time() - t)
                    haspic=True
                else:
                    print("未识别到人脸")
                    WriteTxt('msg.txt', "未识别到人脸txt")
            # if ifsave2 and ifimage:
            #     ifsave2=False
            #     user_id+=1
            #     cv.imwrite(pic_file_path+str(user_id)+'.jpg', cropped)
            #     print(time.time() - t)
            #     WriteTxt(pic_file_path + "0present_user_id.txt", str(user_id))
            if ifsave2 :
                user_id+=1
                try:
                    cv.imwrite(pic_file_path+str(user_id)+'.jpg', cropped)
                    ifsave2 = False
                except:print("没得人脸")
                WriteTxt(pic_file_path + "0present_user_id.txt", str(user_id))
            if ifsave3:
                ifsave3 = False
                if ifimage:
                    cv.imwrite(pic_file_path+'who.jpg', cropped)
                    haspic3=True
                else:
                    print("未识别到人脸")
                    WriteTxt('msg.txt', "未识别到人脸txt")
            video_pic = image
            # test_up_image()


            # ret, jpeg = cv.imencode('.jpg', image)
            # jpeg.
            # yield (b'--frame\r\n'b'Content-Type: image/jpeg\r\n\r\n' + jpeg.tobytes() + b'\r\n\r\n')

        except:
          print("出错了")
          K.color_stop()
          time.sleep(0.5)
          K=KINECT


def face_fuc():
    global ifsave1,ifsave2,ifsave3,user_id,on_off,ifimage,haspic,haspic3
    on_off="off"
    t1 = Thread(target=upload_img)
    t1.start()
    # t2 = Thread(target=video_transport)
    # t2.start()
    print("fun_ok")
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
            time.sleep(0.05)
            counter=0
            while not haspic:
                time.sleep(0.15)
                counter+=1
                if counter>4:
                    break
            result1=FaceDetect(pic_file_path+"face_score.jpg", GetToken())  # 图片名：pic 文件夹里的 1.jpg
            WriteTxt('msg.txt', result1 + 'txt')

        if 'nameis' in temp:

            match_obj = re.match("nameis(\w+)", temp)  # 匹配名字
            if match_obj:
                name= match_obj.group(1)
            # for name in temp if name.startswith(('nameis'))
            # ifsave2 = True
            # time.sleep(0.05)
            # if ifimage:
            #     time.sleep(0.2)
            #     result2=FaceRegister(pic_file_path+str(user_id)+'.jpg',str(user_id),name, GetToken())
            #     WriteTxt('msg.txt', result2 + 'txt')
            # else:
            #     print("mei识别到人脸")
            #     WriteTxt('msg.txt', "未识别到人脸txt" )
            ifsave2 = True
            time.sleep(0.06)
            count=0
            while True:
                count+=1
                time.sleep(0.15)
                result2=FaceRegister(pic_file_path+str(user_id)+'.jpg',str(user_id),name, GetToken())
                if "注册成功" in result2:
                    print("注册成功")
                    WriteTxt('msg.txt', result2+ 'txt')
                    break
                if count>30:
                    print("注册超时")
                    WriteTxt('msg.txt', "注册超时txt")
                    ifsave2 = False
                    break



        if temp == '3':
            ifsave3 = True
            time.sleep(0.05)
            counter = 0
            while not haspic3:
                time.sleep(0.15)
                counter += 1
                if counter > 4:
                    break
            result3,haspic3=face_identif()
            WriteTxt('msg.txt', result3 + 'txt')

#####################################################################################





def get_pipeline():
    pipeline = rs.pipeline()
    config = rs.config()
    config.enable_stream(rs.stream.color, 1280, 720, rs.format.bgr8, 30)
    config.enable_stream(rs.stream.depth, 1280, 720, rs.format.z16, 30)
    pipeline.start(config)
    return pipeline



def get_540_pic():
    first_img = pipeline.wait_for_frames()
    color_frame = first_img.get_color_frame()
    frame1 = np.asanyarray(color_frame.get_data())
    frame = np.zeros((540, 540, 3), np.uint8)
    frame[:, :, 0] = frame1[91: 631, 371:911, 0]
    frame[:, :, 1] = frame1[91: 631, 371:911, 1]
    frame[:, :, 2] = frame1[91: 631, 371:911, 2]
    return  frame


def get_540_depth():
    frames = pipeline.wait_for_frames()
    align_to = rs.stream.color
    align = rs.align(align_to)
    aligned_frames = align.process(frames)
    depth_frame = aligned_frames.get_depth_frame()
    color_frame = aligned_frames.get_color_frame()
    depth_image = np.asanyarray(depth_frame.get_data())
    frame = np.zeros((540, 540), np.uint8)
    frame[:, :] = depth_image[91: 631, 371:911]
    frame = cv.resize(frame, (135, 135))
    depth_img1 = cv.medianBlur(frame, 19)
    color_image = np.asanyarray(color_frame.get_data())
    frame1 = np.zeros((540, 540, 3), np.uint8)
    frame1[:, :, 0] = color_image[91: 631, 371:911, 0]
    frame1[:, :, 1] = color_image[91: 631, 371:911, 1]
    frame1[:, :, 2] = color_image[91: 631, 371:911, 2]
    return  frame1, depth_img1

def chess_modeget():
     print('正在决定先后手！')
     while True:
            try:
                f = open('order.txt')
                b = f.read()
                if b == 'front':
                    f.close()
                    return '2'
                elif b == 'back':
                    f.close()
                    return '1'
                f.close()
            except:
                time.sleep(0.1)

            if try_read('on_off.txt','off') == 1:break

def rotation(img):

    M = cv.getRotationMatrix2D((270,270), 180, 1.0) #12
    rotated = cv.warpAffine(img, M, (540, 540)) #
    return  rotated

def get_mode():
    global video_pic
    count_remote = 0
    count_bottle = 0
    count_chess = 0
    count_mess = 0
    have_cup = False
    while True:  # 判断进入的mode ----->>>>> 1:遥控    2：下棋   3：倒水  4：杂物抓取
        cv.waitKey(30)
        frame = get_540_pic()
        rec.onlion_rec(frame)
        img2 = rec.add_bbox(frame)
        for tup in rec.objtuple:
            if tup[2] == 5: have_cup = True

        #print(rec.objtuple)
        for tuple in rec.objtuple:
            if tuple[2] == 1 :
                count_remote += 1
            elif tuple[2] == 4 and len(rec.objtuple) == 2 and have_cup: #
                count_bottle += 1
            elif tuple[2] == 6 :
                count_chess += 1
            elif tuple[2] == 2 :#and len(rec.objtuple)>1:
                count_mess += 1
            else:pass
        if count_remote >= 20:
            return 1
        elif count_chess >= 20:
            return 2
        elif count_bottle >= 20:
            return 3
        elif count_mess >= 20:
            return 4
        cv.imshow('mode_rec', img2)
        # cv.imwrite('me.jpg',img2)
        video_pic = rotation(img2)
        #vedio.get_frame(img2)#视频展示处！
        if try_read('on_off.txt','off'):break


def to_do_chess():
    write_file(file_path+'mod.txt','2')
    write_file(file_path+'tag.txt','0')
    chess_mode = chess_modeget()
    print('下棋模式，mode = %s'%chess_mode)
    #if mode == '1':sock.sent_posision(str(chess.x) +','+str(chess.y) +',2')
    robot_did = "0"
    count=0
    chess = GUI(chess_mode)
    if chess_mode == '1':
        chess_rec.ai_list.append((8 - chess.y, 8 - chess.x))
        time.sleep(4)
        write_file('msg.txt',str(4)+','+str(4)+',2chess')

    for i in range(100):
        if i < 1 or robot_did == '1':
            chess_rec.get_position(pipeline)
            if try_read('on_off.txt','off') == 1: break
            print("self.black_list", chess_rec.black_list)
            print('self.white_list', chess_rec.white_list)
            rst, out = chess_rec.output_point(27)
            if rst == True:
                try:
                    out_tuple = out.pop()  # 取出集合中的元素
                    outx, outy = int(out_tuple[0]), int(out_tuple[1])
                except:
                    print('出现一些小问题！')
                #sock.sent_posision(str(8-outy)+','+str(8 - outx)+',1')
                if chess_rec.robot_success == True and  chess.board.flag == 0:
                    try:
                        write_file('msg.txt',str(8 - outy)+','+str(8 - outx)+',1chess')
                        robotx,roboty = chess.down(8- outy, 8 - outx)
                        chess_rec.ai_list.append((8 - chess.y, 8 - chess.x))
                    except:print('出现一些小问题！！')
                # else:
                #     write_file(file_path + 'row_column.txt',str(chess.x)+','+str(chess.y) + ',1' )
            if rst == False:
                print('下的太歪了，请您重下！')
                chess_rec.len_w_and_b -= 1
                #sock.sent_posision(str(robotx) + ',' + str(roboty) + ',2')

            if chess.board.flag ==1:
                time.sleep(3)
                read_file(file_path+'tag.txt','1')
                if chess_mode == '2':
                    print('玩家胜利！')
                    write_file('msg.txt','wintxt')
                else:
                    print('AI胜利！')
                    write_file('msg.txt','losetxt')
                break
            elif chess.board.flag == 2:
                time.sleep(3)
                read_file(file_path+'tag.txt','1')
                if chess_mode == '2':
                    print('AI胜利！')
                    write_file('msg.txt','losetxt')
                else:
                    print('玩家胜利！')
                    write_file('msg.txt','wintxt')
                break
            elif chess.board.flag == 3:
                time.sleep(3)
                print('平局！！！')
                break

        # read_file(file_path+'tag.txt','1')
        # write_file(file_path+'tag.txt','2')
        robot_did = '1'
        # else:robot_did = '0'
    print("结束下棋！")
    chess_rec.restart()
    #chess.restart()


def to_do_remote():
    global video_pic
    write_file(file_path+'mod.txt','1')
    print('遥控模式')
    remote_over = False
    to_do_color = 0
    end_count = 0
    start1_remote = 0
    start2_remote = 0
    unrec = True
    while True:
        cv.waitKey(30)
        frame = get_540_pic()
        rec.onlion_rec(frame)
        img2 = rec.add_bbox(frame)
        video_pic = rotation(img2)
        cv.imshow('pic', img2) #视频展示处！
        if unrec:
            for tuple in rec.objtuple:
                if tuple[2] == 10:start1_remote += 1
                if tuple[2] == 11:start2_remote += 1
        else:start1_remote,start2_remote = 0,0

        if start2_remote > 40 or start1_remote >40:remote_over=True
        if try_read(file_path+'flag.txt','1') == 1:
            robot_did =True
        else:robot_did = False
        if remote_over:
            for tuple in rec.objtuple:
                if tuple[2] == 10:
                    to_do_color = 1
                    remote_over =False
                    unrec =False
                    chess_rec.need2getreal.append((tuple[0], tuple[1]))
                    realx, realy = chess_rec.get_real()
                    print('杯子的位置为:',realx,realy)
                    write_file(file_path+'cupsetoff.txt', str(realx)+','+ str(realy)+',1,1')
                    robot_did = True
                    print('put down!!')
                    break
                elif tuple[2] == 11:
                    to_do_color = 2
                    remote_over = False
                    unrec = False
                    chess_rec.need2getreal.append((tuple[0], tuple[1]))
                    realx, realy = chess_rec.get_real()
                    print('杯子的位置为:',realx,realy)
                    write_file(file_path+'cupsetoff.txt', str(realx)+','+ str(realy)+',1,1')
                    robot_did = True
                    cv.waitKey(500)
                    print('put down!!')
                    break

        if to_do_color == 1 and robot_did == True and end_count < 10:
            count = 0
            print('开始抓取对应颜色！')
            for tuple in rec.objtuple:
                if tuple[2] == 7: count += 1
            if count == 0:
                end_count += 1
            for tuple in rec.objtuple:
                if tuple[2] == 7:
                    chess_rec.need2getreal.append((tuple[0], tuple[1]))
                    realx, realy = chess_rec.get_real()
                    print(realx,realy)
                    write_file(file_path+'setoff.txt', str(realx)+','+ str(realy)+',1,1')
                    write_file(file_path+'flag.txt','0')
                    break

        if to_do_color == 2 and robot_did == True and end_count < 10:
            count = 0
            print('开始抓取对应颜色！')
            for tuple in rec.objtuple:
                if tuple[2] == 8: count += 1
            if count == 0:
                end_count += 1
            for tuple in rec.objtuple:
                if tuple[2] == 8:
                    chess_rec.need2getreal.append((tuple[0], tuple[1]))
                    realx, realy = chess_rec.get_real()
                    print(realx,realy)
                    write_file(file_path+'setoff.txt', str(realx)+','+ str(realy)+',1,1')
                    write_file(file_path+'flag.txt','0')
                    break
        if try_read('on_off.txt','off'):
            break
        if end_count == 10:
            print('已经没有对应东西了！结束')
            break


    print('遥控结束！')
    #等待命令，对即将来到的进行识别



def to_do_water():
    global video_pic
    print('倒水模式！')
    write_file(file_path+'mod.txt','3')
    bottle_list = []
    cup_list = []
    count_bottle = 0
    count_cup = 0
    tag = True
    tag1 =False
    end = False
    while True:
        key = cv.waitKey(30)
        frame = get_540_pic()
        rec.onlion_rec(frame)
        img2 = rec.add_bbox(frame)

        # print(rec.objtuple)#识别到的tuple
        cv.imshow('pic', img2) #视频展示处！
        video_pic = rotation(img2)
        for tuple in rec.objtuple:
            if tuple[2] == 4:
                x = tuple[0]
                y = tuple[1]
                bottle_list.append((x,y))
                count_bottle += 1
            if tuple[2] == 5:
                x = tuple[0]
                y = tuple[1]
                cup_list.append((x, y))
                count_cup += 1
        if count_bottle>8 and abs(bottle_list[count_bottle-1][0]- bottle_list[count_bottle-2][0])< 4 and  abs(bottle_list[count_bottle-1][1]- bottle_list[count_bottle-5][1])< 4 and tag:
            chess_rec.need2getreal.append(bottle_list[count_bottle-1])
            realx, realy = chess_rec.get_real()
            print(realx,realy)
            tag = False

            while True:
                try:
                    f = open(file_path+'setoff.txt', 'w')
                    f.write(str(realx) + ',' + str(realy) + ',1,2' )
                    f.close()
                    f = open(file_path+'flagg.txt', 'w')
                    f.write('0')
                    f.close()
                    # print(tag1)
                    time.sleep(1)
                    break
                except:
                    time.sleep(0.1)
                if try_read('on_off.txt','off') == 1:break

        if  try_read(file_path+'flagg.txt','1') == 1:
            tag1 = True
        print('after read',tag1)
        if tag1 and tag ==False and count_cup>5: #and abs(cup_list[count_cup-3][0]- cup_list[count_cup-4][0])< 4 and  abs(cup_list[count_cup-3][1]- cup_list[count_cup-4][1])< 4:
            chess_rec.need2getreal.append(cup_list[count_cup - 4])
            realx, realy = chess_rec.get_real()
            while True:
                try:
                    f = open(file_path+'setoff.txt', 'w')
                    f.write(str(realx) + ',' + str(realy) + ',1,2' )
                    print(str(realx) + ',' + str(realy) + ',' + '1')
                    f.close()
                    f = open(file_path+'flagg.txt', 'w')
                    f.write('0')
                    f.close()
                    end = True
                    tag1=False
                    break
                except:
                    time.sleep(0.1)
                    pass
        if try_read('on_off.txt','off') == 1:
            print('强制结束倒水！')
            break
        if try_read(file_path + 'ifend.txt','1')==1:robot_end = 1
        else:robot_end = 0
        if end == True and robot_end == 1:
            print("倒水结束！")
            cv.destroyAllWindows()
            time.sleep(3)
            break






def to_do_obj_grab():
    global video_pic
    print('物品抓取')
    flat_list = [] #选取扁平物的坐标
    not_flat_list =[] #只能用大手抓的目标坐标
    did_it = False #初始化进入识别模式
    add_count = 0
    add_count1 = 0
    count_begin = False
    count1_begin = False
    Go_depth  =False
    loop = 0
    loop1 = 0
    switch_tag = 0 #初始化为吸取物体
    write_file(file_path+'mod.txt','4')
    end_count = 0
    while True:
        cv.waitKey(30)
        color, depth = get_540_depth()
        rec.onlion_rec(color)
        img = rec.add_bbox(color)
        video_pic = rotation(img)
        # print(rec.objtuple)#识别到的tuple
        # cv.imshow('pic', img2)
        depth_rec = TargetRecognition(3000, 185)
        img1 = depth_rec.target(img, depth)
        cv.imshow('recgnize',img1)
        if not did_it:
            if len(depth_rec.tuplelist) == 0 and len(rec.objtuple) == 0:
                print("桌面上啥也没有！！")
                need_to_do = False
            else:need_to_do = True
            ###手动消除部分时候反光的影响
            if len(depth_rec.tuplelist) > 0:
                if len(not_flat_list) == 0:
                    count1_begin = True
                    not_flat_list.append(depth_rec.tuplelist[0])
                    add_count1 = 1
                else:
                    for tuple in depth_rec.tuplelist:
                        if abs(tuple[0] - not_flat_list[0][0])<15 and abs(tuple[1] - not_flat_list[0][1])<15:
                            not_flat_list.append(tuple)
                            add_count1 += 1


            if count1_begin: loop1 += 1
            if add_count1 >= 6:
                Go_depth = True
            if  loop1 > 20 and add_count1 < 7:
                count1_begin =False
                add_count1 = 0
                not_flat_list = []
                Go_depth = False
                loop1 = 0
            if (need_to_do and switch_tag == 1 and Go_depth)or ( Go_depth and switch_tag ==0  and len(rec.objtuple) == 0 and need_to_do):
                tuple = not_flat_list[3]
                chess_rec.need2getreal.append(tuple)
                realx, realy = chess_rec.get_real()
                while True:
                    try:
                        f = open(file_path+'setoff.txt', 'w')
                        f.write(str(realx) + ',' + str(realy) + ',1,2')
                        print(realx,realy)
                        print('已经写好了杂物的坐标了！！！！   ')
                        f.close()
                        did_it = True
                        count1_begin =False
                        add_count1 = 0
                        not_flat_list = []
                        Go_depth = False
                        loop1 = 0
                        switch_tag = 0
                        break
                    except:
                        time.sleep(0.1)
                        pass


            if   (need_to_do and  switch_tag == 0) or (need_to_do and len(depth_rec.tuplelist)==0):
                for tuple in rec.objtuple:
                    if (tuple[2] == 2 or tuple[2] == 7 or tuple[2] == 8) and len(flat_list) == 0:
                        count_begin = True
                        add_count =  1
                        flat_list.append((tuple[0], tuple[1]))
                    elif  (tuple[2] == 2 or tuple[2] == 7 or tuple[2] == 8) and not len(flat_list) == 0 and abs(
                            tuple[0] - flat_list[0][0]) < 5 and abs(tuple[1] - flat_list[0][1]) < 5:
                        add_count += 1
                        flat_list.append((tuple[0], tuple[1]))

                if loop > 10 and  add_count <8:
                    flat_list = []
                    add_count =0
                    loop =0
                    count_begin =False
                if add_count == 6:
                    chess_rec.need2getreal.append(flat_list[3])
                    realx, realy = chess_rec.get_real()
                    while True:
                        try:
                            f = open(file_path+'setoff.txt', 'w')
                            f.write(str(realx) + ',' + str(realy) + ',1,1')
                            print(realx,realy)
                            print('已经写好了扁平物体的坐标了！！！！   ')
                            f.close()
                            add_count = 0
                            flat_list = []
                            count_begin = False
                            loop = 0
                            switch_tag =1
                            did_it = True
                            break
                        except:
                            time.sleep(0.1)
                            pass
            if count_begin:loop += 1
        if try_read(file_path+'flag.txt','1'):
            did_it = False
            write_file(file_path+'flag.txt','0')

        if try_read('on_off.txt','off')==1:break
        if len(depth_rec.tuplelist) +  len(rec.objtuple) == 0:
            print('暂无！')
            end_count += 1
        if end_count > 6:
            print('结束抓取')
            # time.sleep(2)
            break




def main_func():
    global video_pic
    while True:
        write_file(file_path + 'ifend.txt','0')
        write_file('order.txt','0')
        write_file('mod.txt','0')
        cv.destroyAllWindows()
        mode = get_mode()
        video_pic = np.zeros((540, 540,3), np.uint8)
        if not mode == None:
            write_file('msg.txt','mod'+str(mode) + 'mode')
            write_file('mod.txt',str(mode))
        if mode == 1:
            to_do_remote()
        elif mode == 2:
            to_do_chess()
        elif mode == 3:
            to_do_water()
        elif mode == 4:
            to_do_obj_grab()
        else:pass
        cv.destroyAllWindows()
        if not mode == 2:write_file('msg.txt','endmode')
        video_pic = np.zeros((540, 540,3), np.uint8)
        print('正在读取ifend!')
        read_file(file_path+'ifend.txt','1')
        print('读到ifend为1！')
        write_file(file_path+'mod.txt','0')
        if try_read('on_off.txt','off') == 1:break


def write_file(fpath,content ):
    while True:
        try:
            f = open(fpath,'w')
            f.write(content)
            f.close()
            break
        except:
            time.sleep(0.1)

def read_file(fpath, break_content): #循环阻塞
    while True:
        try:
            f = open(fpath)
            t = f.read()
            if t == break_content:
                f.close()
                return 1

            else:f.close()
        except:time.sleep(0.1)
        if not fpath == 'on_off.txt' and try_read('on_off.txt','off')==1:break

def try_read(fpath, flag_content):
    try:
        f = open(fpath)
        t = f.read()
        if t == flag_content:
            f.close()
            return 1
        else:
            f.close()
            return 0
    except:return 0

def read_txt(filename):
    content="0"
    try:
        f = open(filename, "r")  # 设置文件对象
        content = f.read()  # 将txt文件的所有内容读入到字符串str中
        f.close()  # 将文件关闭
    except:
        print("读TXT出错")
    return content



#####################实现网页推送视频流###########################################

@app.route('/')
def index():
    # jinja2模板，具体格式保存在index.html文件中
    return render_template('index.html')

# 使用generator函数输出视频流， 每次请求输出的content类型是image/jpeg
# 同时生成时间序列
def gen_img():
    frame = np.zeros((540, 540,3), np.uint8)
    global video_pic
    video_pic =frame
    while True:
        # t=time.time()
        # cv.waitKey(30)
        time.sleep(0.03)
        ret, jpeg = cv.imencode('.jpg', video_pic)
        #print(jpeg.shape)
        # print(time.time()-t)
        yield (b'--frame\r\n'
               b'Content-Type: image/jpeg\r\n\r\n' + jpeg.tobytes() + b'\r\n\r\n')
# 返回视频流响应
@app.route('/video_feed')
def video_feed():
    return Response(gen_img(),
                    mimetype='multipart/x-mixed-replace; boundary=frame')
@app.route('/favicon.ico')
def favicon():
    return current_app.send_static_file('static/favicon.ico')

def app_run():
    app.run(host='0.0.0.0', threaded=True, port=50000)
################################################################################


if __name__ == "__main__":

    threading.Thread(target=app_run).start() #网页视频推送线程
    global video_pic,K,video_piccc
    K = KINECT()
    time.sleep(2)
    file_path ='C:\\Users\\myoasm\\Desktop\\zuobiao\\'
    write_file('on_off.txt','off')
    write_file(file_path + 'mod.txt', '0')
    pipeline = get_pipeline()
    rec = Online_rec()#实例化在线识别

    while True: #初始棋盘矫正
        try:
            frame = get_540_pic()
            chess_rec = Recognize(frame) #实例化棋盘识别以便矫正
            ret, list = chess_rec.board_rectify(7)
            if ret == True:break
        except: pass

    while True:
        cv.destroyAllWindows()

        if read_txt('on_off.txt') == "on":
            write_file('status.txt','1')
            print('机械臂模式！')
            main_func()

        if read_txt('on_off.txt')== 'off':
            write_file('status.txt','0')
            print('人脸交互模式')
            # test_up_image()
            face_fuc()   #黑屏了先检查插头，而不是代码  懂？

        # read_file('on_off.txt','on') #阻塞读取
        # print('recognizing！！')






        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
