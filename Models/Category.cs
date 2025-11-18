using System.ComponentModel.DataAnnotations;

namespace Ếch_ăn_chay.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required, StringLength(150)]
        public string CategoryName { get; set; } = default!;

        public string? Description { get; set; }

        public  ICollection<Product>? Products { get; set; }
    }
}
