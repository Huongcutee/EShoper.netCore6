using System.ComponentModel.DataAnnotations;

namespace ShopThoiTrang.Models
{
    public class ShippingModel
    {
        [Key]
        public int Id { get; set; } 
        [Required]
        public string Province { get; set; }
        [Required]
        public string District { get; set; }
        [Required]
        public string Ward { get; set; }
        [Required]
        public decimal Price { get; set; }

    }
}
