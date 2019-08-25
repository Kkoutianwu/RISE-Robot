#!/usr/bin/env python
# -*- coding: utf-8 -*-
import requests
import time
import base64
from playsound import playsound
from time import sleep
from aip import AipSpeech
from numpy import frombuffer,short
import wave
from pyaudio import PyAudio,paInt16
import urllib
from urllib.parse import quote
import re
import os

framerate = 16000  # 采样率
num_samples = 2000  # 采样点
channels = 1  # 声道s
sampwidth = 2  # 采样宽度2bytes
voice_name = 'speech.wav'
file_path='voice\\'

base_url = "https://openapi.baidu.com/oauth/2.0/token?grant_type=client_credentials&client_id=%s&client_secret=%s"

APP_ID = '16926682'
APIKey = "x1bRgxw59iIfW2l746OVvPkp"
SecretKey = "rO8OyIMkYM9Orak1G8cwMIpB6mjdnbIh"

HOST = base_url % (APIKey, SecretKey)




def getToken(host):
    res = requests.post(host)
    return res.json()['access_token']


def save_wave_file(filepath, data):
    wf = wave.open(filepath, 'wb')
    wf.setnchannels(channels)
    wf.setsampwidth(sampwidth)
    wf.setframerate(framerate)
    wf.writeframes(b''.join(data))
    wf.close()




def my_record():
    pa = PyAudio()
    stream = pa.open(format=paInt16, channels=channels,
                     rate=framerate, input=True, frames_per_buffer=num_samples)
    my_buf = []
    # count = 0
    t = time.time()
    print('正在等待对话...')
    coutif = 0
    while True:  # 秒
        string_audio_data = stream.read(num_samples)
        data_list = frombuffer(string_audio_data, dtype=short)
        if max(data_list) > 30000:
            playsound(file_path+'start.mp3')
            print('开始录音————————————————————————')
            while time.time() < t + 60:  # 秒
                string_audio_data = stream.read(num_samples)
                data_list = frombuffer(string_audio_data, dtype=short)
                my_buf.append(string_audio_data)
                # 判断是否有声音
                if max(data_list) < 20000:  # 5000:分贝阈值，小于5000视为环境噪音或静音
                    coutif += 1
                else:
                    coutif = 0
                # 如果连续15个采样点都小于5000，退出循环，即连续1/16000*2000*15=1.875秒没声，就不录音了
                if coutif > 15:
                    break
            break
    playsound(file_path+'end.mp3')
    print('准备识别中—————————————————————')
    save_wave_file(voice_name, my_buf)
    stream.close()


def get_audio(file):
    with open(file, 'rb') as f:
        data = f.read()
    return data


def speech2text(speech_data, token, dev_pid=1537):
    FORMAT = 'wav'
    RATE = '16000'
    CHANNEL = 1
    CUID = '*******'
    SPEECH = base64.b64encode(speech_data).decode('utf-8')

    data = {
        'format': FORMAT,
        'rate': RATE,
        'channel': CHANNEL,
        'cuid': CUID,
        'len': len(speech_data),
        'speech': SPEECH,
        'token': token,
        'dev_pid': dev_pid
    }
    url = 'https://vop.baidu.com/server_api'
    headers = {'Content-Type': 'application/json'}
    # r=requests.post(url,data=json.dumps(data),headers=headers)
    print('正在识别...')
    r = requests.post(url, json=data, headers=headers)
    Result = r.json()
    if 'result' in Result:
        return Result['result'][0]
    else:
        return Result


#语音合成方法
def art_speech(speech_Info,sound_name):
    result = client.synthesis(speech_Info, 'zh', 4, {'spd':5, 'pit':5,'vol':15,'per':110})
    A = isinstance(result, dict)
    if not A:
        with open(file_path+sound_name+'.mp3', 'wb') as f:
          f.write(result)
        f.closed
        sleep(0.1)
    return A


