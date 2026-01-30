# Mimari

Bu dokuman, projenin teknik mimarisini ve tasarim kararlarini aciklar.

## Genel Bakis

Proje, sorumluluk ayrimi prensibine dayanan katmanli bir mimari kullanir. Her katman belirli bir gorevi ustlenir ve sadece altindaki katmana bagimlidir.

```
┌─────────────────────────────────────┐
│          ShopApp.WebUI              │  Sunum Katmani
│   (Controllers, Views, Models)      │
└─────────────────────────────────────┘
                 │
                 ▼
┌─────────────────────────────────────┐
│         ShopApp.Business            │  Is Mantigi Katmani
│    (Servisler, Validasyon)          │
└─────────────────────────────────────┘
                 │
                 ▼
┌─────────────────────────────────────┐
│        ShopApp.DataAccess           │  Veri Erisim Katmani
│   (Repository, DbContext)           │
└─────────────────────────────────────┘
                 │
                 ▼
┌─────────────────────────────────────┐
│         ShopApp.Entities            │  Domain Katmani
│      (Entity siniflari)             │
└─────────────────────────────────────┘
```

## Katmanlar

### ShopApp.Entities

Domain modellerini icerir. Herhangi bir framework'e bagimli degildir.

Temel entity'ler:
- `Product` - Urun bilgileri
- `Category` - Kategori bilgileri
- `Cart`, `CartItem` - Sepet yapisi
- `Order`, `OrderItem` - Siparis yapisi

### ShopApp.DataAccess

Entity Framework Core kullanarak veritabani islemlerini yonetir.

- `ShopContext` - DbContext sinifi
- `EfCoreGenericRepository<T>` - Generic repository implementasyonu
- `EfCoreProductDal`, `EfCoreCategoryDal` vb. - Entity-specific repository'ler

Repository pattern kullanilarak veri erisim mantigi soyutlanmistir. Bu sayede:
- Unit test yazmak kolaylasir
- Veritabani degisiklikleri tek noktadan yonetilir
- Controller'lar dogrudan DbContext'e bagimli olmaz

### ShopApp.Business

Is kurallarini ve servis mantigi icerir.

- `IProductService`, `ICategoryService` vb. - Servis arayuzleri
- `ProductManager`, `CategoryManager` vb. - Servis implementasyonlari

Servisler, repository'leri constructor injection ile alir.

### ShopApp.WebUI

Kullanici arayuzu ve HTTP request yonetimi.

- `Controllers/` - MVC controller'lari
- `Views/` - Razor view'lari
- `Models/` - ViewModel'ler
- `Identity/` - Kimlik dogrulama yapisi

## Veri Akisi

Tipik bir istek akisi:

```
HTTP Request
    │
    ▼
Controller (ShopApp.WebUI)
    │ IProductService.GetProductById(id)
    ▼
ProductManager (ShopApp.Business)
    │ _productDal.GetById(id)
    ▼
EfCoreProductDal (ShopApp.DataAccess)
    │ _context.Products.Find(id)
    ▼
ShopContext (EF Core)
    │
    ▼
SQLite Database
```

## Veritabani Yapisi

Iki ayri DbContext kullanilir:

1. `ShopContext` - Urun, kategori, siparis verileri
2. `ApplicationIdentityDbContext` - Kullanici ve rol verileri

Bu ayrim, kimlik verilerinin uygulama verilerinden bagimsiz yonetilmesini saglar.

### Entity Iliskileri

```
Product ◄──── ProductCategory ────► Category
    │
    ▼
CartItem ────► Cart ────► User
    │
    ▼
OrderItem ────► Order ────► User
```

## Guvenlik

- Parolalar ASP.NET Core Identity tarafindan hash'lenir
- CSRF koruması varsayılan olarak aktiftir
- Hassas veriler (API anahtarlari) User Secrets ile yonetilir

## Caching Stratejisi

Uygulama iki cache implementasyonu destekler:

1. **In-Memory Cache** - Gelistirme ortami icin varsayilan
2. **Redis** - Uretim ortami icin distributed cache

Cache kullanimi:
- Urun listesi: 5 dakika TTL
- Kategori listesi: 10 dakika TTL
- Cache invalidation: CRUD islemlerinde otomatik temizleme

## Docker

Multi-stage Dockerfile ile optimize edilmis container:

```
Build Stage (SDK) -> Publish Stage -> Runtime Stage (ASP.NET)
```

docker-compose.yml ile tum servisler (app, redis) bir arada calistirilabilir.

## Test Stratejisi

- Unit testler: xUnit + Moq
- Servis katmani testleri
- Validation testleri
- Repository mock'lama ile izole testler

## Gelecek Iyilestirmeler

- Integration test coverage
- API katmani (Web API)
- Kubernetes deployment

