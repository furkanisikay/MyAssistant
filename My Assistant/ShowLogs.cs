using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NHTools;

namespace My_Assistant
{
    /// <summary>
    /// Log kayıtlarını görüntüleme ve temizleme penceresi.
    /// Belirtilen log dosyasının içeriğini okuyarak kullanıcıya sunar.
    /// </summary>
    public partial class ShowLogs : Form
    {
        public ShowLogs()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Log dosyasının tam yolunu alarak formu başlatır.
        /// </summary>
        /// <param name="LogFileFullPath">Görüntülenecek log dosyasının tam yolu</param>
        public ShowLogs(string LogFileFullPath)
        {
            InitializeComponent();
            this.LogFileFullPath = LogFileFullPath;
        }

        // Görüntülenecek log dosyasının tam yolu
        private readonly string LogFileFullPath;

        /// <summary>
        /// Tüm log kayıtlarını siler. Kullanıcıdan onay alındıktan sonra işlem gerçekleşir.
        /// </summary>
        private void btnClear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Tüm kayıtları silmek istediğinize emin misiniz?", "Soru", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                richTextBox1.Clear();
                try { File.WriteAllText(LogFileFullPath,string.Empty); }
                catch (Exception ex) { MessageBox.Show("İşlem yürütülürken bir hata oluştu!\nHata Mesajı:" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }

        /// <summary>
        /// Form gösterildiğinde log dosyasını arka planda okuyarak metin kutusuna yükler.
        /// </summary>
        private void ShowLogs_Shown(object sender, EventArgs e)
        {
            Delegates.Text.Set(richTextBox1, "Kayıtlar Yükleniyor...");
            Optimizasyon.ArkaplandaCalistir(()=>
            {
                if (!string.IsNullOrEmpty(LogFileFullPath) && File.Exists(LogFileFullPath))
                {
                    try { Delegates.Text.Set(richTextBox1, File.ReadAllText(LogFileFullPath)); }
                    catch (Exception ex) { Delegates.Text.Set(richTextBox1, "Kayıtlar Yüklenemedi!");  MessageBox.Show("Kayıtlar açılırken bir hata oluştu!\nHata Mesajı:" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
            });

            
        }
    }
}
