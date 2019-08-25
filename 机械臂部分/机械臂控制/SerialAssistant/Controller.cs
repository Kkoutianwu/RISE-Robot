using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SerialAssistant
{
    class Controller
    {
        public Controller(Msg msg)
        {
            this.msg = msg;
        }
        private Msg msg;
        public struct redovar  //redo变量
        {
            public int r_angle0;
            public int r_angle2;
        };
        public List<redovar> r_num = new List<redovar>(); //存放redo历史值
        public static bool Ifopen;
        public double[] joint3DOF = new double[3];
        private static double[] lastJoint3DOF = new double[3];
        public double[] joint4DOF_downward = new double[4];
        public double[] joint4DOF_level = new double[4];
        public  double[] lastJoint4DOF = new double[4];
        private static bool on_off;
        private static double[] lastRedoAngle = new double[] { 0, 0, 90, 0 };//手势控制模式上一次角度值

        public void Restoration(int num) //机械臂复位
        {
            msg.sendmsg("$RST!",num);
            Thread.Sleep(100);
            msg.sendmsg("$DST!", num);
            Thread.Sleep(100);
            if(num==1)Array.Clear(lastJoint3DOF, 0, lastJoint3DOF.Length);
            if (num == 2) Array.Clear(lastJoint4DOF, 0, lastJoint4DOF.Length);
        }
        public void Openhand()
        {
            Thread.Sleep(100);
            msg.sendmsg(new SendVar() { Id = 5, Angle = 1700, Times = 500 }, 2);
            Thread.Sleep(600);
            Ifopen = true;
        }
        public void Closehand()
        {
            Thread.Sleep(100);
            msg.sendmsg(new SendVar() { Id = 5, Angle = 1430, Times = 500 },2);
            Thread.Sleep(500);
            Ifopen = false;
        }

        public void Sideturn()
        {
            msg.sendmsg(new SendVar() { Id = 4, Angle = 900, Times = 2500 }, 2);
            Thread.Sleep(3000);
            msg.sendmsg(new SendVar() { Id = 4, Angle = 720, Times = 2500 }, 2);
            Thread.Sleep(2550);


        }

        public void Levelturn()
        {
            msg.sendmsg(new SendVar() { Id = 4, Angle = 1500, Times = 1500 }, 2);
            Thread.Sleep(1600);

        }

        public void SuckUp()
        {
            msg.sendmsg(new SendVar() { Id = 4, Angle = 1000, Times = 200 },1);//电磁阀开
            msg.sendmsg(new SendVar() { Id = 3, Angle = 2000, Times = 1000 }, 1);//吸气
            Thread.Sleep(800);
            msg.sendmsg(new SendVar() { Id = 3, Angle = 1000, Times = 200 }, 1);//关闭吸气

        }
        public void SuckDown()
        {
            msg.sendmsg(new SendVar() { Id = 4, Angle = 2000, Times = 500 }, 1);//电磁阀关
            Thread.Sleep(600);
            msg.sendmsg(new SendVar() { Id = 4, Angle = 1500, Times = 200 }, 1);//电磁阀开
            Thread.Sleep(300);

        }
        /**************0号舵机*****************/
        public void Joint0Run(double angle,int num, ref double lastAngle)
        {
           SendVar sendVar = new SendVar();
            sendVar.Id = 0;
            sendVar.Angle = (int)(angle * 7.8) + 1500;
            sendVar.Times = (int)Math.Abs(angle - lastAngle) * 40;
            msg.sendmsg(sendVar, num);
            lastAngle = angle;
            Thread.Sleep(sendVar.Times);
        }
        public void Joint0Run(double angle, int num, int times)
        {
            SendVar sendVar = new SendVar();
            double temp;
            sendVar.Id = 0;
            temp = angle * 7.8;
            sendVar.Angle = (int)temp + 1500;
            sendVar.Times = times;
            msg.sendmsg(sendVar,num);
            Thread.Sleep(sendVar.Times);
        }
        /**************1号舵机*****************/
        public void Joint1Run(double angle, int num, ref double lastAngle)
        {
            SendVar sendVar = new SendVar();
            sendVar.Id = 1;
            sendVar.Angle = 1500 - (int)(angle * 7.4);
            sendVar.Times = (int)Math.Abs(angle - lastAngle) * 40;
            msg.sendmsg(sendVar, num);
            lastAngle = angle;
            Thread.Sleep(sendVar.Times);
        }
        public void Joint1Run(double angle, int num, int times)
        {
            SendVar sendVar = new SendVar();
            sendVar.Id = 1;
            sendVar.Angle = 1500 - (int)(angle * 7.4);
            sendVar.Times = times;
            msg.sendmsg(sendVar, num);
            Thread.Sleep(sendVar.Times);
        }
        /**************2号舵机*****************/
        public void Joint2Run(double angle, int num, ref double lastAngle)
        {
            SendVar sendVar = new SendVar();
            sendVar.Id = 2;
            sendVar.Angle = (int)(angle * 7.8) + 1500;
            sendVar.Times = (int)Math.Abs(angle - lastAngle) * 50;
            msg.sendmsg(sendVar, num);
            lastAngle = angle;
            Thread.Sleep(sendVar.Times);
        }
        public void Joint2Run(double angle, int num, int times)
        {
            SendVar sendVar = new SendVar();
            sendVar.Id = 2;
            sendVar.Angle = (int)(angle * 7.8) + 1500;
            sendVar.Times = times;
            msg.sendmsg(sendVar, num);
            Thread.Sleep(sendVar.Times);
        }
        /**************3号舵机*****************/
        public void Joint3Run(double angle, int num, ref double lastAngle)
        {
            SendVar sendVar = new SendVar();
            sendVar.Id = 3;
            sendVar.Angle = (int)(angle * 7.8) + 1500;
            sendVar.Times = (int)Math.Abs(angle - lastAngle) * 50;
            msg.sendmsg(sendVar, num);
            lastAngle = angle;
            Thread.Sleep(sendVar.Times);
        }
        public void Joint3Run(double angle, int num, int times)
        {
            SendVar sendVar = new SendVar();
            sendVar.Id = 3;
            sendVar.Angle = (int)(angle * 7.8) + 1500;
            sendVar.Times = times;
            msg.sendmsg(sendVar, num);
            Thread.Sleep(sendVar.Times);
        }

        public void TogetherRun4DOF(double[] joint,int v, ref double[] lastJoint)
        {
            int[] time = new int[4];
            for(int i=0;i<time.Length;i++)
            {
                time[i]= (int)Math.Abs(joint[i] - lastJoint[i]) * v;
                if (time[i] < 500) time[i] = 500;

            }           
            msg.sendmsg((time.Max() > 2500) ? 2500 : time.Max(), 2, angle0: (int)(joint[0] * 7.41) + 1500, angle1: 1500 - (int)(joint[1] * 7.41), angle2: (int)(joint[2] * 7.41) + 1500, angle3:  1500 - (int)(joint[3] * 7.41));//(time.Max()>1500)?1500:time.Max()
            lastJoint = joint;
            Thread.Sleep(((time.Max() > 2500) ? 2500 : time.Max())+20);
        }


        public void TogetherRun4DOF_con(double[] joint, ref double[] lastJoint)
        {
            int[] time = new int[4];
            for (int i = 0; i < time.Length; i++)
            {
                time[i] = (int)Math.Abs(joint[i] - lastJoint[i]) * 15;
                if (time[i] < 500) time[i] = 500;


            }
            msg.sendmsg((time.Max() > 2500) ? 2500 : time.Max(), 2, angle0: (int)(joint[0] * 7.41) + 1500, angle1: 1500 - (int)(joint[1] * 7.41), angle2: (int)(joint[2] * 7.41) + 1500, angle3: 1500 - (int)(joint[3] * 7.41));//(time.Max()>1500)?1500:time.Max()
            lastJoint = joint;
            //Thread.Sleep((time.Max() > 1500) ? 1500 : time.Max());
        }
        public void SortOrderRun4DOF(double[] joint, ref double[] lastJoint)
        {

            Joint0Run(joint[0], 2, ref lastJoint[0]);
            Joint1Run(joint[1], 2, ref lastJoint[1]);
            Joint2Run(joint[2], 2, ref lastJoint[2]);
            Joint3Run(joint[3], 2, ref lastJoint[3]);
            
        

        }

        public void TogetherRun3DOF(double[] joint, ref double[] lastJoint)
        {
            int[] time = new int[3];
            for (int i = 0; i < time.Length; i++)
            {
                time[i] = (int)Math.Abs(joint[i] - lastJoint[i]) * 15;
                if (time[i] < 500) time[i] = 500;

            }

            //msg.sendmsg((time.Max() > 2500) ? 2500 : time.Max(), 1, angle0: (int)((joint[0] * (7.8 - Math.Abs(joint[0]) * 0.006)) + 1485 - Math.Abs(joint[0]) * 0.385), angle1: 1539 - (int)(joint[1] * 7.41), angle2: (int)(joint[2] * 7.41) + 1533, angle3:1000, angle4:1500, angle5:1500);//(time.Max()>1500)?1500:time.Max()   (8- Math.Abs(joint[0])*0.0344827)  1495-Math.Abs(joint[0])*0.45
            if (joint[0] <= 0)
            {
                msg.sendmsg((time.Max() > 2500) ? 2500 : time.Max(), 1, angle0: (int)(joint[0] * 7.41) + 1485, angle1: 1539 - (int)(joint[1] * 7.41), angle2: (int)(joint[2] * 7.41) + 1533, angle3: 1000, angle4: 1500, angle5: 1500);//(time.Max()>1500)?1500:time.Max()   (8- Math.Abs(joint[0])*0.0344827)  1495-Math.Abs(joint[0])*0.45
            }

            else
            {
                msg.sendmsg((time.Max() > 2500) ? 2500 : time.Max(), 1, angle0: (int)(joint[0] * 6.7) + 1496, angle1: 1539 - (int)(joint[1] * 7.41), angle2: (int)(joint[2] * 7.41) + 1533, angle3: 1000, angle4: 1500, angle5: 1500);//(time.Max()>1500)?1500:time.Max()   (8- Math.Abs(joint[0])*0.0344827)  1495-Math.Abs(joint[0])*0.45 
            }

                lastJoint = joint;
            Thread.Sleep(((time.Max() > 2500) ? 2500 : time.Max()) + 20);
        }

        public void SortOrderRun3DOF(double[] joint, ref double[] lastJoint)
        {

            Joint0Run(joint[0], 1, ref lastJoint[0]);
            Joint1Run(joint[1], 1, ref lastJoint[1]);
            Joint2Run(joint[2], 1, ref lastJoint[2]);

        }

        public bool ArriveSetoff4DOF_downward(double x,double y,double z,int mod)  //模式0为顺序执行，模式1为一起执行
        {
            bool ifsol = InverseKinematics.IK4DOF_downward(x, y, z, out joint4DOF_downward);
            if (ifsol)
            {
                if(mod==0) TogetherRun4DOF(joint4DOF_downward, 50, ref lastJoint4DOF);
                else TogetherRun4DOF(joint4DOF_downward,16, ref lastJoint4DOF);
            }
            else
            {
                lastJoint4DOF = joint4DOF_downward;
            }
            return ifsol;
        }

        public bool ArriveSetoff4DOF_level(double x, double y, double z, int mod)  //模式0为顺序执行，模式1为一起执行
        {
            bool ifsol = InverseKinematics.IK4DOF_level(x, y, z, out joint4DOF_level);
            if (ifsol)
            {
                if (mod == 0) TogetherRun4DOF(joint4DOF_level, 50, ref lastJoint4DOF);
                else TogetherRun4DOF(joint4DOF_level,16, ref lastJoint4DOF);
            }
            else
            {
                lastJoint4DOF = joint4DOF_level;
            }
            return ifsol;
        }

        public bool ArriveSetoff4DOF_level_con(double x, double y, double z, int mod)  //模式0为顺序执行，模式1为一起执行
        {
            bool ifsol = InverseKinematics.IK4DOF_level(x, y, z, out joint4DOF_level);
            joint4DOF_level[1] -= 10;
            joint4DOF_level[2] -= 10;
            joint4DOF_level[3] -= 1;
            if (ifsol)
            {
                if (mod == 0) SortOrderRun4DOF(joint4DOF_level, ref lastJoint4DOF);
                else TogetherRun4DOF_con(joint4DOF_level, ref lastJoint4DOF);
            }
            else
            {
                lastJoint4DOF = joint4DOF_level;
            }
            return ifsol;
        }
        public bool ArriveSetoff3DOF(double x, double y, double z, int mod)  //模式0为顺序执行，模式1为一起执行
        {
            bool ifsol = InverseKinematics.IK3DOF(x, y, z, out joint3DOF);
            if (ifsol)
            {
                if (mod == 0) SortOrderRun3DOF(joint3DOF, ref lastJoint3DOF);
                else TogetherRun3DOF(joint3DOF, ref lastJoint3DOF);
            }
            else
            {
                lastJoint3DOF = joint3DOF;
            }
            return ifsol;

        }
        public bool PlayChess(double row, double column)
        {
            double x = 0, y = 0, z = -7;
            double ex = 9.2;//x方向偏移
            double ey = 12.4;//y方向偏移   
            double a = 3.1;//棋盘边长
            x = ex + column * a;
            y = ey - row * a;
            bool ifsol = ArriveSetoff3DOF(x, y, z, 1);
            return ifsol;

        }

    }
}
