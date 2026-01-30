# EcoMarket E-Ticaret

Katmanli mimari ile gelistirilmis, ASP.NET Core MVC tabanli bir e-ticaret uygulamasi.

## Proje Hakkinda

Bu proje, kullanicilarin urun arama, sepete ekleme ve siparis verebilmesini saglayan bir online market uygulamasidir. Admin paneli uzerinden urun ve kategori yonetimi yapilabilir.

### Temel Ozellikler

- Urun listeleme ve kategori bazli filtreleme
- Kullanici kayit ve giris islemleri
- Sepet yonetimi
- Siparis olusturma ve takibi
- Admin paneli (urun/kategori CRUD islemleri)
- Iyzico odeme entegrasyonu
- Redis ile distributed caching
- Docker ile container destegi

## Teknoloji Stack

| Katman | Teknoloji |
|--------|-----------|
| Framework | .NET 8, ASP.NET Core MVC |
| ORM | Entity Framework Core 8 |
| Veritabani | SQLite |
| Cache | Redis / In-Memory |
| Kimlik Dogrulama | ASP.NET Core Identity |
| Frontend | Bootstrap 5, jQuery |
| Odeme | Iyzico API |
| Container | Docker |
| CI/CD | GitHub Actions |

## Proje Yapisi

```
ShopApp.sln
├── ShopApp.Entities        # Domain modelleri
├── ShopApp.DataAccess      # Repository katmani, EF Core
├── ShopApp.Business        # Is mantigi, servisler
├── ShopApp.WebUI           # MVC Controllers, Views
└── ShopApp.Tests           # Unit testler (xUnit, Moq)
```

Mimari detaylar icin [ARCHITECTURE.md](ARCHITECTURE.md) dosyasina bakiniz.

## Kurulum

### Gereksinimler

- .NET 8 SDK
- Docker (istege bagli)

### Yerel Calistirma

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

### Docker ile Calistirma

```bash
docker-compose up -d
```

Uygulama `http://localhost:5000` adresinde calisacaktir.

## Testler

Unit testleri calistirmak icin:

```bash
dotnet test
```

## Yapilandirma

Proje varsayilan olarak SQLite ve in-memory cache kullanir. Redis kullanmak icin `appsettings.json` dosyasina asagidaki ayari ekleyin:

```json
{
  "Redis": {
    "ConnectionString": "localhost:6379"
  }
}
```

Odeme ve e-posta servisleri icin User Secrets kullanin:

```bash
dotnet user-secrets set "Iyzico:ApiKey" "your-api-key" --project ShopApp.WebUI
dotnet user-secrets set "SendGrid:ApiKey" "your-api-key" --project ShopApp.WebUI
```

## CI/CD

GitHub Actions ile her push'ta:
- Build kontrolu
- Unit test calistirma
- Docker image olusturma

## Lisans

MIT
