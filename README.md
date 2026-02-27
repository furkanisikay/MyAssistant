# MyAssistant — KYK Wi-Fi Otomatik Oturum Yöneticisi

![C#](https://img.shields.io/badge/C%23-.NET%20Framework%204.5-blue?logo=csharp)
![Selenium](https://img.shields.io/badge/Selenium-3.141.0-43B02A?logo=selenium)
![Chrome](https://img.shields.io/badge/Chrome-WebDriver-4285F4?logo=googlechrome)
![Lisans](https://img.shields.io/badge/Lisans-MIT-green)
![Platform](https://img.shields.io/badge/Platform-Windows-0078D6?logo=windows)

## Neden Bu Proje?

GSB / KYK yurtlarındaki Wi-Fi ağı, kullanıcıların **8 saatte bir** `wifi.kyk.gov.tr` portalı üzerinden oturum açmasını zorunlu kılmaktadır. Bu durum özellikle gece saatlerinde veya yoğun çalışma sırasında bağlantı kopmasına ve iş kaybına neden olur. **MyAssistant**, Selenium ChromeDriver kullanarak bu oturum yenileme sürecini tamamen otomatikleştirir; oturum kapandığında **30 saniyede bir** kontrol ederek anında yeniden giriş yapar ve kullanıcının kesintisiz internet erişimine sahip olmasını sağlar.

## Mimari ve Özellikler

- **Otomatik Oturum Yenileme**: 30 saniyelik periyodik kontrol ile oturum kapandığında anında yeniden giriş
- **Selenium ChromeDriver Entegrasyonu**: Gömülü Chrome tarayıcı üzerinden portal otomasyonu
- **Kullanıcı Dostu Arayüz**: Windows Forms tabanlı, minimalist ve Türkçe arayüz
- **Canlı Durum Göstergesi**: Bağlantı durumunu (Aktif / Pasif / Yükleniyor) görsel simgelerle anlık takip
- **Log Kayıt Sistemi**: Tüm oturum işlemlerini tarih ve saatle birlikte dosyaya kaydeder
- **Ayarlanabilir Zaman Aşımı**: Sayfa yükleme süresini 1-60 saniye arasında özelleştirme
- **Süreç Yönetimi**: Uygulama kapanışında ChromeDriver süreçlerini otomatik temizleme
- **Tek Tuşla İşlem**: Manuel oturum açma ve çıkış yapma butonları

## Hızlı Başlangıç

### Gereksinimler

- **İşletim Sistemi**: Windows 7 veya üzeri
- **Google Chrome**: Bilgisayarınızda yüklü olmalıdır
- **IDE**: Visual Studio 2019 veya üzeri (derleme için)
- **.NET Framework**: 4.5 veya üzeri

### Kurulum

```bash
# 1. Depoyu klonlayın
git clone https://github.com/furkanisikay/MyAssistant.git
cd MyAssistant

# 2. Projeyi Visual Studio ile açın
start MyAssistant.sln
```

> **Not:** Proje derlenirken `Optimizasyon.dll` bağımlılığının `lib/` klasöründe bulunması gerekir. Detaylar için [`lib/README.md`](lib/README.md) dosyasına bakın.

### NuGet Paketlerini Geri Yükleme

Visual Studio üzerinden projeyi açtıktan sonra:

1. **Araçlar** → **NuGet Paket Yöneticisi** → **Paket Yöneticisi Konsolu**
2. Konsolda şu komutu çalıştırın:

```powershell
nuget restore MyAssistant.sln
```

Ya da çözüm gezgininde projeye sağ tıklayıp **NuGet Paketlerini Geri Yükle** seçeneğini kullanın.

## Ortam Kurulumu

1. **Dişli çark (⚙️) simgesine** tıklayarak Ayarlar penceresini açın.
2. Aşağıdaki bilgileri girin:

   | Alan | Açıklama |
   |------|----------|
   | **KYK Kullanıcı Adı** | KYK Wi-Fi portalı kullanıcı adınız |
   | **KYK Şifre** | KYK Wi-Fi portalı şifreniz |
   | **Ad Soyad** | Portaldaki ad soyad bilginiz (oturum doğrulama için) |
   | **Log Dosyası** | İşlem kayıtlarının kaydedileceği dosya yolu |
   | **Sayfa Zaman Aşımı** | Sayfa yükleme zaman aşımı süresi (1-60 saniye) |

3. **Kaydet** butonuna tıklayın.
4. Ana ekranda **Otomatik Oturum Aç** kutucuğunu işaretleyin.

> ⚠️ **Önemli**: Program çalışırken açılan Chrome penceresini **kapatmayın** (simge durumuna küçültebilirsiniz). Pencere kapanırsa otomatik oturum açma çalışmaz.

## Kullanım

| Buton / Özellik | Açıklama |
|------------------|----------|
| ✅ **Otomatik Oturum Aç** | 30 saniyede bir oturumu kontrol eder, kapanmışsa otomatik giriş yapar |
| 🔑 **Oturum Aç** | Tek tuşla manuel oturum açar |
| 🚪 **Çıkış Yap** | Tek tuşla oturumdan çıkış yapar |
| 📋 **... (Log)** | İşlem kayıtlarını görüntüler ve temizleme imkanı sunar |
| ⚙️ **Ayarlar** | Kullanıcı bilgileri ve program yapılandırmasını düzenler |

## Ekran Görüntüleri

| Ana Ekran | Ayarlar | Log Kayıtları |
|-----------|---------|---------------|
| ![Ana Ekran](https://i.hizliresim.com/VPNixY.png) | ![Ayarlar](https://i.hizliresim.com/qV1yzv.png) | ![Loglar](https://i.hizliresim.com/Ty5plj.png) |

## Proje Yapısı

```
MyAssistant/
├── My Assistant/
│   ├── MainForm.cs            # Ana form - oturum yönetimi mantığı
│   ├── SettingsForm.cs        # Ayarlar formu - kullanıcı yapılandırması
│   ├── ShowLogs.cs            # Log görüntüleme formu
│   ├── Status.cs              # Durum göstergesi kullanıcı kontrolü
│   ├── Durum.cs               # Bağlantı durumu enum tanımı
│   ├── Program.cs             # Uygulama giriş noktası
│   ├── App.config             # Uygulama yapılandırma dosyası
│   ├── packages.config        # NuGet paket bağımlılıkları
│   └── Properties/            # Proje ayarları ve kaynaklar
├── lib/                       # Harici bağımlılık dosyaları
├── CONTRIBUTING.md            # Katkıda bulunma rehberi
├── LICENSE                    # MIT Lisansı
└── MyAssistant.sln            # Visual Studio çözüm dosyası
```

## Katkıda Bulunma

Katkılarınızı bekliyoruz! Detaylı bilgi için [CONTRIBUTING.md](CONTRIBUTING.md) dosyasına göz atın.

## Lisans

Bu proje [MIT Lisansı](LICENSE) ile lisanslanmıştır. Detaylar için `LICENSE` dosyasına bakın.

---

<p align="center">
  <sub>Furkan IŞIKAY tarafından ❤️ ile geliştirilmiştir.</sub>
</p>

