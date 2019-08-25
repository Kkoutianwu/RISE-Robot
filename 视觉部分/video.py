from flask import Flask, render_template, Response, flash, session, current_app
from flask_bootstrap import Bootstrap
import pyrealsense2 as rs
import time
import numpy as np
import cv2
import os
import threading
#from datetime import timedelta, time, datetime

app = Flask(__name__)
app.config['SECRET_KEY'] = os.urandom(24)
#app.config['PERMANENT_SESSION_LIFETIME'] = timedelta(hours=50)
bootstrap=Bootstrap(app)


def get_pipeline():
    pipeline = rs.pipeline()
    config = rs.config()
    config.enable_stream(rs.stream.color, 1280, 720, rs.format.bgr8, 30)
    #config.enable_stream(rs.stream.depth, 1280, 720, rs.format.z16, 30)
    pipeline.start(config)
    return pipeline

def get_540_pic(pipeline):
    first_img = pipeline.wait_for_frames()
    color_frame = first_img.get_color_frame()
    frame1 = np.asanyarray(color_frame.get_data())
    frame = np.zeros((540, 540, 3), np.uint8)
    frame[:, :, 0] = frame1[91: 631, 371:911, 0]
    frame[:, :, 1] = frame1[91: 631, 371:911, 1]
    frame[:, :, 2] = frame1[91: 631, 371:911, 2]
    return  frame

# 通过opencv获取实时视频流
class VideoCamera(object):
    def __init__(self):
        # self.video = cv2.VideoCapture(0)
        # self.time = datetime.now()
        #self.pip = get_pipeline()
        pass
        #
    def __del__(self):
        # self.video.release()
        # self.time.release()
        pass

    def get_frame(self,pip):
        # success, image = self.video.read()
        image = get_540_pic(pip)
        # time.sleep(1)
        # 因为opencv读取的图片并非jpeg格式，因此要用motion JPEG模式需要先将图片转码成jpg格式图片
        ret, jpeg = cv2.imencode('.jpg', image)
        return jpeg.tobytes()

# 主页

@app.route('/')
def index():
    # jinja2模板，具体格式保存在index.html文件中
    return render_template('index.html')

# 使用generator函数输出视频流， 每次请求输出的content类型是image/jpeg
# 同时生成时间序列
def gen_img(camera):
    while True:
        yield (b'--frame\r\n'
               b'Content-Type: image/jpeg\r\n\r\n' + camera.get_frame(pip) + b'\r\n\r\n')

# 返回视频流响应
@app.route('/video_feed')
def video_feed():
    return Response(gen_img(VideoCamera()),
                    mimetype='multipart/x-mixed-replace; boundary=frame')

@app.route('/favicon.ico')
def favicon():
    return current_app.send_static_file('static/favicon.ico')

if __name__ == '__main__':
    pip = get_pipeline()
    threading.Thread(target=app.run).start()
    #app.run(host='0.0.0.0', threaded=True, port=5000)
    for i in range(1000):
        print(i)
    # serve(app, listen='*:5000', threads=2)
