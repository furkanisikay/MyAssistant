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
    public partial class ShowLogs : Form
    {
        public ShowLogs()
        {
            InitializeComponent();
        }

        public ShowLogs(string LogFileFullPath)
        {
            InitializeComponent();
            this.LogFileFullPath = LogFileFullPath;
        }

        private readonly string LogFileFullPath;
        private void btnClear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Tüm kayıtları silmek istediğinize emin misiniz?", "Soru", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                richTextBox1.Clear();
                try { File.WriteAllText(LogFileFullPath,string.Empty); }
                catch (Exception ex) { MessageBox.Show("İşlem yürütülürken bir hata oluştu!\nHata Mesajı:" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }

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
