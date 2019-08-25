import time

def read_file(fpath):
    while True:
        try:
            f = open(fpath,'r')
            t = f.read()
            f.close()
            if not t == '0' and not len(t) == 0:
                write_file('msg.txt','0')

                return t
            else:pass
        except:time.sleep(0.1)


def write_file(fpath,content ):
    while True:
        try:
            f = open(fpath,'w')
            f.write(content)
            f.close()
            break
        except:
            time.sleep(0.1)

msg = read_file('msg.txt')

print('msg',msg)
