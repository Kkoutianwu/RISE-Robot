using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Diagnostics;
using System.Net;
using MatrixTool;

namespace SerialAssistant
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            controller = new Controller(new Msg(this));
            InitializeComponent();
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;//设置该属性 为false
            FormClosing += new FormClosingEventHandler(this.FrmMain_FormClosing);
        }
        private FileOperations fileOperations;
        private Controller controller;
        private StringBuilder sb = new StringBuilder();    //为了避免在接收处理函数中反复调用，依然声明为一个全局变量
        private bool con = true;
        private bool if4DOF=false;
        private bool if3DOF = false; 
        int AutoGrab_counter = 0, AutoGrab_tag3 = 0, AutoGrab_tag4 = 0 ;
        #region 窗口事件
        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)//关闭窗口事件
        {
            serialPort1.Close();
            serialPort2.Close();
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)//加载窗口事件
        {

            //获取电脑当前可用串口并添加到选项列表中
            comboBox1.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
            comboBox2.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
            //设置选项默认值
           if(Application.StartupPath == @"C:\Users\myoasm\Desktop\整合\机械臂控制\SerialAssistant\bin\Debug") textBoxFolderBrowserDialog.Text = @"C:\Users\myoasm\Desktop\zuobiao";
            else textBoxFolderBrowserDialog.Text = Application.StartupPath;  //获取当前路径 
            fileOperations = new FileOperations(textBoxFolderBrowserDialog.Text);
            
            Stamp_button.Enabled = true;

        }
        #endregion

        #region 串口事件
        public void SendMsg(string str, int num)  //串口发送msg
        {
            if(con)Thread.Sleep(50);
            if (num == 1) serialPort1.Write(str);//3DOF
            if (num == 2) serialPort2.Write(str);//4DOF
            if (con) Thread.Sleep(50);
        }

        private void PortOpen_button_Click(object sender, EventArgs e) //连接小手按钮
        {
            try
            {
                //将可能产生异常的代码放置在try块中
                //根据当前串口属性来判断是否打开
                if (serialPort1.IsOpen)
                {
                    controller.Restoration(1);

                    //串口已经处于打开状态
                    serialPort1.Close();    //关闭串口
                    button_3DOF.Text = "连接机械臂";
                    button_3DOF.BackColor = Color.ForestGreen;
                    comboBox1.Enabled = true;
                    label6.Text = "未连接机械臂";
                    label6.ForeColor = Color.Red;
                    button2.Enabled = false;        //失能发送按钮                   
                }
                else
                {
                    //串口已经处于关闭状态，则设置好串口属性后打开
                    comboBox1.Enabled = false;
                    serialPort1.PortName = comboBox1.Text;
                    serialPort1.BaudRate = 9600;
                    serialPort1.Open();     //打开串口
                    button_3DOF.Text = "断开机械臂连接";
                    button_3DOF.BackColor = Color.Firebrick;
                    label6.Text = "机械臂已连接";
                    label6.ForeColor = Color.Green;
                    button2.Enabled = true;        //使能发送按钮
                    PourWater_button.Enabled = true;
                    PlayChess_button.Enabled = true;
                    button4.Enabled = true;
                    button5.Enabled = true;
                    controller.Restoration(1);
                    Thread.Sleep(2000);
                    fixed_point_text.Text = "0,22,-7";
                    fileOperations.WriteTxt("row_column", "0,0,0");
                    fileOperations.WriteTxt("setoff", "0,0,0,0");
                    fileOperations.WriteTxt("cupsetoff", "0,0,0,0");
                    controller.ArriveSetoff3DOF(0, -19, 12, 1);//手臂躲到旁边
                }
            }
            catch (Exception ex)
            {
                //捕获可能发生的异常并进行处理

                //捕获到异常，创建一个新的对象，之前的不可以再用
                serialPort1 = new System.IO.Ports.SerialPort();
                //刷新COM口选项
                comboBox1.Items.Clear();
                comboBox1.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
                //响铃并显示异常给用户
                System.Media.SystemSounds.Beep.Play();
                button_3DOF.Text = "连接机械臂";
                button_3DOF.BackColor = Color.ForestGreen;
                MessageBox.Show(ex.Message);
                comboBox1.Enabled = true;
            }
        }


        private void button_4DOF_Click(object sender, EventArgs e)//连接大手按钮
        {
            try
            {
                //将可能产生异常的代码放置在try块中
                //根据当前串口属性来判断是否打开
                if (serialPort2.IsOpen)
                {
                    controller.Restoration(2);

                    //串口已经处于打开状态
                    serialPort2.Close();    //关闭串口
                    button_4DOF.Text = "连接机械臂";
                    button_4DOF.BackColor = Color.ForestGreen;
                    comboBox2.Enabled = true;
                    label6.Text = "未连接机械臂";
                    label6.ForeColor = Color.Red;
                    button2.Enabled = false;        //失能发送按钮                   
                }
                else
                {
                    //串口已经处于关闭状态，则设置好串口属性后打开
                    comboBox2.Enabled = false;
                    serialPort2.PortName = comboBox2.Text;
                    serialPort2.BaudRate = 9600;
                    serialPort2.Open();     //打开串口
                    button_4DOF.Text = "断开机械臂连接";
                    button_4DOF.BackColor = Color.Firebrick;
                    label6.Text = "机械臂已连接";
                    label6.ForeColor = Color.Green;
                    button2.Enabled = true;        //使能发送按钮
                    PourWater_button.Enabled = true;
                    PlayChess_button.Enabled = true;
                    button4.Enabled = true;
                    button5.Enabled = true;
                    controller.Restoration(2);
                    Thread.Sleep(100);
                    controller.Openhand();
                    fileOperations.WriteTxt("setoff", "0,0,0,0");
                }
            }
            catch (Exception ex)
            {
                //捕获可能发生的异常并进行处理

                //捕获到异常，创建一个新的对象，之前的不可以再用
                serialPort2 = new System.IO.Ports.SerialPort();
                //刷新COM口选项
                comboBox2.Items.Clear();
                comboBox2.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
                //响铃并显示异常给用户
                System.Media.SystemSounds.Beep.Play();
                button_4DOF.Text = "连接机械臂";
                button_4DOF.BackColor = Color.ForestGreen;
                MessageBox.Show(ex.Message);
                comboBox2.Enabled = true;
            }

        }

        #endregion


        #region Click事件
        private void ChooseFile_button_Click(object sender, EventArgs e)  //选择路径
        {
            FolderBrowserDialog dilog = new FolderBrowserDialog();
            dilog.Description = "请选择文件夹";
            if (dilog.ShowDialog() == DialogResult.OK || dilog.ShowDialog() == DialogResult.Yes)
            {
                textBoxFolderBrowserDialog.Text = dilog.SelectedPath;
            }
            fileOperations = new FileOperations(textBoxFolderBrowserDialog.Text);//更新路径

        }

        private void AutoGrab_button_Click(object sender, EventArgs e)  //抓取
        {
            Thread thread = new Thread(AutoGrab);
            thread.Start();
        }
              
        private void PourWater_button_Click(object sender, EventArgs e)   //倒水
        {
            Thread thread = new Thread(PourWater);
            thread.Start();
        }

        private void PlayChess_button_Click(object sender, EventArgs e)  //下棋
        {
            Thread thread3DOF = new Thread(PlayChess);
            thread3DOF.Start();

        }

        private void Stamp_button_Click(object sender, EventArgs e)  //盖章 
        {
            
            controller.SuckUp();
            controller.SuckDown();


        }

        private void button4_Click(object sender, EventArgs e)  //调试用
        {
            button4.Text = "OK";
            button4.BackColor = Color.Firebrick;
            Thread thread = new Thread(OnOff);
            thread.IsBackground = true;
            thread.Start();
        }

        private void button5_Click(object sender, EventArgs e)  //调试用
        {
            //SayHello();
            // FistGrip();
        }


        #endregion

        #region 其他函数

        private void DisplayJoint(double[] joint)//显示关节角度
        {
            sb.Clear();
            sb.Append(joint[0] + "\r\n");
            sb.Append(joint[1] + "\r\n");
            sb.Append(joint[2] + "\r\n");
            sb.Append(joint[3] + "\r\n");
            try
            {
                //因为要访问UI资源，所以需要使用invoke方式同步ui
                Invoke((EventHandler)(delegate { textBox_receive.AppendText(sb.ToString() + "\r\n"); }));
            }
            catch (Exception ex)
            {
                //响铃并显示异常给用户
                System.Media.SystemSounds.Beep.Play();
                MessageBox.Show(ex.Message);

            }
        }



        private void AutoGrab4DOF(object obj)
        {
            if4DOF = true;
            double goalx = -12;
            double goaly = 20;
            double goalz = 10;  //目标  
            string str = obj as string;
            fileOperations.GetSetoff(str, out double x, out double y, out int flag, out int id);

            InverseKinematics.Transformation(new double[3] { x, y, -4 }, -90, 12.4, 38, out double[] newsetoff);
            try
            {
                if (serialPort2.IsOpen)
                {
                
                    if (newsetoff[1] >= 0) newsetoff[1] = newsetoff[1] + 2;
                    if (newsetoff[1] < 0) newsetoff[1] = newsetoff[1] - 1.4;

                    bool ifsol = controller.ArriveSetoff4DOF_downward(newsetoff[0], newsetoff[1], 0, 1);
                    if (!ifsol)
                    {
                        label6.Text = "无解";
                    }
                    else
                    {
                        label6.Text = "正常运行";
                        DisplayJoint(controller.joint4DOF_downward);
                        controller.Closehand();
                        bool ifsol1 = controller.ArriveSetoff4DOF_downward(newsetoff[0], newsetoff[1], 20, 1);
                        if (!ifsol1) controller.ArriveSetoff4DOF_downward(0, 0, 37, 1);
                        controller.ArriveSetoff4DOF_downward(10, 17, 20, 1);
                        fileOperations.WriteTxt("flag", "1");
                        con = false;
                        controller.ArriveSetoff4DOF_downward(goalx, goaly, 20, 1);
                        con = true;
                        controller.ArriveSetoff4DOF_downward(goalx, goaly, goalz, 1);
                        controller.Openhand();
                        controller.ArriveSetoff4DOF_downward(goalx, goaly, 20, 1);
                        controller.ArriveSetoff4DOF_downward(0, 0, 37, 1);
                    }

                    AutoGrab_tag4 = 1;
                    if4DOF = false;
                }
            }
            catch (Exception ex)
            {
                serialPort2.Close();
                //捕获到异常，创建一个新的对象，之前的不可以再用
                serialPort2 = new System.IO.Ports.SerialPort();
                //刷新COM口选项
                comboBox2.Items.Clear();
                comboBox2.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
                //响铃并显示异常给用户
                System.Media.SystemSounds.Beep.Play();
                button_4DOF.Text = "连接小手";
                button_4DOF.BackColor = Color.ForestGreen;
                MessageBox.Show(ex.Message);
                comboBox2.Enabled = true;
                if4DOF = false;
            }

        }

        private void AutoGrab3DOF(object obj)
        {
            if3DOF = true;
            string str = obj as string;
            fileOperations.GetSetoff(str, out double x, out double y, out int flag, out int id);

            InverseKinematics.Transformation(new double[3] { x, y, -8.5 }, 90, 12.4, -9, out double[] newsetoff);
            try
            {
                if (serialPort1.IsOpen)
                {
                    controller.ArriveSetoff3DOF(newsetoff[0], newsetoff[1], 4, 1);//吸住前过中间点
                    bool ifsol = controller.ArriveSetoff3DOF(newsetoff[0], newsetoff[1], newsetoff[2], 1);
                    if (ifsol)
                    {
                        controller.SuckUp();
                        controller.ArriveSetoff3DOF(10, -10, 10, 1);//吸住后过中间点
                        
                        controller.ArriveSetoff3DOF(0, -15, -3, 1);//吸住后放置到目标点
                        fileOperations.WriteTxt("flag", "1");
                        Thread.Sleep(200);
                        controller.SuckDown();
                    }
                    AutoGrab_tag3 = 1;
                    if3DOF = false;
                }
            }
            catch (Exception ex)
            {
                serialPort1.Close();
                //捕获到异常，创建一个新的对象，之前的不可以再用
                serialPort1 = new System.IO.Ports.SerialPort();
                //刷新COM口选项
                comboBox1.Items.Clear();
                comboBox1.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
                //响铃并显示异常给用户
                System.Media.SystemSounds.Beep.Play();
                button_3DOF.Text = "连接小手";
                button_3DOF.BackColor = Color.ForestGreen;
                MessageBox.Show(ex.Message);
                comboBox1.Enabled = true;
                if3DOF = false;
            }

        }

        private void AutoGrab() //抓取
        {
            string temp;
            int run4 = 0, run3 = 0;
            AutoGrab_tag3 = 0;
            AutoGrab_tag4 = 0;
            bool AutoGrab_con = false;
            controller.Openhand();
            controller.ArriveSetoff3DOF(0, -19, 12, 1);//手臂躲到旁边
            while (serialPort2.IsOpen || serialPort1.IsOpen)    
            {
                fileOperations.GetSetoff(fileOperations.ReadTxt("mod"), out int mod); if (mod == 0) { Application.ExitThread(); break; }
                textBox_send.Text = fileOperations.ReadTxt("setoff");
                fileOperations.GetSetoff(textBox_send.Text, out double x, out double y, out int flag,out int id);
                if (flag == 1)
                {

                                
                    if (id==2)  //大手抓取
                    {
                        if(!if4DOF)
                        {
                            run4 = 1;
                            fileOperations.WriteTxt("setoff", "0,0,0,0");
                            AutoGrab_tag4 = 0;
                            AutoGrab_counter = 0;
                            Thread thread4DOF = new Thread(AutoGrab4DOF);
                            thread4DOF.Start(textBox_send.Text);
                            if4DOF = true;
                        }
                    }

                    else if (id == 1)  //小手吸
                    {
                        if (!if3DOF)
                        {
                            run3 = 1;
                            fileOperations.WriteTxt("setoff", "0,0,0,0");
                            AutoGrab_tag3 = 0;
                            AutoGrab_counter = 0;
                            Thread thread3DOF = new Thread(AutoGrab3DOF);
                            thread3DOF.Start(textBox_send.Text);
                            if3DOF = true;
                        }

                    }
                }
                if (run3 == 1 && run4 == 1)
                {
                    AutoGrab_con = true;
                }
                if (AutoGrab_con)
                {
                    if (AutoGrab_tag3 == 1 && AutoGrab_tag4 == 1)
                    {
                        AutoGrab_counter++;
                        Thread.Sleep(50);
                    }

                    if (AutoGrab_counter > 100)
                    {
                        AutoGrab_tag3 = 0;
                        AutoGrab_tag4 = 0;
                        AutoGrab_counter = 0;
                        controller.ArriveSetoff3DOF(0, -19, 12, 1);//手臂躲到旁边
                        controller.ArriveSetoff4DOF_downward(0, 0, 37, 1);
                        fileOperations.WriteTxt("ifend", "1");

                    }

                }
                else
                {
                    if (AutoGrab_tag3 == 1 || AutoGrab_tag4 == 1)
                    {
                        AutoGrab_counter++;
                        Thread.Sleep(50);
                    }

                    if (AutoGrab_counter > 100)
                    {
                        AutoGrab_tag3 = 0;
                        AutoGrab_tag4 = 0;
                        AutoGrab_counter = 0;
                        controller.ArriveSetoff3DOF(0, -19, 12, 1);//手臂躲到旁边
                        controller.ArriveSetoff4DOF_downward(0, 0, 37, 1);
                        fileOperations.WriteTxt("ifend", "1");

                    }

                }



            }
        }

        private void PlayChess()  //下棋
        {
            int flag = 0,mod;
            double row=0, column=0;
            string mod_str;
            fileOperations.GetSetoff(fixed_point_text.Text, out double x, out double y, out double z);
            controller.ArriveSetoff3DOF(0, 17, 12, 1);//手臂躲到旁边
            while (serialPort1.IsOpen)
            {

                fileOperations.GetSetoff(fileOperations.ReadTxt("mod"), out  mod); if (mod == 0) { Application.ExitThread(); break; }
                
                Thread.Sleep(100);
                controller.ArriveSetoff3DOF(x, y, z, 1);//棋子正上方等待手臂稳定
                
                flag = 0;
                while (flag==0)
                {
                    fileOperations.GetSetoff(fileOperations.ReadTxt("mod"), out mod); if (mod == 0) { Application.ExitThread(); break; }
                    textBox_chess.Text = fileOperations.ReadTxt("row_column");
                    fileOperations.GetSetoff(textBox_chess.Text, out row, out column, out flag);
                }
               
                if (flag==1)
                {
                    fileOperations.WriteTxt("row_column", "0,0,0");
                    try
                    {
                        controller.ArriveSetoff3DOF(x, y, z, 1);//棋子正上方等待手臂稳定
                        controller.ArriveSetoff3DOF(x, y, z - 1.5, 1);//到定点抓棋                                                                      
                        controller.SuckUp();
                        controller.ArriveSetoff3DOF(x, y, 2, 1);//到定点抓棋
                        controller.ArriveSetoff3DOF(13, 9, 0, 1);//路径规划中间点
                        bool ifsol1 = controller.PlayChess(row, column);
                        if (!ifsol1)
                        {
                            label6.Text = "够不着，帮我下";
                            controller.ArriveSetoff3DOF(20, -10, 15, 1);
                            Thread.Sleep(2500);
                        }
                        Thread.Sleep(200);
                        controller.SuckDown();
                        controller.ArriveSetoff3DOF(13, 9, 0, 1);//路径规划中间点
                        controller.ArriveSetoff3DOF(0, 17, 12, 1);
                        fileOperations.WriteTxt("tag", "1");
                        fileOperations.WriteTxt("ifend", "1");
                        //controller.Restoration();


                    }
                    catch (Exception ex)
                    {
                        serialPort1.Close();
                        //捕获到异常，创建一个新的对象，之前的不可以再用
                        serialPort1 = new System.IO.Ports.SerialPort();
                        //刷新COM口选项
                        comboBox1.Items.Clear();
                        comboBox1.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
                        //响铃并显示异常给用户
                        System.Media.SystemSounds.Beep.Play();
                        button_3DOF.Text = "连接小手";
                        button_3DOF.BackColor = Color.ForestGreen;
                        MessageBox.Show(ex.Message);
                        comboBox1.Enabled = true;
                    }
                }

            }

        }

        private void Stamp()  //盖章
        {


        }

        private void RemoteControl()  //遥控
        {
            
            double[] goalsetoff = new double[3] { 0,0,0};
            double goalx = 0, goaly = 0;
            int flag1 = 0;
            int counter = 0, tag = 0;
            while (flag1==0)
            {
                fileOperations.GetSetoff(fileOperations.ReadTxt("mod"), out int mod); if (mod == 0) { Application.ExitThread(); break; }
                textBox_send.Text = fileOperations.ReadTxt("cupsetoff");
                fileOperations.GetSetoff(textBox_send.Text, out goalx, out goaly, out flag1, out int id);
                Thread.Sleep(100);

            }
            fileOperations.WriteTxt("cupsetoff", "0,0,0,0");
            InverseKinematics.Transformation(new double[3] { goalx, goaly, -1 }, 90, 12.4, -9, out goalsetoff);
            while (serialPort2.IsOpen || serialPort1.IsOpen)
            {
                fileOperations.GetSetoff(fileOperations.ReadTxt("mod"), out int mod); if (mod == 0) { Application.ExitThread(); break; }
                textBox_send.Text = fileOperations.ReadTxt("setoff");
                fileOperations.GetSetoff(textBox_send.Text, out double x, out double y, out int flag, out int id);
                if (flag == 1)
                {
                    counter = 0;
                    fileOperations.WriteTxt("setoff", "0,0,0,0");
                    InverseKinematics.Transformation(new double[3] { x, y, -7.8 }, 90, 12.4, -9, out double[] newsetoff);
                    try
                    {
                        if (serialPort1.IsOpen)
                        {
                            controller.ArriveSetoff3DOF(newsetoff[0], newsetoff[1], -1, 1);//吸前过中间点
                            bool ifsol = controller.ArriveSetoff3DOF(newsetoff[0], newsetoff[1], newsetoff[2], 1);
                            if (ifsol)
                            {
                                controller.SuckUp();
                                controller.ArriveSetoff3DOF(newsetoff[0], newsetoff[1], -1,1);//吸前过中间点                      
                                controller.ArriveSetoff3DOF(goalsetoff[0], goalsetoff[1], goalsetoff[2], 1);//吸住后放置到目标点
                                Thread.Sleep(500);
                                controller.SuckDown();
                                controller.ArriveSetoff3DOF(13, 0, -1, 1);//吸住后过中间点
                            }
                            fileOperations.WriteTxt("flag", "1");
                            tag = 1;
                        }
                    }
                    catch (Exception ex)
                    {
                        serialPort1.Close();
                        //捕获到异常，创建一个新的对象，之前的不可以再用
                        serialPort1 = new System.IO.Ports.SerialPort();
                        //刷新COM口选项
                        comboBox1.Items.Clear();
                        comboBox1.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
                        //响铃并显示异常给用户
                        System.Media.SystemSounds.Beep.Play();
                        button_3DOF.Text = "连接小手";
                        button_3DOF.BackColor = Color.ForestGreen;
                        MessageBox.Show(ex.Message);
                        comboBox1.Enabled = true;
                    }                    
                }
                if (tag == 1)
                { 
                    counter++;
                    Thread.Sleep(50);
                }
                if (counter > 30)
                {
                    tag = 0;
                    counter = 0;
                    fileOperations.WriteTxt("ifend", "1");
                }
            }

        } 

        private void PourWater()  //倒水
        {

            double x = 0, y = 0;
            int mod=3;
            int flag = 0;
            double[] newsetoff1 = new double[3] { 0, 0, 0 }; 
            while (flag == 0)
            {
                string mod_str = fileOperations.ReadTxt("mod");
                fileOperations.GetSetoff(mod_str, out mod);
                if (mod == 0) { Application.ExitThread();break; }
                textBox_chess.Text = fileOperations.ReadTxt("setoff");
                fileOperations.GetSetoff(textBox_chess.Text, out x, out y, out flag,out int id);
            }

            if (flag == 1)
            {
                controller.Openhand();
                PourWaterInit();
                fileOperations.WriteTxt("setoff", "0,0,0,0");
                InverseKinematics.Transformation(new double[3] { x, y, 0 }, -90, 12.4, 38, out double[] newsetoff);
                try
                {
                    bool ifsol = controller.ArriveSetoff4DOF_level(newsetoff[0], newsetoff[1], 0, 1);

                    controller.Closehand();
                    if (ifsol)
                    {
                        PourWaterMid();

                        //controller.ArriveSetoff4DOF_level(26.5, 0, 10.5, 1);
                        fileOperations.WriteTxt("flagg", "1");
                        int flag1 = 0;
                        double x1 = 0;
                        double y1 = 0;

                        while (flag1 == 0)
                        {
                            textBox_chess.Text = fileOperations.ReadTxt("setoff");
                            fileOperations.GetSetoff(textBox_chess.Text, out x1, out y1, out flag1,out int id);
                            string mod_str = fileOperations.ReadTxt("mod");
                            fileOperations.GetSetoff(mod_str, out mod);
                            if (mod == 0) { Application.ExitThread(); break; }
                        }
                        if (mod != 0)
                        {
                            fileOperations.WriteTxt("setoff", "0,0,0,0");
                            InverseKinematics.Transformation(new double[3] { x1 , y1, 0 }, -90, 12.4, 38, out newsetoff1);
                            bool ifsol2 = controller.ArriveSetoff4DOF_level(newsetoff1[0]-2.5, newsetoff1[1] - 14, 9, 1);
                            if (ifsol2)
                            {
                                controller.Sideturn();
                                Thread.Sleep(1000);
                                controller.Levelturn();

                                // controller.ArriveSetoff4DOF_level(23, 0, 0, 1);
                                

                            }
                            controller.ArriveSetoff4DOF_level(newsetoff[0], newsetoff[1], 0, 1);
                        }

                    }
                    if (mod != 0)
                    {
                        controller.Openhand();
                        PourWaterInit();
                        PourWaterEndM();
                        controller.Openhand();
                        bool ifsol2 = controller.ArriveSetoff4DOF_level(newsetoff1[0], newsetoff1[1], 0, 0);
                        controller.Closehand();
                        controller.ArriveSetoff4DOF_level(newsetoff1[0], newsetoff1[1], 3,0);
                        controller.ArriveSetoff4DOF_level(newsetoff1[0], newsetoff1[1], 6, 0);
                   
                        controller.ArriveSetoff4DOF_level(10, 23, 4, 0);
                        controller.ArriveSetoff4DOF_level(10, 23, 0, 0);
                        controller.Openhand();
                        PourWaterEndend();
                        controller.ArriveSetoff4DOF_downward(0, 0, 37, 1);
                        controller.Closehand();
                    }
                    fileOperations.WriteTxt("ifend", "1");
                }
                catch (Exception ex)
                {
                    serialPort2.Close();
                    //捕获到异常，创建一个新的对象，之前的不可以再用
                    serialPort2 = new System.IO.Ports.SerialPort();
                    //刷新COM口选项
                    comboBox2.Items.Clear();
                    comboBox2.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
                    //响铃并显示异常给用户
                    System.Media.SystemSounds.Beep.Play();
                    button_4DOF.Text = "连接大手";
                    button_4DOF.BackColor = Color.ForestGreen;
                    MessageBox.Show(ex.Message);
                    comboBox2.Enabled = true;
                }
            }
        }

        private void PourWaterInit()
        {
  
            SendMsg("{#000P1500T1500!#001P2100T1500!#002P2500T1500!#003P1100T1500!}\r\n", 2);
            controller.lastJoint4DOF[0] = 0;
            controller.lastJoint4DOF[1] = -81;
            controller.lastJoint4DOF[2] = 135;
            controller.lastJoint4DOF[3] = 54;
            Thread.Sleep(1500);

        }

        private void PourWaterMid()
        {

            SendMsg("{#000P1500T1500!#001P1500T1500!#002P1900T1500!#003P1250T1500!}\r\n", 2);
            controller.lastJoint4DOF[0] = 0;
            controller.lastJoint4DOF[1] = 0;
            controller.lastJoint4DOF[2] = 54;
            controller.lastJoint4DOF[3] = 34;
            Thread.Sleep(1500);

        }

        private void PourWaterEnd()
        {

            SendMsg("{#000P1200T1500!#001P2100T1500!#002P2500T1500!#003P1100T1500!}\r\n", 2);
            controller.lastJoint4DOF[0] = -40;
            controller.lastJoint4DOF[1] = -81;
            controller.lastJoint4DOF[2] = 135;
            controller.lastJoint4DOF[3] = 54;
            Thread.Sleep(1500);

        }

        private void PourWaterEndM()
        {

            SendMsg("{#000P1720T1500!#001P2100T1500!#002P2500T1500!#003P1100T1500!}\r\n", 2);
            controller.lastJoint4DOF[0] = 30;
            controller.lastJoint4DOF[1] = -81;
            controller.lastJoint4DOF[2] = 135;
            controller.lastJoint4DOF[3] = 54;
            Thread.Sleep(1500);

        }

        private void PourWaterEndend()
        {

            SendMsg("{#000P1944T1500!#001P2100T1500!#002P2500T1500!#003P1100T1500!}\r\n", 2);
            controller.lastJoint4DOF[0] = 60;
            controller.lastJoint4DOF[1] = -81;
            controller.lastJoint4DOF[2] = 135;
            controller.lastJoint4DOF[3] = 54;
            Thread.Sleep(1500);

        }

        private void NULL()
        { }

        private void RES()
        {
            if (serialPort1.IsOpen)
            {
                Thread.Sleep(500);
                controller.ArriveSetoff3DOF(0, -19, 12, 1);//手臂躲到旁边
            }
            if (serialPort2.IsOpen) controller.Restoration(2);

        }

        private void FistGrip()  //抱拳
        {
            SendMsg("{#000P1500T1000!#001P1400T1000!#002P1450T1000!}\r\n", 1);
            SendMsg("{#002P1870T1000!#003P1680T1000!}\r\n", 2);
            Thread.Sleep(2500);
            SendMsg("{#002P1500T1000!#003P1500T1000!}\r\n", 2);
            SendMsg("{#000P0830T1000!#001P1400T0500!#002P1400T1000!}\r\n", 1);
            Thread.Sleep(300);
        }

        private void SayHello()
        {
            SendMsg("{#004P2150T0300!}\r\n", 2);
            Thread.Sleep(310);
            SendMsg("{#002P1580T0150!#003P1350T0150!}\r\n", 2);
            Thread.Sleep(160);
            for(int i=0;i<1;i++)
            { 
                SendMsg("{#002P1460T0300!#003P1600T0300!}\r\n", 2);
                Thread.Sleep(310);
                SendMsg("{#002P1580T0300!#003P1350T0300!}\r\n", 2);
                Thread.Sleep(310);
            }
            SendMsg("{#002P1460T0300!#003P1600T0300!}\r\n", 2);
            Thread.Sleep(310);
            SendMsg("{#002P1500T0500!#003P1500T0500!#004P1500T0500!}\r\n", 2);
            Thread.Sleep(300);
        }

        private void Circle()//画圈
        {
            con = false;
            InverseKinematics.Transformation(new double[3] { 12.4, 15, -6 }, -90, 12.4, 38, out double[] newsetoff);
            controller.ArriveSetoff4DOF_level_con(newsetoff[0], newsetoff[1], newsetoff[2], 1);
            Thread.Sleep(1500);
            InverseKinematics.Transformation(new double[3] { 12.4, 15, -1.5 }, 90, 12.4, -9, out newsetoff);
            controller.ArriveSetoff3DOF(newsetoff[0], newsetoff[1], newsetoff[2], 1);


            InverseKinematics.Transformation(new double[3] { 16.8, 13.7, -6 }, -90, 12.4, 38, out newsetoff);
            controller.ArriveSetoff4DOF_level_con(newsetoff[0], newsetoff[1], newsetoff[2], 1);
            InverseKinematics.Transformation(new double[3] { 16.8, 13.7, -1.5 }, 90, 12.4, -9, out newsetoff);
            controller.ArriveSetoff3DOF(newsetoff[0], newsetoff[1], newsetoff[2], 1);
            //Thread.Sleep(1000);

            InverseKinematics.Transformation(new double[3] { 18.6, 9.3, -6 }, -90, 12.4, 38, out newsetoff);
            controller.ArriveSetoff4DOF_level_con(newsetoff[0], newsetoff[1], newsetoff[2], 1);
            InverseKinematics.Transformation(new double[3] { 18.6, 9.3, -1.5 }, 90, 12.4, -9, out newsetoff);
            controller.ArriveSetoff3DOF(newsetoff[0], newsetoff[1], newsetoff[2], 1);
            //Thread.Sleep(1000);

            InverseKinematics.Transformation(new double[3] { 16.8, 4.9, -6 }, -90, 12.4, 38, out newsetoff);
            controller.ArriveSetoff4DOF_level_con(newsetoff[0], newsetoff[1], newsetoff[2], 1);
            InverseKinematics.Transformation(new double[3] { 16.8, 4.9, -1.5 }, 90, 12.4, -9, out newsetoff);
            controller.ArriveSetoff3DOF(newsetoff[0], newsetoff[1], newsetoff[2], 1);
            //Thread.Sleep(1000);

            InverseKinematics.Transformation(new double[3] { 12.4, 3, -6 }, -90, 12.4, 38, out newsetoff);
            controller.ArriveSetoff4DOF_level_con(newsetoff[0], newsetoff[1], newsetoff[2], 1);
            InverseKinematics.Transformation(new double[3] { 12.4, 3, -1.5 }, 90, 12.4, -9, out newsetoff);
            controller.ArriveSetoff3DOF(newsetoff[0], newsetoff[1], newsetoff[2], 1);
            //Thread.Sleep(1000);

            InverseKinematics.Transformation(new double[3] { 8, 4.9, -6 }, -90, 12.4, 38, out newsetoff);
            controller.ArriveSetoff4DOF_level_con(newsetoff[0], newsetoff[1], newsetoff[2], 1);
            InverseKinematics.Transformation(new double[3] { 8, 4.9, -1.5 }, 90, 12.4, -9, out newsetoff);
            controller.ArriveSetoff3DOF(newsetoff[0], newsetoff[1], newsetoff[2], 1);
            //Thread.Sleep(1000);

            InverseKinematics.Transformation(new double[3] { 6.2, 9.3, -6 }, -90, 12.4, 38, out newsetoff);
            controller.ArriveSetoff4DOF_level_con(newsetoff[0], newsetoff[1], newsetoff[2], 1);
            InverseKinematics.Transformation(new double[3] { 6.2, 9.3, -1.5 }, 90, 12.4, -9, out newsetoff);
            controller.ArriveSetoff3DOF(newsetoff[0], newsetoff[1], newsetoff[2], 1);
            //Thread.Sleep(1000);

            InverseKinematics.Transformation(new double[3] { 8, 13.7, -6 }, -90, 12.4, 38, out newsetoff);
            controller.ArriveSetoff4DOF_level_con(newsetoff[0], newsetoff[1], newsetoff[2], 1);
            InverseKinematics.Transformation(new double[3] { 8, 13.7, -1.5 }, 90, 12.4, -9, out newsetoff);
            controller.ArriveSetoff3DOF(newsetoff[0], newsetoff[1], newsetoff[2], 1);
            //Thread.Sleep(1000);

            InverseKinematics.Transformation(new double[3] { 12.4, 15, -6 }, -90, 12.4, 38, out newsetoff);
            controller.ArriveSetoff4DOF_level_con(newsetoff[0], newsetoff[1], newsetoff[2], 1);
            InverseKinematics.Transformation(new double[3] { 12.4, 15, -1.5 }, 90, 12.4, -9, out newsetoff);
            controller.ArriveSetoff3DOF(newsetoff[0], newsetoff[1], newsetoff[2], 1);
            //Thread.Sleep(1000);

            con = true;

            controller.ArriveSetoff3DOF(0, -19, 12, 1);//手臂躲到旁边
            controller.ArriveSetoff4DOF_downward(0, 0, 37, 1);


        }
      
        private void OnOff()
        {
            bool doing = false;
            Thread thread=new Thread(NULL);
            thread.IsBackground = true;
            string mod_str;
            while (true)
            {
                mod_str = fileOperations.ReadTxt("mod");
                fileOperations.GetSetoff(mod_str, out int mod);
                Thread.Sleep(100);
                if (mod != 0  & !doing)
                {
                    if (mod == 1)
                    {
                        thread = new Thread(RemoteControl);
                        thread.IsBackground = true;
                        thread.Start();                      
                    }
                    else if (mod == 2)
                    {
                        thread = new Thread(PlayChess);
                        thread.IsBackground = true;
                        thread.Start();
                    }
                    else if (mod == 3)
                    {
                        thread = new Thread(PourWater);
                        thread.IsBackground = true;
                        thread.Start();

                    }
                    else if(mod == 4)
                    {
                        thread = new Thread(AutoGrab);
                        thread.IsBackground = true;
                        thread.Start();
                    }
                    doing = true;
                }
                if(mod==0)
                {

                    if (doing)
                    {

                        thread = new Thread(RES);
                        thread.IsBackground = true;
                        thread.Start();
                        doing = false;
                    }
                    else
                    {
                        mod_str = fileOperations.ReadTxt("action_mod");
                        fileOperations.GetSetoff(mod_str, out int action_mod);
                        if (action_mod == 1)
                        {
                            fileOperations.WriteTxt("action_mod", "0");
                            FistGrip();
                        }
                        else if (action_mod == 2)
                        {
                            fileOperations.WriteTxt("action_mod", "0");
                            SayHello();
                        }
                        else if (action_mod == 3)
                        {
                            fileOperations.WriteTxt("action_mod", "0");
                            Circle();
                        }
                    }

                }


            }
        }
        #endregion
    }
}



