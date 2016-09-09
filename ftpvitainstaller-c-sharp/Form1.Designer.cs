namespace ftpvitainstaller_c_sharp
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.gameNameLabel = new System.Windows.Forms.Label();
            this.GameGroupBox = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.fileSizeLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.progressLabel = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.portBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ipBox = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.GameGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(14, 27);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(104, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Open Game...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // gameNameLabel
            // 
            this.gameNameLabel.AutoSize = true;
            this.gameNameLabel.Location = new System.Drawing.Point(124, 32);
            this.gameNameLabel.Name = "gameNameLabel";
            this.gameNameLabel.Size = new System.Drawing.Size(33, 13);
            this.gameNameLabel.TabIndex = 1;
            this.gameNameLabel.Text = "None";
            // 
            // GameGroupBox
            // 
            this.GameGroupBox.Controls.Add(this.pictureBox1);
            this.GameGroupBox.Controls.Add(this.fileSizeLabel);
            this.GameGroupBox.Controls.Add(this.gameNameLabel);
            this.GameGroupBox.Controls.Add(this.button1);
            this.GameGroupBox.Location = new System.Drawing.Point(12, 12);
            this.GameGroupBox.Name = "GameGroupBox";
            this.GameGroupBox.Size = new System.Drawing.Size(304, 237);
            this.GameGroupBox.TabIndex = 2;
            this.GameGroupBox.TabStop = false;
            this.GameGroupBox.Text = "Game";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.ErrorImage = null;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(17, 85);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(275, 146);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // fileSizeLabel
            // 
            this.fileSizeLabel.AutoSize = true;
            this.fileSizeLabel.Location = new System.Drawing.Point(14, 57);
            this.fileSizeLabel.Name = "fileSizeLabel";
            this.fileSizeLabel.Size = new System.Drawing.Size(52, 13);
            this.fileSizeLabel.TabIndex = 2;
            this.fileSizeLabel.Text = "File Size: ";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.progressBar2);
            this.groupBox1.Controls.Add(this.progressLabel);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.statusLabel);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.portBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.ipBox);
            this.groupBox1.Controls.Add(this.progressBar1);
            this.groupBox1.Location = new System.Drawing.Point(13, 256);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(303, 211);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connection";
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(6, 163);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(285, 10);
            this.progressBar2.TabIndex = 8;
            // 
            // progressLabel
            // 
            this.progressLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.progressLabel.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.progressLabel.Location = new System.Drawing.Point(6, 176);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(285, 32);
            this.progressLabel.TabIndex = 7;
            this.progressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(6, 58);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(285, 40);
            this.button2.TabIndex = 6;
            this.button2.Text = "Install";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // statusLabel
            // 
            this.statusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.Location = new System.Drawing.Point(6, 101);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(285, 35);
            this.statusLabel.TabIndex = 5;
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(220, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(10, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = ":";
            // 
            // portBox
            // 
            this.portBox.Location = new System.Drawing.Point(236, 32);
            this.portBox.Name = "portBox";
            this.portBox.Size = new System.Drawing.Size(55, 20);
            this.portBox.TabIndex = 3;
            this.portBox.Text = "1337";
            this.portBox.TextChanged += new System.EventHandler(this.portBox_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "PSVita IP:";
            // 
            // ipBox
            // 
            this.ipBox.Location = new System.Drawing.Point(71, 32);
            this.ipBox.Name = "ipBox";
            this.ipBox.Size = new System.Drawing.Size(143, 20);
            this.ipBox.TabIndex = 1;
            this.ipBox.Text = "192.168.1.25";
            this.ipBox.TextChanged += new System.EventHandler(this.ipBox_TextChanged);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(6, 139);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(285, 23);
            this.progressBar1.TabIndex = 0;
            this.progressBar1.Value = 50;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(327, 479);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.GameGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "PSVita FTP VPK Installer";
            this.GameGroupBox.ResumeLayout(false);
            this.GameGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label gameNameLabel;
        private System.Windows.Forms.GroupBox GameGroupBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox portBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ipBox;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label fileSizeLabel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label progressLabel;
        private System.Windows.Forms.ProgressBar progressBar2;
    }
}

