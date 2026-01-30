# Teknolojiler

Bu dokuman, projede kullanilan teknolojileri ve secim nedenlerini aciklar.

## Backend

### .NET 8 (LTS)

.NET 8, Microsoft'un uzun sureli destek (LTS) versiyonudur. Kasim 2026'ya kadar destek almaya devam edecektir.

Secim nedenleri:
- Performans iyilestirmeleri
- Native AOT destegi
- Minimal API gelistirmeleri

### ASP.NET Core MVC

Model-View-Controller pattern'i uygulayan web framework'u.

- Razor view engine ile sunucu tarafli HTML render
- Tag helpers ile temiz view syntax'i
- Model binding ve validation destegi

### Entity Framework Core 8

Microsoft'un ORM (Object-Relational Mapping) cozumu.

- Code-first yaklasimi
- LINQ destegi
- Migration sistemi
- Lazy/eager loading secenekleri

## Veritabani

### SQLite

Hafif, dosya tabanli iliskisel veritabani.

Secim nedenleri:
- Kurulum gerektirmez
- Gelistirme ortami icin ideal
- Tasinabilirlik

Uretim ortami icin SQL Server veya PostgreSQL onerilir.

## Kimlik Dogrulama

### ASP.NET Core Identity

Kullanici yonetimi icin hazir cozum.

Sagladiklari:
- Kullanici kayit ve giris
- Parola hash'leme (PBKDF2)
- Rol tabanli yetkilendirme
- E-posta dogrulama
- İki faktorlu dogrulama destegi

## Frontend

### Bootstrap 5

CSS framework'u.

- Responsive grid sistemi
- Hazir UI componentleri
- JavaScript eklentileri (modal, dropdown vb.)

### jQuery

DOM manipulasyonu ve AJAX islemleri icin.

- Validation eklentileri ile form dogrulama
- Unobtrusive validation destegi

## Harici Servisler

### Iyzico

Turkiye'de yaygin kullanilan odeme altyapisi.

- Sandbox ortami ile test imkani
- 3D Secure destegi

### SendGrid

E-posta gonderim servisi.

- Transactional email destegi
- API tabanli entegrasyon

## Gelistirme Araclari

### EF Core Tools

Veritabani migration yonetimi.

```bash
dotnet ef migrations add MigrationName
dotnet ef database update
```

### User Secrets

Gelistirme ortaminda hassas verilerin yonetimi.

```bash
dotnet user-secrets init
dotnet user-secrets set "Key" "Value"
```

Bu yontem, API anahtarlarinin kaynak koduna dahil edilmesini onler.
