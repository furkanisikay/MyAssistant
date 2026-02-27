# Katkıda Bulunma Rehberi

MyAssistant projesine katkıda bulunmak istediğiniz için teşekkür ederiz! Bu rehber, katkı sürecini kolaylaştırmak için hazırlanmıştır.

## Başlamadan Önce

1. Projeyi **fork** edin.
2. Kendi fork'unuzu yerel makinenize klonlayın:

   ```bash
   git clone https://github.com/<KULLANICI_ADINIZ>/MyAssistant.git
   cd MyAssistant
   ```

3. Geliştirme için yeni bir dal (branch) oluşturun:

   ```bash
   git checkout -b ozellik/yeni-ozellik-adi
   ```

## Geliştirme Ortamı

- **IDE**: Visual Studio 2019 veya üzeri
- **.NET Framework**: 4.5
- **Google Chrome**: Yüklü olmalıdır (Selenium ChromeDriver için)
- **NuGet Paketleri**: `nuget restore MyAssistant.sln` komutuyla geri yükleyin

## Kod Standartları

### Genel Kurallar

- **Değişken, sınıf ve fonksiyon isimleri** İngilizce veya mevcut Türkçe isimler korunmalıdır.
- **Yorum satırları ve kod içi açıklamalar** Türkçe yazılmalıdır.
- XML belgelendirme yorumları (`/// <summary>`) tüm public metotlara eklenmelidir.
- Kullanılmayan `using` ifadelerini kaldırın.

### Güvenlik Kuralları

- Koda **hardcoded şifre, API anahtarı veya yerel dosya yolu** eklemeyin.
- Hassas bilgiler `Properties.Settings` veya ortam değişkenleri üzerinden yönetilmelidir.
- Pull request göndermeden önce kodunuzu hassas bilgiler açısından kontrol edin.

### Commit Mesajları

Commit mesajlarını Türkçe ve açıklayıcı şekilde yazın:

```
✅ İyi: "Oturum kontrol aralığı yapılandırılabilir hale getirildi"
✅ İyi: "Log dosyası yazma hatası düzeltildi"
❌ Kötü: "düzeltme"
❌ Kötü: "güncelleme"
```

## Katkı Süreci

1. **Issue Oluşturun**: Büyük değişiklikler için önce bir issue açarak tartışma başlatın.
2. **Kodunuzu Yazın**: Yukarıdaki standartlara uygun olarak geliştirme yapın.
3. **Test Edin**: Değişikliklerinizin mevcut işlevselliği bozmadığından emin olun.
4. **Commit Edin**: Anlamlı commit mesajlarıyla değişikliklerinizi kaydedin.
5. **Pull Request Açın**: Açıklayıcı bir başlık ve detaylı bir açıklama ile PR oluşturun.

## Pull Request Kontrol Listesi

- [ ] Kod, mevcut standartlara uygun mu?
- [ ] Yorum satırları Türkçe mi?
- [ ] Hardcoded hassas bilgi içermiyor mu?
- [ ] Mevcut özellikler bozulmamış mı?
- [ ] Commit mesajları açıklayıcı mı?

## Hata Bildirimi

Bir hata bulduysanız lütfen şu bilgilerle birlikte issue açın:

1. **Hata açıklaması**: Ne oldu?
2. **Beklenen davranış**: Ne olması gerekiyordu?
3. **Adımlar**: Hatayı nasıl tekrarlayabiliriz?
4. **Ortam bilgisi**: İşletim sistemi, Chrome sürümü, .NET sürümü

## Özellik Talebi

Yeni bir özellik önermek istiyorsanız, issue açarak şunları belirtin:

1. Özelliğin amacı ve kullanıcıya sağlayacağı fayda
2. Olası teknik yaklaşım (varsa)
3. Ekran tasarım önerisi (varsa)

## Lisans

Bu projeye katkıda bulunarak, katkılarınızın [MIT Lisansı](LICENSE) kapsamında lisanslanacağını kabul etmiş olursunuz.

---

Sorularınız için issue açmaktan çekinmeyin. Katkılarınız için teşekkür ederiz! 🙏
