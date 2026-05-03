using System.ComponentModel.DataAnnotations;

namespace KargoSistemi.Models
{
    public class Musteri
    {
        [Key]
        public int Customer_ID { get; set; } // Diyagramdaki isim

        [Required(ErrorMessage = "Ad alanı zorunludur.")]
        public string Customer_name { get; set; } = "";

        [Required(ErrorMessage = "Soyad alanı zorunludur.")]
        public string Customer_lastname { get; set; } = "";

        // Adres Bilgileri (Diyagramda var)
        public string City { get; set; } = "";
        public string Town { get; set; } = "";
        public string Neighbourhood { get; set; } = "";
        public string Street { get; set; } = "";

        // Telefon Kontrolü (Harf girmeyi engellemek için Regex)
        [Required(ErrorMessage = "Telefon numarası zorunludur.")]
        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Telefon sadece rakamlardan oluşmalı ve 10-11 haneli olmalıdır.")]
        public string Phone_number { get; set; } = ""; 
    }
}