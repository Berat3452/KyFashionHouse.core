
using System.ComponentModel.DataAnnotations;

namespace KyFashionHouse.core.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public decimal TotalAmount { get; set; }

        [Required]
        [StringLength(30)]
        public string Status { get; set; } = "Beklemede";

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string ShippingAddress { get; set; } = string.Empty;

        public List<OrderItem> OrderItems { get; set; }
            = new List<OrderItem>();
    }
}