import time


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


while True:
    WriteTxt("on_off.txt","on")
    time.sleep(1)
    WriteTxt("on_off.txt","off")
    time.sleep(1)
