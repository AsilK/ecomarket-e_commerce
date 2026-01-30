# EcoMarket E-Ticaret

Katmanlı mimari ile gelistirilmis, ASP.NET Core MVC tabanli bir e-ticaret uygulamasi.

## Proje Hakkinda

Bu proje, kullanicilarin urun arama, sepete ekleme ve siparis verebilmesini saglayan bir online market uygulamasidir. Admin paneli uzerinden urun ve kategori yonetimi yapilabilir.

### Temel Ozellikler

- Urun listeleme ve kategori bazli filtreleme
- Kullanici kayit ve giris islemleri
- Sepet yonetimi
- Siparis olusturma ve takibi
- Admin paneli (urun/kategori CRUD islemleri)
- Iyzico odeme entegrasyonu

## Teknoloji Stack

| Katman | Teknoloji |
|--------|-----------|
| Framework | .NET 8, ASP.NET Core MVC |
| ORM | Entity Framework Core 8 |
| Veritabani | SQLite |
| Kimlik Dogrulama | ASP.NET Core Identity |
| Frontend | Bootstrap 5, jQuery |
| Odeme | Iyzico API |

## Proje Yapisi

```
ShopApp.sln
├── ShopApp.Entities        # Domain modelleri
├── ShopApp.DataAccess      # Repository katmani, EF Core
├── ShopApp.Business        # Is mantigi, servisler
└── ShopApp.WebUI           # MVC Controllers, Views
```

Mimari detaylar icin [ARCHITECTURE.md](ARCHITECTURE.md) dosyasina bakiniz.

## Kurulum

### Gereksinimler

- .NET 8 SDK

### Adimlar

1. Projeyi klonlayin:
```bash
git clone https://github.com/AsilK/E-ticaret.git
cd E-ticaret
```

2. Veritabanini olusturun:
```bash
dotnet ef database update --project ShopApp.WebUI --context ApplicationIdentityDbContext
dotnet ef database update --project ShopApp.DataAccess --startup-project ShopApp.WebUI --context ShopContext
```

3. Uygulamayi calistirin:
```bash
cd ShopApp.WebUI
dotnet run
```

Uygulama `http://localhost:5000` adresinde calisacaktir.

## Yapilandirma

Proje varsayilan olarak SQLite kullanir. Veritabani dosyalari (`ShopAppIdentity.db`, `ShopAppData.db`) otomatik olusturulur.

Odeme ve e-posta servisleri icin `appsettings.json` dosyasindaki API anahtarlarini guncellemeniz gerekir. Gelistirme ortaminda User Secrets kullanmaniz onerilir:

```bash
dotnet user-secrets set "Iyzico:ApiKey" "your-api-key" --project ShopApp.WebUI
dotnet user-secrets set "SendGrid:ApiKey" "your-api-key" --project ShopApp.WebUI
```

## Lisans

MIT
