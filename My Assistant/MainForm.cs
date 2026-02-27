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
    /// <summary>
    /// Uygulamanın ana formu.
    /// KYK Wi-Fi oturum yönetimini (giriş, çıkış, otomatik kontrol) sağlar.
    /// </summary>
    public partial class MainForm : Form
    {
        // Kullanıcı ayarlarından okunan KYK giriş bilgileri
        string KYKUserName;
        string KYKSifre;
        string KYKLogFileFullPath;
        string CommonLogFolderPath;
        string ADSOYAD;

        // Sayfa yükleme zaman aşımı süresi (saniye)
        int ZamanAsimi;

        // Chrome tarayıcı servis nesnesi
        ChromeDriverService srv;

        /// <summary>
        /// Ana form yapıcı metodu. Kullanıcı ayarlarını yükler ve varsayılan değerleri atar.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            // Kullanıcı ayarlarını yerel değişkenlere yükle
            CommonLogFolderPath = Properties.Settings.Default.CommonLogFolderPath;
            KYKLogFileFullPath = Properties.Settings.Default.KYKLogPath;
            KYKUserName = Properties.Settings.Default.KYKUserName;
            KYKSifre = Properties.Settings.Default.KYKPassword;
            ADSOYAD = Properties.Settings.Default.ADSOYAD;
            ZamanAsimi = Properties.Settings.Default.ZamanAsimi;

            // Açılan servislerin listesi boşsa yeni bir koleksiyon oluştur
            if (Properties.Settings.Default.AcilanServisler == null)
            {
                Properties.Settings.Default.AcilanServisler = new StringCollection();
                Properties.Settings.Default.Save();
            }

            // Zaman aşımı geçersizse varsayılan 30 saniye olarak ayarla
            if (ZamanAsimi <= 0)
            {
                Properties.Settings.Default.ZamanAsimi = 30;
                ZamanAsimi = Properties.Settings.Default.ZamanAsimi;
                Properties.Settings.Default.Save();
            }
        }

        // Selenium ChromeDriver tarayıcı nesnesi
        ChromeDriver driver;

        /// <summary>
        /// KYK Wi-Fi giriş sayfasına giderek kullanıcı bilgileriyle oturum açar.
        /// </summary>
        private void GirisYap()
        {
            try { driver.Navigate().GoToUrl("https://wifi.kyk.gov.tr/login.html"); }
            catch { }

            // Eğer oturum henüz açılmadıysa giriş bilgilerini doldur ve gönder
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

        // Oturumun daha önce başarıyla açılıp açılmadığını takip eden bayrak
        private bool girisyapti = false;

        /// <summary>
        /// Mevcut oturumun durumunu kontrol eder. Oturum kapalıysa otomatik giriş yapar.
        /// Zamanlayıcı (timer) tarafından periyodik olarak çağrılır.
        /// </summary>
        private void CheckSessionOpen()
        {
            SaveKYKLog("Oturum Kontrol Ediliyor...");
            status1.SetDurum(Durum.Yukleniyor);

            // Tarayıcı henüz oluşturulmadıysa yeni bir tane oluştur
            if (driver == null)
            {
                CreateDriver();
            }
            try { driver.Navigate().GoToUrl("https://wifi.kyk.gov.tr/"); }
            catch { }

            // Araç ipucu metnini son kontrol zamanıyla güncelle
            Optimizasyon.Delagate(chckKYK, () =>
            {
                string caption = string.Format("30 sn de bir oturumuzu kontrol eder eğer wifi.kyk.gov.tr \nüzerinde oturum açılması gerekiyorsa otomatik giriş yapar.\nEn son kontrol edilme :{0}", DateTime.Now.ToLongTimeString());
                toolTip1.SetToolTip(chckKYK, caption);
            });

            // Oturum açıksa durumu güncelle, kapalıysa tekrar giriş yap
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

        /// <summary>
        /// Başarılı giriş sonrası durumu günceller ve log kaydı oluşturur.
        /// </summary>
        private void GirisYapildi()
        {
            status1.SetDurum(Durum.Aktif);
            SaveKYKLog("Oturum Açıldı!");
        }

        /// <summary>
        /// Yeni bir ChromeDriver tarayıcı örneği oluşturur ve yapılandırır.
        /// Servis PID'si ayarlara kaydedilir (uygulama kapanırken temizlenmesi için).
        /// </summary>
        private void CreateDriver()
        {
            srv.Start();

            // Servis PID'sini kaydet (uygulama kapanışında öldürmek için)
            Properties.Settings.Default.AcilanServisler.Add(srv.ProcessId.ToString());
            Properties.Settings.Default.Save();

            ChromeOptions options = new ChromeOptions();
            options.AddArgument("disable-infobars");
            driver = new ChromeDriver(srv, options);

            // Sayfa yükleme zaman aşımını kullanıcı ayarlarından al
            driver.Manage().Timeouts().PageLoad = new TimeSpan(0, 0, ZamanAsimi);
            driver.Manage().Window.Minimize();
            DriverChanged();
            Optimizasyon.Delagate(this, () => { this.Focus(); });
        }

        /// <summary>
        /// Mevcut ChromeDriver tarayıcısını kapatır ve kaynakları serbest bırakır.
        /// </summary>
        private void DeleteDriver()
        {
            if (driver != null)
            {
                driver.Quit();
                driver = null;
                DriverChanged();
            }
        }

        /// <summary>
        /// Tarayıcı durumu değiştiğinde arayüz butonlarının aktif/pasif durumunu günceller.
        /// </summary>
        private void DriverChanged()
        {
            bool status = driver != null;
            Delegates.Enabled.Set(button1, status);
            Delegates.Enabled.Set(button2, status);
            Delegates.Enabled.Set(chckKYK, status);
        }

        /// <summary>
        /// Oturumun başarıyla açılıp açılmadığını kontrol eder.
        /// Sayfadaki "myinfo" sınıfındaki elemanlarda ad soyad bilgisini arar.
        /// </summary>
        /// <param name="driver">Kontrol yapılacak ChromeDriver nesnesi</param>
        /// <returns>Oturum açıksa true, değilse false</returns>
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

        /// <summary>
        /// Giriş bilgilerinin geçersiz olup olmadığını kontrol eder.
        /// Sayfada "Invalid username/password." mesajını arar.
        /// </summary>
        /// <param name="driver">Kontrol yapılacak ChromeDriver nesnesi</param>
        /// <returns>Giriş bilgileri geçersizse true, değilse false</returns>
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

        /// <summary>
        /// "Oturum Aç" butonuna tıklandığında çalışır.
        /// Arka planda giriş işlemini başlatır ve sonucu kullanıcıya bildirir.
        /// </summary>
        private void BtnGirisYap_Click(object sender, EventArgs e)
        {
            Optimizasyon.ArkaplandaCalistir(() =>
            {
                SetStatusDelegate(Durum.Yukleniyor);
                Optimizasyon.ArkaplandaCalistir(() => GirisYap());

                // Geçersiz giriş bilgileri kontrolü
                if (IsEnteredInvalid(driver))
                {
                    SaveKYKLog("Giriş Yapılamadı!\nGiriş bilgileri geçersiz!");
                    SetStatusDelegate(Durum.Pasif);
                    MessageBox.Show("Giriş Yapılamadı!\nGiriş bilgileri geçersiz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Başarılı giriş kontrolü
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

        /// <summary>
        /// Form yüklendiğinde çalışır. Log klasörünü ayarlar, ChromeDriver servisini başlatır
        /// ve önceki çalışmadan kalan ChromeDriver süreçlerini temizler.
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            // Log klasörü belirtilmemişse uygulama dizininde oluştur
            if (string.IsNullOrEmpty(CommonLogFolderPath))
            {
                CommonLogFolderPath = Application.StartupPath + "\\Logs";
                Properties.Settings.Default.CommonLogFolderPath = CommonLogFolderPath;
                Properties.Settings.Default.Save();
            }
            srv = ChromeDriverService.CreateDefaultService();
            srv.HideCommandPromptWindow = true;

            // Önceki çalışmadan kalan ChromeDriver süreçlerini sonlandır
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

            // Tarayıcıyı arka planda oluştur
            Optimizasyon.ArkaplandaCalistir(() => CreateDriver());
            
        }

        /// <summary>
        /// Form kapatıldığında çalışır. ChromeDriver tarayıcısını temizler.
        /// </summary>
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            DeleteDriver();
        }

        /// <summary>
        /// "Çıkış Yap" butonuna tıklandığında çalışır.
        /// Wi-Fi portalındaki çıkış butonunu bulup tıklar.
        /// </summary>
        private void btnLogout_click(object sender, EventArgs e)
        {
            Optimizasyon.ArkaplandaCalistir(() =>
            {

                SetStatusDelegate(Durum.Yukleniyor);
                driver.Navigate().GoToUrl("https://wifi.kyk.gov.tr/");

                // Sayfadaki "Logout" veya "Çıkış" butonunu bul
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

        /// <summary>
        /// "Otomatik Oturum Aç" onay kutusu durumu değiştiğinde çalışır.
        /// İşaretlendiğinde zamanlayıcıyı başlatır, kaldırıldığında durdurur.
        /// </summary>
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

        /// <summary>
        /// Zamanlayıcı her tetiklendiğinde (30 saniyede bir) oturum kontrolü yapar.
        /// </summary>
        private void tmrKYK_Tick(object sender, EventArgs e)
        {
            Optimizasyon.ArkaplandaCalistir(() => CheckSessionOpen());
        }

        /// <summary>
        /// Durum göstergesini UI thread üzerinden güvenli şekilde günceller.
        /// </summary>
        /// <param name="durum">Yeni durum değeri</param>
        private void SetStatusDelegate(Durum durum)
        {
            Optimizasyon.Delagate(status1, () => { status1.SetDurum(durum); });
        }

        /// <summary>
        /// Belirtilen log metnini KYK log dosyasına kaydeder.
        /// Log klasörü veya dosyası yoksa otomatik oluşturur.
        /// </summary>
        /// <param name="logText">Kaydedilecek log metni</param>
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

        /// <summary>
        /// Log metnini tarih ve saat bilgisiyle birlikte dosyaya asenkron olarak yazar.
        /// </summary>
        /// <param name="logFileFullPath">Log dosyasının tam yolu</param>
        /// <param name="logText">Yazılacak log metni</param>
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

        /// <summary>
        /// Log kayıtlarını gösteren pencereyi açar.
        /// </summary>
        private void btnKYKLogShow_Click(object sender, EventArgs e)
        {
            using (ShowLogs frmLogShow = new ShowLogs(KYKLogFileFullPath))
            {
                frmLogShow.ShowDialog();
            }
        }

        /// <summary>
        /// Ayarlar penceresini açar. Kapandığında güncel ayarları yeniden yükler.
        /// Zaman aşımı değişmişse tarayıcıyı yeniden oluşturur.
        /// </summary>
        private void btnSettings_Click(object sender, EventArgs e)
        {
            using (SettingsForm frmSett = new SettingsForm())
            {
                int tmpZamanAsimi = Properties.Settings.Default.ZamanAsimi;
                frmSett.ShowDialog();

                // Ayarları yeniden yükle
                KYKLogFileFullPath = Properties.Settings.Default.KYKLogPath;
                KYKUserName = Properties.Settings.Default.KYKUserName;
                KYKSifre = Properties.Settings.Default.KYKPassword;
                ADSOYAD = Properties.Settings.Default.ADSOYAD;
                ZamanAsimi = Properties.Settings.Default.ZamanAsimi;

                // Zaman aşımı değiştiyse tarayıcıyı yeniden başlat
                if(tmpZamanAsimi != ZamanAsimi)
                {
                    DeleteDriver();
                    CreateDriver();
                }
            }
        }
    }
}
