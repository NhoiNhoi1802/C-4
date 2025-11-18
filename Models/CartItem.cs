using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ếch_ăn_chay.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }

        [Required]
        public string UserId { get; set; } = default!;  // ✅ sửa từ int → string

        [Required]
        public int ProductId { get; set; }

        [Range(1, 999)]
        public int Quantity { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } = default!;

        [ForeignKey("ProductId")]
        public Product Product { get; set; } = default!;
    }
}
