namespace SerialAssistant
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_4DOF = new System.Windows.Forms.Button();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button_3DOF = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.PourWater_button = new System.Windows.Forms.Button();
            this.Stamp_button = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.fixed_point_text = new System.Windows.Forms.TextBox();
            this.textBox_send = new System.Windows.Forms.TextBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.textBoxFolderBrowserDialog = new System.Windows.Forms.TextBox();
            this.textBox_receive = new System.Windows.Forms.TextBox();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.PlayChess_button = new System.Windows.Forms.Button();
            this.textBox_chess = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.serialPort2 = new System.IO.Ports.SerialPort(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.button_4DOF);
            this.panel1.Controls.Add(this.comboBox2);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.button_3DOF);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(3, 12);
            this.panel1.MinimumSize = new System.Drawing.Size(179, 212);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(179, 223);
            this.panel1.TabIndex = 0;
            // 
            // button_4DOF
            // 
            this.button_4DOF.BackColor = System.Drawing.Color.ForestGreen;
            this.button_4DOF.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_4DOF.Location = new System.Drawing.Point(13, 171);
            this.button_4DOF.Name = "button_4DOF";
            this.button_4DOF.Size = new System.Drawing.Size(159, 34);
            this.button_4DOF.TabIndex = 13;
            this.button_4DOF.Text = "连接大手";
            this.button_4DOF.UseVisualStyleBackColor = false;
            this.button_4DOF.Click += new System.EventHandler(this.button_4DOF_Click);
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(78, 123);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(94, 29);
            this.comboBox2.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.Control;
            this.label2.Cursor = System.Windows.Forms.Cursors.Default;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(9, 126);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 21);
            this.label2.TabIndex = 11;
            this.label2.Text = "串   口";
            // 
            // button_3DOF
            // 
            this.button_3DOF.BackColor = System.Drawing.Color.ForestGreen;
            this.button_3DOF.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_3DOF.Location = new System.Drawing.Point(13, 56);
            this.button_3DOF.Name = "button_3DOF";
            this.button_3DOF.Size = new System.Drawing.Size(159, 34);
            this.button_3DOF.TabIndex = 10;
            this.button_3DOF.Text = "连接小手";
            this.button_3DOF.UseVisualStyleBackColor = false;
            this.button_3DOF.Click += new System.EventHandler(this.PortOpen_button_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(78, 8);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(94, 29);
            this.comboBox1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Cursor = System.Windows.Forms.Cursors.Default;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(9, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "串   口";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.PourWater_button);
            this.panel2.Controls.Add(this.Stamp_button);
            this.panel2.Location = new System.Drawing.Point(3, 241);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(180, 214);
            this.panel2.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.button2.Enabled = false;
            this.button2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button2.Location = new System.Drawing.Point(44, 101);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(84, 43);
            this.button2.TabIndex = 1;
            this.button2.Text = "抓取";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.AutoGrab_button_Click);
            // 
            // PourWater_button
            // 
            this.PourWater_button.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.PourWater_button.Enabled = false;
            this.PourWater_button.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.PourWater_button.Location = new System.Drawing.Point(44, 52);
            this.PourWater_button.Name = "PourWater_button";
            this.PourWater_button.Size = new System.Drawing.Size(84, 43);
            this.PourWater_button.TabIndex = 3;
            this.PourWater_button.Text = "倒水";
            this.PourWater_button.UseVisualStyleBackColor = false;
            this.PourWater_button.Click += new System.EventHandler(this.PourWater_button_Click);
            // 
            // Stamp_button
            // 
            this.Stamp_button.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Stamp_button.Enabled = false;
            this.Stamp_button.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Stamp_button.Location = new System.Drawing.Point(44, 3);
            this.Stamp_button.Name = "Stamp_button";
            this.Stamp_button.Size = new System.Drawing.Size(84, 43);
            this.Stamp_button.TabIndex = 2;
            this.Stamp_button.Text = "盖章";
            this.Stamp_button.UseVisualStyleBackColor = false;
            this.Stamp_button.Click += new System.EventHandler(this.Stamp_button_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.button3.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.button3.Location = new System.Drawing.Point(371, 0);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(63, 30);
            this.button3.TabIndex = 11;
            this.button3.Text = "文件夹";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.ChooseFile_button_Click);
            // 
            // panel4
            // 
            this.panel4.AutoScroll = true;
            this.panel4.AutoSize = true;
            this.panel4.Controls.Add(this.label3);
            this.panel4.Controls.Add(this.label8);
            this.panel4.Controls.Add(this.fixed_point_text);
            this.panel4.Controls.Add(this.textBox_send);
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Controls.Add(this.button3);
            this.panel4.Controls.Add(this.textBoxFolderBrowserDialog);
            this.panel4.Controls.Add(this.textBox_receive);
            this.panel4.Location = new System.Drawing.Point(189, 13);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(442, 379);
            this.panel4.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(140, 307);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 17;
            this.label3.Text = "抓棋子位置";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(384, 349);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 14;
            this.label8.Text = "坐标";
            // 
            // fixed_point_text
            // 
            this.fixed_point_text.Location = new System.Drawing.Point(3, 304);
            this.fixed_point_text.Name = "fixed_point_text";
            this.fixed_point_text.ShortcutsEnabled = false;
            this.fixed_point_text.Size = new System.Drawing.Size(122, 21);
            this.fixed_point_text.TabIndex = 16;
            // 
            // textBox_send
            // 
            this.textBox_send.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox_send.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_send.Location = new System.Drawing.Point(1, 339);
            this.textBox_send.Multiline = true;
            this.textBox_send.Name = "textBox_send";
            this.textBox_send.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_send.Size = new System.Drawing.Size(370, 36);
            this.textBox_send.TabIndex = 0;
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Location = new System.Drawing.Point(1, 339);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(434, 35);
            this.panel5.TabIndex = 4;
            // 
            // textBoxFolderBrowserDialog
            // 
            this.textBoxFolderBrowserDialog.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.textBoxFolderBrowserDialog.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxFolderBrowserDialog.Location = new System.Drawing.Point(0, 0);
            this.textBoxFolderBrowserDialog.Multiline = true;
            this.textBoxFolderBrowserDialog.Name = "textBoxFolderBrowserDialog";
            this.textBoxFolderBrowserDialog.ReadOnly = true;
            this.textBoxFolderBrowserDialog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxFolderBrowserDialog.Size = new System.Drawing.Size(369, 30);
            this.textBoxFolderBrowserDialog.TabIndex = 1;
            // 
            // textBox_receive
            // 
            this.textBox_receive.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.textBox_receive.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_receive.Location = new System.Drawing.Point(3, 47);
            this.textBox_receive.Multiline = true;
            this.textBox_receive.Name = "textBox_receive";
            this.textBox_receive.ReadOnly = true;
            this.textBox_receive.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_receive.Size = new System.Drawing.Size(368, 248);
            this.textBox_receive.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Left;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(0, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(106, 21);
            this.label6.TabIndex = 0;
            this.label6.Text = "未链接机械臂";
            // 
            // panel6
            // 
            this.panel6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel6.Controls.Add(this.label9);
            this.panel6.Controls.Add(this.label6);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel6.Location = new System.Drawing.Point(0, 461);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(625, 25);
            this.panel6.TabIndex = 7;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Right;
            this.label9.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.ForeColor = System.Drawing.Color.SeaGreen;
            this.label9.Location = new System.Drawing.Point(548, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(73, 21);
            this.label9.TabIndex = 3;
            this.label9.Text = "@V0.0.1";
            // 
            // PlayChess_button
            // 
            this.PlayChess_button.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.PlayChess_button.Enabled = false;
            this.PlayChess_button.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.PlayChess_button.Location = new System.Drawing.Point(49, 398);
            this.PlayChess_button.Name = "PlayChess_button";
            this.PlayChess_button.Size = new System.Drawing.Size(84, 43);
            this.PlayChess_button.TabIndex = 10;
            this.PlayChess_button.Text = "下棋";
            this.PlayChess_button.UseVisualStyleBackColor = false;
            this.PlayChess_button.Click += new System.EventHandler(this.PlayChess_button_Click);
            // 
            // textBox_chess
            // 
            this.textBox_chess.Location = new System.Drawing.Point(226, 427);
            this.textBox_chess.Name = "textBox_chess";
            this.textBox_chess.Size = new System.Drawing.Size(122, 21);
            this.textBox_chess.TabIndex = 13;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.button4.Enabled = false;
            this.button4.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button4.Location = new System.Drawing.Point(449, 412);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(84, 43);
            this.button4.TabIndex = 14;
            this.button4.Text = "调试";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.button5.Enabled = false;
            this.button5.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button5.Location = new System.Drawing.Point(539, 412);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(84, 43);
            this.button5.TabIndex = 15;
            this.button5.Text = "调试起";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(352, 429);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 18;
            this.label4.Text = "调试坐标";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(625, 486);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.textBox_chess);
            this.Controls.Add(this.PlayChess_button);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "顶超1号强无敌";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button button_3DOF;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_receive;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox_send;
        public System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxFolderBrowserDialog;
        private System.Windows.Forms.Button Stamp_button;
        private System.Windows.Forms.Button PourWater_button;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button PlayChess_button;
        private System.Windows.Forms.TextBox textBox_chess;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox fixed_point_text;
        private System.Windows.Forms.Button button_4DOF;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label2;
        public System.IO.Ports.SerialPort serialPort2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}

