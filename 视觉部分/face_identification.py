from aip import AipFace
import base64
import time,os

""" 你的 APPID AK SK """
APP_ID = '16949290'
API_KEY = 'R0H8ynY7MVFoqYFjRrvGF4DD'
SECRET_KEY = 'b8d0QKMv9uI0mhXEzCmBAPz3BLcNQQLF'
client = AipFace(APP_ID, API_KEY, SECRET_KEY)

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

def face_identif():
	try:
		filePath = "pic\\who.jpg"
		with open(filePath,"rb") as f:
			base64_data = base64.b64encode(f.read())

		image = str(base64_data,'utf-8')
		imageType = "BASE64"
		os.remove(filePath)
		groupIdList = "male"

		""" 调用人脸搜索 """
		content = client.search(image, imageType, groupIdList);
		# print(content)
		if "SUCCESS" in content['error_msg']:
			user_name=content['result']['user_list'][0]['user_info']
			print(user_name)
			return user_name,False
		else:
			print("error")
			return "error",False

	except:
		print("未识别到人脸")
		WriteTxt('msg.txt', "未识别到人脸")
		return "未识别到人脸",True

