from socket import *
import threading
import time
import inspect
import ctypes
import  random

class Socekt_connect():

    def __init__(self):   #socket的连接创建
        self.HOST = ''
        self.PORT = 3000
        self.BUFSIZ = 1024
        self.addr = (self.HOST, self.PORT)
        self.send_list = []
        self.Sockt = socket(AF_INET, SOCK_STREAM)
        self.cid = 0
        self.send_list = []
        self.Sockt.setsockopt(SOL_SOCKET, SO_REUSEADDR, 1)
        self.Sockt.bind(self.addr)
        print('服务器已经启动！')
        self.Sockt.listen(5)

    def start_wait(self):
        while True:
            print('等待连接！')
            global Conn,client_addr
            Conn,client_addr = self.Sockt.accept()
            sta = read_txt('status.txt')
            mod = read_txt('mod.txt')
            Conn.sendall((str(self.cid+451).rjust(10) + '9'.rjust(10) + '3'.rjust(10) + str(sta) + ',' + str(mod)).encode())
            print('已经发送当前工作状态并建立连接！')


    def heart(self):
        #心跳
        TYPE = '7'
        LEN = '0'
        while True:
            try:
                CID = str(self.cid)
                Conn.sendall((CID.rjust(10) + TYPE.rjust(10) + LEN.rjust(10)).encode())
                #print(CID)
                self.send_list.append((CID,TYPE,LEN))
                time.sleep(3)
                self.cid += 1
                if self.cid > 10000: self.cid = 0
            except :
                print('心跳失败！')
                time.sleep(1)

    def sendanything(self):
        while True:
            try:
                CID = str(self.cid + 1000)
                msg = read_file('msg.txt')
                print('msg',msg)
                #write_file('msg.txt','0')
                if msg.endswith('chess'):
                    TYPE = '3'
                    LEN =  str(len(msg)-5)
                    Conn.sendall((CID.rjust(10) + TYPE.rjust(10) + LEN.rjust(10)+ msg[:-5]).encode())
                    self.send_list.append((CID,TYPE,LEN,msg[:-5]))
                    print('已发送棋盘')
                elif msg.endswith('mode'):
                    TYPE = '4'
                    LEN = str(len(msg)-4)
                    Conn.sendall((CID.rjust(10) + TYPE.rjust(10) + LEN.rjust(10) + msg[:-4]).encode())
                    self.send_list.append((CID,TYPE,LEN,msg[:-4]))
                    print('已发送模式')
                elif msg.endswith('txt'):
                    TYPE = '6'
                    LEN = str(len(msg.encode())-3)
                    Conn.sendall((CID.rjust(10) + TYPE.rjust(10) + LEN.rjust(10) + msg[:-3]).encode())
                    self.send_list.append((CID,TYPE,LEN,msg[:-3]))
                    print('已发送文本')
                self.cid += 1
            except :
                time.sleep(1)
                print('出现发送问题')


    def receive(self):
        #接收消息
        while True:
            try:

                recv_data = Conn.recv(1024).decode('utf-8')
                print(self.send_list)
                recv = recv_data.split()
                print('收到',recv)
                if len(recv)>0:
                    if not recv[1] == '8':
                        CID = recv[0]
                        Conn.sendall((CID.rjust(10) + '8'.rjust(10) + '0'.rjust(10)).encode())
                        print('已返回！')
                    if recv[1] == '8':#去除列表里堆积的发送元组
                        for tuple in self.send_list:
                            if tuple[0] == recv[0]:self.send_list.remove(tuple)
                    print(len(self.send_list))
                    if len(recv) == 4:
                        if recv[3] == 'front':
                            write_file('order.txt','front')
                            print('front')
                        elif recv[3] == 'on':
                            write_file('on_off.txt','on')
                            print('on')
                        elif recv[3] == 'off':
                            write_file('on_off.txt','off')
                            print('off')
                        elif recv[3] == 'back':
                            write_file('order.txt', 'back')
                            print('back')
                        elif recv[3] == 'face_score':
                            write_file('face.txt', '1')
                            print(recv[3])
                            print('写好颜值！')

                        elif 'hello' in recv[3]:
                            print('招手！')
                            write_file(file_path +'action_mod.txt',str(random.choice([1,2])) )
                        elif 'nameis' in  recv[3]:
                            write_file('face.txt',recv[3])
                        elif recv[3] == 'name':  #用户询问我叫什么名字
                            write_file('face.txt','3')
                        elif recv[3] == 'bye':
                            write_file(file_path +  'action_mod.txt','2')
                        elif recv[3] == 'circle' or '画圈圈' in  recv[3]:
                            write_file(file_path +'action_mod.txt','3')
            except :
                time.sleep(1)
                print('出现接收问题！')


def write_file(fpath,content ):
    while True:
        try:
            f = open(fpath,'w')
            f.write(content)
            f.close()
            break
        except:
            time.sleep(0.1)

def read_file(fpath):
    while True:
        try:
            f = open(fpath,'r')
            t = f.read()
            f.close()
            if not t.startswith('x')  and not len(t) == 0:
                write_file(fpath, 'x')
                return t
            else:pass
        except:time.sleep(0.1)

def read_txt(fpath):
    while True:
        try:
            f =  open(fpath,'r')
            t = f.read()
            f.close()
            return t
        except:time.sleep(0.1)

def _async_raise(tid, exctype):
    """raises the exception, performs cleanup if needed"""
    tid = ctypes.c_long(tid)
    if not inspect.isclass(exctype):
        exctype = type(exctype)
    res = ctypes.pythonapi.PyThreadState_SetAsyncExc(tid, ctypes.py_object(exctype))
    if res == 0:
        raise ValueError("invalid thread id")
    elif res != 1:
        ctypes.pythonapi.PyThreadState_SetAsyncExc(tid, None)
        raise SystemError("PyThreadState_SetAsyncExc failed")

def stop_thread(thread):
    _async_raise(thread.ident, SystemExit)

def multi_thread(soc):
    t_conn = threading.Thread(target=soc.start_wait)
    t_heart = threading.Thread(target=soc.heart)
    t_receive = threading.Thread(target=soc.receive)
    t_send = threading.Thread(target=soc.sendanything)
    t_conn.start()
    t_heart.start()
    t_receive.start()
    t_send.start()


if __name__ == '__main__':
    global Conn,client_addr
    file_path ='C:\\Users\\myoasm\\Desktop\\zuobiao\\'
    soc = Socekt_connect()
    multi_thread(soc)




