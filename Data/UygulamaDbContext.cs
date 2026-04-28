using Microsoft.EntityFrameworkCore;
using KargoSistemi.Models;

namespace KargoSistemi.Data
{
    // Projemiz ile SQL Server veritabanı arasındaki ana köprü sınıfımız
    public class UygulamaDbContext : DbContext
    {
        // Program.cs dosyasındaki veritabanı bağlantı ayarlarını (ConnectionString) alıp hazırlayan yapıcı metot
        public UygulamaDbContext(DbContextOptions<UygulamaDbContext> options) : base(options) 
        { 
        }

        // SQL veritabanımızdaki 'Personeller' tablosunu C# tarafında temsil eden liste
        public DbSet<Personel> Personeller { get; set; }
        
        // İleride Şubeler veya Kargolar tablosu eklersek onların DbSet'lerini de buraya alt alta yazacağız
    }
}