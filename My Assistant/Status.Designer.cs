namespace My_Assistant
{
    partial class Status
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

        #region Bileşen Tasarımcısı üretimi kod

        /// <summary> 
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblText = new System.Windows.Forms.Label();
            this.pBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pBox)).BeginInit();
            this.SuspendLayout();
            // 
            // lblText
            // 
            this.lblText.AutoSize = true;
            this.lblText.Location = new System.Drawing.Point(-2, 5);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(47, 13);
            this.lblText.TabIndex = 0;
            this.lblText.Text = "Durum : ";
            // 
            // pBox
            // 
            this.pBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pBox.Image = global::My_Assistant.Properties.Resources.close_window_24px;
            this.pBox.Location = new System.Drawing.Point(51, 0);
            this.pBox.Name = "pBox";
            this.pBox.Size = new System.Drawing.Size(22, 23);
            this.pBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pBox.TabIndex = 1;
            this.pBox.TabStop = false;
            // 
            // Status
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.pBox);
            this.Controls.Add(this.lblText);
            this.Name = "Status";
            this.Size = new System.Drawing.Size(76, 23);
            ((System.ComponentModel.ISupportInitialize)(this.pBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblText;
        private System.Windows.Forms.PictureBox pBox;
    }
}
