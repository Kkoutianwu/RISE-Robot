using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SerialAssistant
{
    public struct SendVar  //串口发送变量
    {
        private const int maxAngle = 2500;
        private const int minAngle = 500;
        private const int maxTimes = 2500;
        private const int minTimes = 300;
        private int angle;
        private int times;

        public int Id { get; set; }
        public int Angle
        {
          get{return angle;}
          set{if (value > maxAngle) value = maxAngle; if (value < minAngle) value = minAngle; angle = value; }
        }
        public int Times
        {
            get { return times; }
            set { if (value > maxTimes) value = maxTimes; if (value < minTimes) value = minTimes; times = value; }
        }

    };

    class Msg
    {
        private Form1 form;
        public Msg(Form1 f)
        {
            form = f;
        }
        public void sendmsg(string str,int num)
        {
            form.SendMsg(str, num);

        }
        public void sendmsg(int times,int num, int angle0 = 1500, int angle1 = 1500, int angle2 = 1500, int angle3 = 1500, int angle4 = 1500, int angle5 = 1500)
        {
            form.SendMsg("{#000P" + angle0.ToString() + "T0" + times.ToString() + "!" +
                          "#001P" + angle1.ToString() + "T0" + times.ToString() + "!" +
                          "#002P" + angle2.ToString() + "T0" + times.ToString() + "!" +
                          "#003P" + angle3.ToString() + "T0" + times.ToString() + "!" +
                          "#004P" + angle4.ToString() + "T0" + times.ToString() + "!" +
                          "#005P" + angle5.ToString() + "T0" + times.ToString() + "!" +
                          "}\r\n", num);

        }
        public void sendmsg(int times, int num, int angle0 = 1500, int angle1 = 1500, int angle2 = 1500, int angle3 = 1500)
        {
            form.SendMsg("{#000P" + angle0.ToString() + "T0" + times.ToString() + "!" +
                          "#001P" + angle1.ToString() + "T0" + times.ToString() + "!" +
                          "#002P" + angle2.ToString() + "T0" + times.ToString() + "!" +
                          "#003P" + angle3.ToString() + "T0" + times.ToString() + "!" +
                          "}\r\n", num);

        }
        public void sendmsg(int times, int num, int angle0 = 1500, int angle1 = 1500, int angle2 = 1500)
        {
            form.SendMsg("{#000P" + angle0.ToString() + "T0" + times.ToString() + "!" +
                          "#001P" + angle1.ToString() + "T0" + times.ToString() + "!" +
                          "#002P" + angle2.ToString() + "T0" + times.ToString() + "!" +                         
                          "}\r\n",num);

        }
        public void sendmsg(SendVar sendVar,int num)//给机械臂蓝牙串口发送控制指令
        {
            string strAngle = "";
            string strtimes = "";
            string msg = "";
            if (sendVar.Times < 100)
                strtimes = "T00" + sendVar.Times.ToString() + "!\r\n";
            if (sendVar.Times >= 100 && sendVar.Times < 1000)
                strtimes = "T0" + sendVar.Times.ToString() + "!\r\n";
            if (sendVar.Times >= 1000)
                strtimes = "T" + sendVar.Times.ToString() + "!\r\n";
            if (sendVar.Angle < 1000)
                strAngle = "P0" + sendVar.Angle.ToString();
            if (sendVar.Angle >= 1000)
                strAngle = "P" + sendVar.Angle.ToString();
            msg = "{#00" + sendVar.Id.ToString() + strAngle + strtimes+ "}";
            form.SendMsg(msg,num);
        }


    }
}
