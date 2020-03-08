using NHTools;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace My_Assistant
{
    public partial class MainForm : Form
    {
        string KYKUserName;
        string KYKSifre;
        string KYKLogFileFullPath;
        string CommonLogFolderPath;
        string ADSOYAD;
        int ZamanAsimi;

        ChromeDriverService srv;

        public MainForm()
        {
            InitializeComponent();
            CommonLogFolderPath = Properties.Settings.Default.CommonLogFolderPath;
            KYKLogFileFullPath = Properties.Settings.Default.KYKLogPath;
            KYKUserName = Properties.Settings.Default.KYKUserName;
            KYKSifre = Properties.Settings.Default.KYKPassword;
            ADSOYAD = Properties.Settings.Default.ADSOYAD;
            ZamanAsimi = Properties.Settings.Default.ZamanAsimi;
            if (Properties.Settings.Default.AcilanServisler == null)
            {
                Properties.Settings.Default.AcilanServisler = new StringCollection();
                Properties.Settings.Default.Save();
            }
            if (ZamanAsimi <= 0)
            {
                Properties.Settings.Default.ZamanAsimi = 30;
                ZamanAsimi = Properties.Settings.Default.ZamanAsimi;
                Properties.Settings.Default.Save();
            }
        }

        ChromeDriver driver;

        private void GirisYap()
        {
            try { driver.Navigate().GoToUrl("https://wifi.kyk.gov.tr/login.html"); }
            catch { }
            if (!IsEnteredSucceed(driver))
            {
                try
                {
                    IWebElement txtUserName = driver.FindElementByName("j_username");
                    IWebElement txtPass = driver.FindElementByName("j_password");
                    IWebElement btnSubmit = driver.FindElementByName("submit");
                    txtUserName.SendKeys(KYKUserName);
                    txtPass.SendKeys(KYKSifre);
                    btnSubmit.Click();
                }
                catch { }
            }
        }

        private bool girisyapti = false;
        private void CheckSessionOpen()
        {
            SaveKYKLog("Oturum Kontrol Ediliyor...");
            status1.SetDurum(Durum.Yukleniyor);
            if (driver == null)
            {
                CreateDriver();
            }
            try { driver.Navigate().GoToUrl("https://wifi.kyk.gov.tr/"); }
            catch { }
            Optimizasyon.Delagate(chckKYK, () =>
            {
                string caption = string.Format("30 sn de bir oturumuzu kontrol eder eğer wifi.kyk.gov.tr \nüzerinde oturum açılması gerekiyorsa otomatik giriş yapar.\nEn son kontrol edilme :{0}", DateTime.Now.ToLongTimeString());
                toolTip1.SetToolTip(chckKYK, caption);
            });
            if (IsEnteredSucceed(driver))
            {
                status1.SetDurum(Durum.Aktif);
                if (girisyapti == false)
                {
                    girisyapti = true;
                    GirisYapildi();
                }
            }
            else
            {
                status1.SetDurum(Durum.Pasif);
                girisyapti = false;
                SaveKYKLog("Oturum Kapalı! Oturum Açılıyor...");
                GirisYap();
                if (IsEnteredSucceed(driver))
                {
                    status1.SetDurum(Durum.Aktif);
                }
            }
        }

        private void GirisYapildi()
        {
            status1.SetDurum(Durum.Aktif);
            SaveKYKLog("Oturum Açıldı!");
        }

        private void CreateDriver()
        {
            srv.Start();
            Properties.Settings.Default.AcilanServisler.Add(srv.ProcessId.ToString());
            Properties.Settings.Default.Save();
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("disable-infobars");
            driver = new ChromeDriver(srv, options);
            driver.Manage().Timeouts().PageLoad = new TimeSpan(0, 0, ZamanAsimi);
            driver.Manage().Window.Minimize();
            DriverChanged();
            Optimizasyon.Delagate(this, () => { this.Focus(); });
        }

        private void DeleteDriver()
        {
            if (driver != null)
            {
                driver.Quit();
                driver = null;
                DriverChanged();
            }
        }

        private void DriverChanged()
        {
            bool status = driver != null;
            Delegates.Enabled.Set(button1, status);
            Delegates.Enabled.Set(button2, status);
            Delegates.Enabled.Set(chckKYK, status);
        }

        private bool IsEnteredSucceed(ChromeDriver driver)
        {
            bool value = false;
            try
            {
                if (driver.Url == "https://wifi.kyk.gov.tr/" || driver.Url == "https://wifi.kyk.gov.tr/index.html")
                {
                    ReadOnlyCollection<IWebElement> liste = driver.FindElementsByClassName("myinfo");
                    foreach (IWebElement element in liste)
                    {
                        if (element.Text == ADSOYAD)
                        {
                            value = true;
                            break;
                        }
                    }
                }
            }
            catch { }
            return value;
        }

        private bool IsEnteredInvalid(ChromeDriver driver)
        {
            bool value = false;
            try
            {
                if ((driver.Url == "https://wifi.kyk.gov.tr/")
                && (driver.FindElementByClassName("myinfo") != null))
            {
                ReadOnlyCollection<IWebElement> liste = driver.FindElementsByClassName("myinfo");
                foreach (IWebElement element in liste)
                {
                    if (element.Text == "Invalid username/password.")
                    {
                        value = true;
                        break;
                    }
                }
                }
            }
            catch { }
            return value;
        }

        private void BtnGirisYap_Click(object sender, EventArgs e)
        {
            Optimizasyon.ArkaplandaCalistir(() =>
            {
                SetStatusDelegate(Durum.Yukleniyor);
                Optimizasyon.ArkaplandaCalistir(() => GirisYap());
                if (IsEnteredInvalid(driver))
                {
                    SaveKYKLog("Giriş Yapılamadı!\nGiriş bilgileri geçersiz!");
                    SetStatusDelegate(Durum.Pasif);
                    MessageBox.Show("Giriş Yapılamadı!\nGiriş bilgileri geçersiz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (IsEnteredSucceed(driver))
                {
                    SetStatusDelegate(Durum.Aktif);
                    SaveKYKLog("Giriş Yapıldı!");
                    MessageBox.Show("Giriş Yapıldı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    SaveKYKLog("Giriş Yapılamadı!");
                    SetStatusDelegate(Durum.Pasif);
                    MessageBox.Show("Giriş Yapılamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(CommonLogFolderPath))
            {
                CommonLogFolderPath = Application.StartupPath + "\\Logs";
                Properties.Settings.Default.CommonLogFolderPath = CommonLogFolderPath;
                Properties.Settings.Default.Save();
            }
            srv = ChromeDriverService.CreateDefaultService();
            srv.HideCommandPromptWindow = true;
            StringCollection pids = Properties.Settings.Default.AcilanServisler;
            if (pids != null)
            {
                foreach (string pid in pids)
                {
                    Process prc;
                    try
                    {
                        prc = Process.GetProcessById(int.Parse(pid));
                        prc.Kill();
                    }
                    catch { }
                }
                Properties.Settings.Default.AcilanServisler = new StringCollection();
                Properties.Settings.Default.Save();
            }
            Optimizasyon.ArkaplandaCalistir(() => CreateDriver());
            
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            DeleteDriver();
        }
        private void btnLogout_click(object sender, EventArgs e)
        {
            Optimizasyon.ArkaplandaCalistir(() =>
            {

                SetStatusDelegate(Durum.Yukleniyor);
                driver.Navigate().GoToUrl("https://wifi.kyk.gov.tr/");
                ReadOnlyCollection<IWebElement> liste = driver.FindElementsByClassName("ui-button-text");
                IWebElement btnLogout = null;
                foreach (IWebElement element in liste)
                {
                    if (element.Text == "Logout" || element.Text == "Çıkış")
                    {
                        btnLogout = element;
                        break;
                    }
                }
                if (btnLogout != null)
                {
                    btnLogout.Click();
                    SetStatusDelegate(Durum.Pasif);
                    SaveKYKLog("Çıkış Yapıldı!");
                }
            });
        }

        private void chckKYK_CheckedChanged(object sender, EventArgs e)
        {
            if (Delegates.Enabled.Get(chckKYK))
            {
                Optimizasyon.ArkaplandaCalistir(() => CheckSessionOpen());
                tmrKYK.Start();
            }
            else
            {
                tmrKYK.Stop();
            }
        }

        private void tmrKYK_Tick(object sender, EventArgs e)
        {
            Optimizasyon.ArkaplandaCalistir(() => CheckSessionOpen());
        }

        private void SetStatusDelegate(Durum durum)
        {
            Optimizasyon.Delagate(status1, () => { status1.SetDurum(durum); });
        }
        private void SaveKYKLog(string logText)
        {
            if (!Directory.Exists(CommonLogFolderPath))
            {
                Directory.CreateDirectory(CommonLogFolderPath);
            }
            if (!string.IsNullOrEmpty(KYKLogFileFullPath) && !File.Exists(KYKLogFileFullPath))
            {
                File.WriteAllText(KYKLogFileFullPath, string.Empty);
            }
            Optimizasyon.ArkaplandaCalistir(async () => { await SaveLogAsync(KYKLogFileFullPath, logText); });
        }
        private async Task SaveLogAsync(string logFileFullPath, string logText)
        {
            try
            {
                using (StreamWriter sr = new StreamWriter(logFileFullPath, true))
                {
                    await sr.WriteLineAsync(string.Format("{0}, {1} : {2}", DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString(), logText));
                }
            }
            catch { }
        }

        private void btnKYKLogShow_Click(object sender, EventArgs e)
        {
            using (ShowLogs frmLogShow = new ShowLogs(KYKLogFileFullPath))
            {
                frmLogShow.ShowDialog();
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            using (SettingsForm frmSett = new SettingsForm())
            {
                int tmpZamanAsimi = Properties.Settings.Default.ZamanAsimi;
                frmSett.ShowDialog();
                KYKLogFileFullPath = Properties.Settings.Default.KYKLogPath;
                KYKUserName = Properties.Settings.Default.KYKUserName;
                KYKSifre = Properties.Settings.Default.KYKPassword;
                ADSOYAD = Properties.Settings.Default.ADSOYAD;
                ZamanAsimi = Properties.Settings.Default.ZamanAsimi;
                if(tmpZamanAsimi != ZamanAsimi)
                {
                    DeleteDriver();
                    CreateDriver();
                }
            }
        }
    }
}
