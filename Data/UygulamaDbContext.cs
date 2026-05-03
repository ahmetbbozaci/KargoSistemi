using Microsoft.EntityFrameworkCore;
using KargoSistemi.Models;

namespace KargoSistemi.Data
{
    public class UygulamaDbContext : DbContext
    {
        public UygulamaDbContext(DbContextOptions<UygulamaDbContext> options) : base(options) 
        { 
        }

        // Sistem girişlerini yapan çalışanlar
        public DbSet<Personel> Personeller { get; set; }
        
        // Kargo gönderim ve takip bilgileri
        public DbSet<Kargo> Kargolar { get; set; } 

        // YENİ: Kargo gönderen ve alan kişiler
        // Bu satırı eklediğinde "Musteri"nin altı kırmızı çizilirse 2. adımı yapmalısın.
        public DbSet<Musteri> Musteriler { get; set; } 
    }
}