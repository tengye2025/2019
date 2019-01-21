namespace HGControlLibrary
{
    partial class LeftButtons
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LeftButtons));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonlogo = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonwork = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtondebug = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonabout = new System.Windows.Forms.ToolStripButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.BackColor = System.Drawing.Color.White;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonlogo,
            this.toolStripLabel1,
            this.toolStripLabel2,
            this.toolStripSeparator2,
            this.toolStripButtonwork,
            this.toolStripSeparator1,
            this.toolStripButtondebug,
            this.toolStripSeparator3,
            this.toolStripButtonabout});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.MaximumSize = new System.Drawing.Size(100, 0);
            this.toolStrip1.MinimumSize = new System.Drawing.Size(100, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(100, 526);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // toolStripButtonlogo
            // 
            this.toolStripButtonlogo.AutoSize = false;
            this.toolStripButtonlogo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonlogo.DoubleClickEnabled = true;
            this.toolStripButtonlogo.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonlogo.Image")));
            this.toolStripButtonlogo.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonlogo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonlogo.Name = "toolStripButtonlogo";
            this.toolStripButtonlogo.Size = new System.Drawing.Size(98, 80);
            this.toolStripButtonlogo.Text = "登录";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(98, 15);
            this.toolStripLabel1.Text = "toolStripLabel1";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(98, 15);
            this.toolStripLabel2.Text = "toolStripLabel2";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(98, 6);
            // 
            // toolStripButtonwork
            // 
            this.toolStripButtonwork.AutoSize = false;
            this.toolStripButtonwork.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonwork.ForeColor = System.Drawing.Color.Black;
            this.toolStripButtonwork.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonwork.Image")));
            this.toolStripButtonwork.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonwork.Name = "toolStripButtonwork";
            this.toolStripButtonwork.Size = new System.Drawing.Size(98, 40);
            this.toolStripButtonwork.Text = "工作界面";
            this.toolStripButtonwork.Click += new System.EventHandler(this.toolStripButton1_Click_1);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(98, 6);
            // 
            // toolStripButtondebug
            // 
            this.toolStripButtondebug.AutoSize = false;
            this.toolStripButtondebug.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtondebug.ForeColor = System.Drawing.Color.Black;
            this.toolStripButtondebug.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtondebug.Image")));
            this.toolStripButtondebug.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtondebug.Name = "toolStripButtondebug";
            this.toolStripButtondebug.Size = new System.Drawing.Size(98, 40);
            this.toolStripButtondebug.Text = "系统调试";
            this.toolStripButtondebug.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(98, 6);
            // 
            // toolStripButtonabout
            // 
            this.toolStripButtonabout.AutoSize = false;
            this.toolStripButtonabout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonabout.ForeColor = System.Drawing.Color.Black;
            this.toolStripButtonabout.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonabout.Image")));
            this.toolStripButtonabout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonabout.Name = "toolStripButtonabout";
            this.toolStripButtonabout.Size = new System.Drawing.Size(98, 40);
            this.toolStripButtonabout.Text = "关于";
            this.toolStripButtonabout.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // LeftButtons
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.toolStrip1);
            this.Name = "LeftButtons";
            this.Size = new System.Drawing.Size(444, 526);
            this.Load += new System.EventHandler(this.UserControl1_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolStripButton toolStripButtonlogo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButtonwork;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripButton toolStripButtondebug;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButtonabout;
        private System.Windows.Forms.ToolStrip toolStrip1;
    }
}
