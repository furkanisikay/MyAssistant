using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NHTools;

namespace My_Assistant
{
    /// <summary>
    /// Kullanıcı ayarları formu.
    /// KYK kullanıcı adı, şifre, ad soyad, log dosyası yolu ve 
    /// sayfa zaman aşımı gibi ayarların yönetildiği pencere.
    /// </summary>
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// KYK log dosyasının kaydedileceği konumu seçmek için dosya diyaloğu açar.
        /// </summary>
        private void btnKYKLogBrowse_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog()
            {
                InitialDirectory = Properties.Settings.Default.CommonLogFolderPath,
                Filter = "Log Dosyası(*.log)|*.log",
                FileName = "kyk.log"
            })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    Delegates.Text.Set(txtKYKLogPath, sfd.FileName);
                }
            }
        }

        /// <summary>
        /// Tüm ayarları varsayılan değerlerine döndürür.
        /// Kullanıcıdan onay alındıktan sonra işlem gerçekleştirilir.
        /// </summary>
        private void btnDefault_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Ayarları varsayılana döndürmek istediğiniz emin misiniz?","Soru", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Properties.Settings.Default.KYKLogPath = Properties.Settings.Default.CommonLogFolderPath + "\\kyk.log";
                Properties.Settings.Default.KYKUserName = string.Empty;
                Properties.Settings.Default.KYKPassword = string.Empty;
                Properties.Settings.Default.ADSOYAD = string.Empty;
                Properties.Settings.Default.ZamanAsimi = 30;
                Properties.Settings.Default.Save();
            }
        }

        /// <summary>
        /// Formdaki tüm ayar değerlerini kalıcı olarak kaydeder.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.KYKLogPath = Delegates.Text.Get(txtKYKLogPath);
            Properties.Settings.Default.KYKUserName = Delegates.Text.Get(txtUserName);
            Properties.Settings.Default.KYKPassword = Delegates.Text.Get(txtPass);
            Properties.Settings.Default.ADSOYAD = Delegates.Text.Get(txtAdSoyad);
            Optimizasyon.Delagate(trackBar1, () => { Properties.Settings.Default.ZamanAsimi = trackBar1.Value; });
            Properties.Settings.Default.Save();
            MessageBox.Show("Ayarlar başarıyla kaydedildi!","Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Form yüklendiğinde kaydedilmiş ayarları arayüze yükler.
        /// </summary>
        private void SettingsForm_Load(object sender, EventArgs e)
        {
            Delegates.Text.Set(txtKYKLogPath, Properties.Settings.Default.KYKLogPath);
            Delegates.Text.Set(txtUserName, Properties.Settings.Default.KYKUserName);
            Delegates.Text.Set(txtPass, Properties.Settings.Default.KYKPassword);
            Delegates.Text.Set(txtAdSoyad, Properties.Settings.Default.ADSOYAD);
            Optimizasyon.Delagate(trackBar1, () => { trackBar1.Value = Properties.Settings.Default.ZamanAsimi; });
            int Value = -1;
            Optimizasyon.Delagate(trackBar1, () => { Value = trackBar1.Value; });
            Delegates.Text.Set(lblZamanAsimi, string.Format("{0} saniye", Value.ToString()));
        }

        /// <summary>
        /// Şifre alanının görünürlüğünü "Göster" onay kutusuyla değiştirir.
        /// </summary>
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            bool check = false;
            Optimizasyon.Delagate(checkBox1, ()=> 
            {
                check = checkBox1.Checked;
            });
            Optimizasyon.Delagate(txtPass, ()=> 
            {
                txtPass.UseSystemPasswordChar = !check;
            });
        }


        /// <summary>
        /// Zaman aşımı kaydırıcısı değiştiğinde etiketi günceller.
        /// </summary>
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            int Value = -1;
            Optimizasyon.Delagate(trackBar1, () => { Value = trackBar1.Value; });
            Delegates.Text.Set(lblZamanAsimi, string.Format("{0} saniye", Value.ToString()));
        }
    }
}
