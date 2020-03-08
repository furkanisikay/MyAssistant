namespace My_Assistant
{
    partial class MainForm
    {
        /// <summary>
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer üretilen kod

        /// <summary>
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnKYKLogShow = new System.Windows.Forms.Button();
            this.chckKYK = new System.Windows.Forms.CheckBox();
            this.status1 = new My_Assistant.Status();
            this.tmrKYK = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(6, 74);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(86, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Oturum Aç";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.BtnGirisYap_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(98, 74);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(87, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Çıkış Yap";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.btnLogout_click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSettings);
            this.groupBox1.Controls.Add(this.btnKYKLogShow);
            this.groupBox1.Controls.Add(this.chckKYK);
            this.groupBox1.Controls.Add(this.status1);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Location = new System.Drawing.Point(12, 66);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(191, 103);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "KYK-WiFi";
            // 
            // btnSettings
            // 
            this.btnSettings.Image = global::My_Assistant.Properties.Resources.settings_16px;
            this.btnSettings.Location = new System.Drawing.Point(160, 44);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(25, 25);
            this.btnSettings.TabIndex = 5;
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnKYKLogShow
            // 
            this.btnKYKLogShow.Location = new System.Drawing.Point(160, 15);
            this.btnKYKLogShow.Name = "btnKYKLogShow";
            this.btnKYKLogShow.Size = new System.Drawing.Size(25, 23);
            this.btnKYKLogShow.TabIndex = 4;
            this.btnKYKLogShow.Text = "...";
            this.btnKYKLogShow.UseVisualStyleBackColor = true;
            this.btnKYKLogShow.Click += new System.EventHandler(this.btnKYKLogShow_Click);
            // 
            // chckKYK
            // 
            this.chckKYK.AutoSize = true;
            this.chckKYK.Enabled = false;
            this.chckKYK.Location = new System.Drawing.Point(11, 19);
            this.chckKYK.Name = "chckKYK";
            this.chckKYK.Size = new System.Drawing.Size(121, 17);
            this.chckKYK.TabIndex = 3;
            this.chckKYK.Text = "Otomatik Oturum Aç";
            this.toolTip1.SetToolTip(this.chckKYK, "30 sn de bir oturumuzu kontrol eder eğer wifi.kyk.gov.tr üzerinde oturum açılması" +
        " gerekiyorsa otomatik giriş yapar.");
            this.chckKYK.UseVisualStyleBackColor = true;
            this.chckKYK.CheckedChanged += new System.EventHandler(this.chckKYK_CheckedChanged);
            // 
            // status1
            // 
            this.status1.BackColor = System.Drawing.Color.Transparent;
            this.status1.Location = new System.Drawing.Point(6, 42);
            this.status1.Name = "status1";
            this.status1.Size = new System.Drawing.Size(123, 23);
            this.status1.TabIndex = 3;
            this.status1.Yazi = "Bağlantı Durumu : ";
            // 
            // tmrKYK
            // 
            this.tmrKYK.Interval = 30000;
            this.tmrKYK.Tick += new System.EventHandler(this.tmrKYK_Tick);
            // 
            // toolTip1
            // 
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip1.ToolTipTitle = "Bilgi";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::My_Assistant.Properties.Resources.icon;
            this.pictureBox1.Location = new System.Drawing.Point(83, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 48);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(215, 187);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "My Assistant";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chckKYK;
        private Status status1;
        private System.Windows.Forms.Timer tmrKYK;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnKYKLogShow;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

