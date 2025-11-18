using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ếch_ăn_chay.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public string UserId { get; set; } = default!;  // ✅ sửa int → string

        [Required, StringLength(255)]
        public string ShippingAddress { get; set; } = default!;

        [Required, StringLength(20)]
        public string PhoneNumber { get; set; } = default!;

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public decimal TotalAmount { get; set; }

        [Required]
        public string Status { get; set; } = "Pending";

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } = default!;

        public virtual List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    }
}