def write_txt(filename ,content):
    b = 1
    while b < 2:
        try:
            with open(filename, 'w') as f:  # 如果filename不存在会自动创建， 'w'表示写数据，写之前会清空文件中的原有数据！
                f.write(str(content))
                b=3
            f.close()
        except:
            b = 1
            time.sleep(0.1)
            print("未能写入")
def read_txt(filename):
    try:
        f = open(filename, "r")  # 设置文件对象
        content = f.read()  # 将txt文件的所有内容读入到字符串str中
        f.close()  # 将文件关闭
    except:
        print("读mod出错")
    return content

if __name__ == '__main__':
    speech_length=500
    counter=0
    client = AipSpeech(APP_ID, APIKey, SecretKey)
    while True:
        my_record()
        TOKEN = getToken(HOST)
        speech = get_audio(voice_name)
        voice_result = speech2text(speech, TOKEN, 1536)  #识别到得语音结果,1536：普通话(简单英文),1537:普通话(有标点),1737:英语,1637:粤语,1837:四川话
        try:
            voice_result_content=voice_result.strip('，')
            print(voice_result_content)
            if "名字" in voice_result_content:
                playsound(file_path+'name.mp3')
            elif "傻逼" in voice_result_content:
                playsound(file_path+'sb.mp3')
            elif "请开始" in voice_result_content:
                write_txt('on_off.txt', 'on')
                playsound(file_path+'start_recognition.mp3')
                t = time.time()
                while True:
                    mod=read_txt('C:\\Users\\myosam\\Desktop\\zuobiao\\mod.txt')
                    time.sleep(0.1)
                    if mod=='2':
                        playsound(file_path + 'order.mp3')
                        break
                    if time.time()> t+3:
                         break
                while True:
                    my_record()
                    TOKEN = getToken(HOST)
                    speech = get_audio(voice_name)
                    voice_result = speech2text(speech, TOKEN, 1536)  # 识别到得语音结果
                    try:
                        voice_result_content = voice_result.strip('，')
                        print(voice_result_content)
                        if "先手" in voice_result_content:
                            write_txt('order.txt','front')
                            playsound(file_path + 'chess_start.mp3')
                        if "后手" in voice_result_content:
                            write_txt('order.txt','back')
                            playsound(file_path + 'chess_start.mp3')
                        if "请结束" in voice_result_content:
                            write_txt('on_off.txt','off')
                            playsound(file_path + 'end_recognition.mp3')
                            break
                    except:
                        print("有一些无关紧要的错误")
            else:
                msg = quote(voice_result_content.encode('utf-8'))
                response = urllib.request.urlopen('http://api.qingyunke.com/api.php?key=free&appid=0&msg=' + msg)  #HTTP聊天请求
                chat_result = response.read().decode('utf-8', 'replace')
                print(chat_result)
                chat_result_content = re.compile(r'\"(.*?)\"') .findall(chat_result)[2]  #匹配内容部分
                # print(chat_result_content)
                match_obj = re.match(".*?([\u4E00-\u9FA5]+.*)", chat_result_content)  # 匹配中文开头
                if match_obj:
                    chat_result_content_zh=match_obj.group(1)
                    if '菲菲' in chat_result_content_zh:
                        chat_result_content_zh=chat_result_content_zh.replace('菲菲', '铁憨憨')  #换名字
                    print(chat_result_content_zh)
                    if len(chat_result_content_zh)<speech_length:
                        sound_name=str(counter)
                        counter= counter + 1
                        ifre=art_speech(chat_result_content_zh, sound_name)
                        if ifre:
                            sleep(1)
                            os.remove(file_path+sound_name + '.mp3')
                            sound_name = str(counter)
                            counter = counter + 1
                            ifre = art_speech(chat_result_content_zh, sound_name)
                            if ifre:
                                playsound(file_path+'net_error.mp3')
                        playsound(file_path+sound_name+'.mp3')
                        os.remove(file_path+sound_name+'.mp3')
                    else:
                        playsound(file_path+'toomore.mp3')
                else:
                    playsound(file_path+'toodifficult.mp3')

        except:
            print("未识别到语音")


