using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ếch_ăn_chay.Data;
using Ếch_ăn_chay.Models;
using Ếch_ăn_chay.ViewModels;

namespace Ếch_ăn_chay.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDBContext _db;
        public ProductsController(AppDBContext db) => _db = db;

        // GET: Index
        public async Task<IActionResult> Index(string? searchName, decimal? minPrice, decimal? maxPrice)
        {
            var products = _db.Products.Include(p => p.Category).AsQueryable();

            if (!string.IsNullOrEmpty(searchName))
                products = products.Where(p => p.ProductName.Contains(searchName));

            if (minPrice.HasValue)
                products = products.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                products = products.Where(p => p.Price <= maxPrice.Value);

            return View(await products.ToListAsync());
        }

        // GET: Create
        public IActionResult Create()
        {
            var vm = new ProductViewModel
            {
                Categories = _db.Categories.ToList() ?? new List<Category>()
            };
            return View(vm);
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel vm)
        {
            if (!ModelState.IsValid || vm.Product.CategoryId == 0)
            {
                if (vm.Product.CategoryId == 0)
                    ModelState.AddModelError("Product.CategoryId", "Bạn phải chọn danh mục");

                vm.Categories = _db.Categories.ToList() ?? new List<Category>();
                return View(vm);
            }

            _db.Products.Add(vm.Product);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Edit
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();

            var vm = new ProductViewModel
            {
                Product = product,
                Categories = _db.Categories.ToList() ?? new List<Category>()
            };
            return View(vm);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel vm)
        {
            if (!ModelState.IsValid || vm.Product.CategoryId == 0)
            {
                if (vm.Product.CategoryId == 0)
                    ModelState.AddModelError("Product.CategoryId", "Bạn phải chọn danh mục");

                vm.Categories = _db.Categories.ToList() ?? new List<Category>();
                return View(vm);
            }

            _db.Products.Update(vm.Product);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Delete
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _db.Products.Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null) return NotFound();
            return View(product);
        }

        // POST: Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product != null)
            {
                _db.Products.Remove(product);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Details
        public async Task<IActionResult> Details(int id)
        {
            var product = await _db.Products.Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null) return NotFound();
            return View(product);
        }
    }
}
