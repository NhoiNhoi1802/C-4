using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Ếch_ăn_chay.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(200, ErrorMessage = "Tên sản phẩm không vượt quá 200 ký tự")]
        public string ProductName { get; set; } = default!;

        [Range(0, double.MaxValue, ErrorMessage = "Giá phải lớn hơn hoặc bằng 0")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Số lượng không được để trống")]
        [StringLength(100, ErrorMessage = "Số lượng không vượt quá 100 ký tự")]
        public string Stock { get; set; } = default!;

        public string Unit { get; set; } = string.Empty; // Đơn vị tính (ví dụ: "kg", "trái", "hộp")

        public string? ImageUrl { get; set; }

        // FK: Category

         public int CategoryId { get; set; }
        [ValidateNever] public Category? Category { get; set; }
    }
}
