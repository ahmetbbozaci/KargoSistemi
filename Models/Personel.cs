using System.ComponentModel.DataAnnotations;

namespace KargoSistemi.Models
{
    public class Personel
    {
        [Key] 
        public int Id { get; set; }

        [Required] 
        public string KullaniciAdi { get; set; }

        [Required]
        public string SifreHash { get; set; } 

        public string Rol { get; set; } 
    }
}