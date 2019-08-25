import socket
import threading
import time
# 创建socket对象
s = socket.socket()
# 连接远程主机
s.connect(('127.0.0.1', 30000))
def read_from_server(s):
    i = 0
    while True:
        content = s.recv(2048).decode('utf-8')
        cont = content.split()
        print(cont)
        s.sendall((cont[0].rjust(10) + '8'.rjust(10) + '0'.rjust(10)).encode() )
        # if '7' in content:
        #     #print('heart:',i)
        #     s.sendall('heart'.encode())
        if '1,1,1' in content:
            print('chess')
        # i += 1
# 客户端启动线程不断地读取来自服务器的数据
def send_to_server(s):
    while True:
        # s.send(input('请输入命令：').encode())
        # print('已输入')
        time.sleep(1)
threading.Thread(target=read_from_server, args=(s, )).start()   # ①
threading.Thread(target=send_to_server, args=(s, )).start()
