# Araç Kiralama

ASP.NET Core **Blazor Server** ile geliştirilmiş araç kiralama yönetim uygulaması. Filo, müşteri, rezervasyon, kiralama, faturalama ve uyumluluk süreçlerini tek panelden yönetir.

## Özellikler

- **Filo yönetimi:** Şubeler, araç kategorileri, araçlar, bakım, sigorta, muayene, hasar kayıtları
- **Müşteri & rezervasyon:** Ön rezervasyon, ek ürün satırları, kampanya desteği
- **Kiralama:** Rezervasyondan sözleşmeye dönüşüm, PDF sözleşme, kiralama durumları
- **Faturalama:** Fatura kesimi, ödeme takibi, KDV hesabı
- **Kimlik & roller:** ASP.NET Identity — Admin, Operator, ReadOnly, Customer
- **Demo veri:** İsteğe bağlı örnek şube, araç ve müşteri seed’i

## Teknolojiler

| Katman | Teknoloji |
|--------|-----------|
| UI | Blazor Server (.NET 9) |
| Veri | Entity Framework Core 9, SQL Server |
| Kimlik | ASP.NET Core Identity |
| PDF | QuestPDF |

## Proje yapısı

```
AracKiralama.sln
├── AracKiralama.Web/     # Blazor arayüz + uygulama mantığı
└── AracKiralama.Data/    # Entity'ler, DbContext, migration'lar
```

## Gereksinimler

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- SQL Server (ör. `.\SQLEXPRESS04`)
- Veritabanı adı: **`Araç KiralamaDB`**

## Kurulum

1. Repoyu klonlayın:

```bash
git clone https://github.com/EmreEmir-17/Arac_Kiralama.git
cd Arac_Kiralama
```

2. Ayar dosyalarını oluşturun:

```bash
copy AracKiralama.Web\appsettings.example.json AracKiralama.Web\appsettings.json
copy AracKiralama.Web\appsettings.Development.example.json AracKiralama.Web\appsettings.Development.json
```

3. `appsettings.json` içindeki `DefaultConnection` değerini kendi SQL Server instance’ınıza göre düzenleyin.

4. Veritabanını oluşturun ve migration’ları uygulayın:

```bash
dotnet ef database update --project AracKiralama.Data --startup-project AracKiralama.Web
```

5. Uygulamayı çalıştırın:

```bash
dotnet run --project AracKiralama.Web
```

Tarayıcı: `https://localhost:7093` veya `http://localhost:5191`

## İlk giriş (seed)

Uygulama ilk açılışta rolleri ve admin kullanıcısını oluşturur. `appsettings.json` içindeki `Seed` bölümünden yapılandırılır:

| Alan | Varsayılan |
|------|------------|
| AdminEmail | `admin@local` |
| AdminPassword | Kendi şifreniz |
| DemoData | `false` (Development’ta `true` yapılabilir) |

## Roller

| Rol | Yetki |
|-----|-------|
| Admin | Tam yönetim |
| Operator | Yazma işlemleri |
| ReadOnly | Salt okunur |
| Customer | Müşteri portalı (rezervasyon, profil) |

## Veritabanı tabloları (özet)

- **Organizasyon:** `Branches`, `VehicleCategories`, `Vehicles`
- **Müşteri:** `Customers` (+ `AspNetUsers` bağlantısı)
- **İş akışı:** `Reservations`, `Rentals`, `Invoices`, `Payments`
- **Ek:** `Campaigns`, `ExtraProducts`, `SeasonalPriceRules`
- **Uyumluluk:** `DamageReports`, `VehicleInsurances`, `VehicleInspections`, `MaintenanceRecords`

## Yayınlama (AWS / sunucu)

```bash
dotnet publish AracKiralama.Web -c Release -o ./publish
```

Publish çıktısını sunucuya kopyalayın; sunucuda `.NET 9 Hosting Bundle` kurulu olmalıdır. Canlı ortamda `appsettings.json` içinde production connection string kullanın.

## Lisans

Bu proje eğitim / bitirme projesi kapsamındadır.
