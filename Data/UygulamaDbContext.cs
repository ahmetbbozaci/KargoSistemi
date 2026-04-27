using Microsoft.EntityFrameworkCore;
using KargoSistemi.Models;

namespace KargoSistemi.Data
{
    public class UygulamaDbContext : DbContext
    {
        public UygulamaDbContext(DbContextOptions<UygulamaDbContext> options) : base(options) { }

        public DbSet<Personel> Personeller { get; set; }
    }
}