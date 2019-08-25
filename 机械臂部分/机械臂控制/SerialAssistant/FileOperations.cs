using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SerialAssistant
{
    class FileOperations
    {
        public string filePath;
        public FileOperations(string filePath)
        {
            this.filePath = filePath;
        }
        public void GetSetoff(string str, out double x, out double y, out int flag)
        {
            string[] angl = Regex.Split(str, ",", RegexOptions.IgnoreCase);
            x = (Convert.ToDouble(angl[0]));
            y = (Convert.ToDouble(angl[1]));
            flag = (Convert.ToInt32(angl[2]));
        }
        public void GetSetoff(string str, out double x, out double y, out double z)
        {
            string[] angl = Regex.Split(str, ",", RegexOptions.IgnoreCase);
            x = (Convert.ToDouble(angl[0]));
            y = (Convert.ToDouble(angl[1]));
            z = (Convert.ToDouble(angl[2]));
        }
        public void GetSetoff(string str, out int flag)
        {
            flag = (Convert.ToInt32(str));
        }
        public void GetSetoff(string str, out double x, out double y, out int flag,out int id)
        {
            string[] angl = Regex.Split(str, ",", RegexOptions.IgnoreCase);
            x = (Convert.ToDouble(angl[0]));
            y = (Convert.ToDouble(angl[1]));
            flag = (Convert.ToInt32(angl[2]));
            id = (Convert.ToInt32(angl[3]));
        }
        public string ReadTxt(string fileName)//读TXT，直到读成功为止
        {
            string content = null;
            bool i = false;
            while (!i)
            {
                try
                {
                    StreamReader sr = new StreamReader(filePath + "\\" + fileName + ".txt", false);
                    content = sr.ReadLine().ToString();
                    sr.Close();
                    i = true;
                }
                catch
                {
                    System.Threading.Thread.Sleep(20);
                }
            }
            return content;
        }
        public void WriteTxt(string fileName, string content) //写TXT,直到写成功为止
        {
            bool i = false;
            while(!i)
            { 
                try
                {
                    FileStream fs = new FileStream(filePath + "\\" + fileName + ".txt", FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write(content);  //开始写入
                    sw.Flush();//清空缓冲区
                    sw.Close();//关闭流
                    fs.Close();
                    i = true;
                }
                catch
                {
                    System.Threading.Thread.Sleep(100);
                }
            }
        }
        public void OpenProcess(string name)//打开进程
        {
            System.Diagnostics.Process.Start(filePath + @"\openni\x64\Debug\" + name + ".exe");

        }
        public void KillProcess(string name)//关闭进程
        {
            System.Diagnostics.Process myproc = new System.Diagnostics.Process();//得到所有打开的进程   
            try
            {
                foreach (Process thisproc in Process.GetProcessesByName(name)) //找到程序进程,kill之。
                {
                    if (!thisproc.CloseMainWindow())
                    {
                        thisproc.Kill();
                    }
                }

            }
            catch
            {
                Thread.Sleep(50);
            }

        }
    }
}
