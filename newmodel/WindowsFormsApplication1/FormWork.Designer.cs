namespace WindowsFormsApplication1
{
    partial class FormWork
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonloadmodelfile = new System.Windows.Forms.Button();
            this.pictureBoxworkshow = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxxpoint = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxypoint = new System.Windows.Forms.TextBox();
            this.textBoxzpoint = new System.Windows.Forms.TextBox();
            this.textBoxmodelfilepath = new System.Windows.Forms.TextBox();
            this.buttonstartwork = new System.Windows.Forms.Button();
            this.buttonstopwork = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.textBoxstartmarkshuzi = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxworkshow)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonloadmodelfile
            // 
            this.buttonloadmodelfile.Location = new System.Drawing.Point(319, 29);
            this.buttonloadmodelfile.Name = "buttonloadmodelfile";
            this.buttonloadmodelfile.Size = new System.Drawing.Size(133, 29);
            this.buttonloadmodelfile.TabIndex = 0;
            this.buttonloadmodelfile.Text = "加载模板";
            this.buttonloadmodelfile.UseVisualStyleBackColor = true;
            this.buttonloadmodelfile.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBoxworkshow
            // 
            this.pictureBoxworkshow.Location = new System.Drawing.Point(6, 67);
            this.pictureBoxworkshow.Name = "pictureBoxworkshow";
            this.pictureBoxworkshow.Size = new System.Drawing.Size(887, 477);
            this.pictureBoxworkshow.TabIndex = 1;
            this.pictureBoxworkshow.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.textBoxmodelfilepath);
            this.groupBox1.Controls.Add(this.pictureBoxworkshow);
            this.groupBox1.Controls.Add(this.buttonloadmodelfile);
            this.groupBox1.Location = new System.Drawing.Point(15, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(902, 573);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "打印预览";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.textBoxxpoint);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.textBoxypoint);
            this.groupBox2.Controls.Add(this.textBoxzpoint);
            this.groupBox2.Location = new System.Drawing.Point(479, 20);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(414, 35);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(275, 74);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(133, 32);
            this.button3.TabIndex = 7;
            this.button3.Text = "模板和位置保存";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(94, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(11, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "Y";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(73, 47);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(133, 32);
            this.button1.TabIndex = 2;
            this.button1.Text = "获取三轴的位置";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(184, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(11, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "Z";
            // 
            // textBoxxpoint
            // 
            this.textBoxxpoint.Location = new System.Drawing.Point(22, 85);
            this.textBoxxpoint.Name = "textBoxxpoint";
            this.textBoxxpoint.Size = new System.Drawing.Size(67, 21);
            this.textBoxxpoint.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(11, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "X";
            // 
            // textBoxypoint
            // 
            this.textBoxypoint.Location = new System.Drawing.Point(111, 85);
            this.textBoxypoint.Name = "textBoxypoint";
            this.textBoxypoint.Size = new System.Drawing.Size(67, 21);
            this.textBoxypoint.TabIndex = 5;
            // 
            // textBoxzpoint
            // 
            this.textBoxzpoint.Location = new System.Drawing.Point(202, 85);
            this.textBoxzpoint.Name = "textBoxzpoint";
            this.textBoxzpoint.Size = new System.Drawing.Size(67, 21);
            this.textBoxzpoint.TabIndex = 6;
            // 
            // textBoxmodelfilepath
            // 
            this.textBoxmodelfilepath.Location = new System.Drawing.Point(6, 34);
            this.textBoxmodelfilepath.Name = "textBoxmodelfilepath";
            this.textBoxmodelfilepath.Size = new System.Drawing.Size(307, 21);
            this.textBoxmodelfilepath.TabIndex = 3;
            this.textBoxmodelfilepath.TextChanged += new System.EventHandler(this.textBoxmodelfilepath_TextChanged);
            // 
            // buttonstartwork
            // 
            this.buttonstartwork.Location = new System.Drawing.Point(923, 311);
            this.buttonstartwork.Name = "buttonstartwork";
            this.buttonstartwork.Size = new System.Drawing.Size(133, 58);
            this.buttonstartwork.TabIndex = 3;
            this.buttonstartwork.Text = "自动运行";
            this.buttonstartwork.UseVisualStyleBackColor = true;
            this.buttonstartwork.Click += new System.EventHandler(this.buttonstartwork_Click);
            // 
            // buttonstopwork
            // 
            this.buttonstopwork.Location = new System.Drawing.Point(923, 594);
            this.buttonstopwork.Name = "buttonstopwork";
            this.buttonstopwork.Size = new System.Drawing.Size(133, 58);
            this.buttonstopwork.TabIndex = 4;
            this.buttonstopwork.Text = "紧急停止";
            this.buttonstopwork.UseVisualStyleBackColor = true;
            this.buttonstopwork.Click += new System.EventHandler(this.buttonstopwork_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("SimSun", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(29, 619);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(246, 33);
            this.label1.TabIndex = 5;
            this.label1.Text = "起始打印数字：";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(923, 87);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(133, 58);
            this.button2.TabIndex = 7;
            this.button2.Text = "三轴归零";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(987, 151);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(69, 38);
            this.button4.TabIndex = 8;
            this.button4.Text = "归零复位";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // textBoxstartmarkshuzi
            // 
            this.textBoxstartmarkshuzi.Location = new System.Drawing.Point(253, 631);
            this.textBoxstartmarkshuzi.Name = "textBoxstartmarkshuzi";
            this.textBoxstartmarkshuzi.Size = new System.Drawing.Size(75, 21);
            this.textBoxstartmarkshuzi.TabIndex = 10;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(923, 409);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(133, 64);
            this.button5.TabIndex = 11;
            this.button5.Text = "激光标刻暂停";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // FormWork
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1124, 672);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.textBoxstartmarkshuzi);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonstopwork);
            this.Controls.Add(this.buttonstartwork);
            this.Controls.Add(this.groupBox1);
            this.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.Name = "FormWork";
            this.Text = "work";
            this.Load += new System.EventHandler(this.FormWork_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxworkshow)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonloadmodelfile;
        private System.Windows.Forms.PictureBox pictureBoxworkshow;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonstartwork;
        private System.Windows.Forms.Button buttonstopwork;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxzpoint;
        private System.Windows.Forms.TextBox textBoxypoint;
        private System.Windows.Forms.TextBox textBoxxpoint;
        private System.Windows.Forms.TextBox textBoxmodelfilepath;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox textBoxstartmarkshuzi;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}