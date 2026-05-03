using System;
using System.ComponentModel.DataAnnotations;

namespace KargoSistemi.Models
{
    public class Kargo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string TakipNo { get; set; } = "";

        public int GondericiId { get; set; }

        
        public int AliciId { get; set; }

        public double Agirlik { get; set; }

        public string Boyut { get; set; } = "";


        public string Icerik { get; set; } = "";


        public string Durum { get; set; } = "";


        public DateTime KayitTarihi { get; set; }
    }
}