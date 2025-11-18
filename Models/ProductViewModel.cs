using Microsoft.AspNetCore.Http;
using Ếch_ăn_chay.Models;

namespace Ếch_ăn_chay.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; } = new Product();
        public List<Category> Categories { get; set; } = new List<Category>();
        public IFormFile? ImageFile { get; set; } // file upload
    }
}
