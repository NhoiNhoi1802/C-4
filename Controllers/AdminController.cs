using Ếch_ăn_chay.Data;
using Ếch_ăn_chay.Models;
using Ếch_ăn_chay.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ếch_ăn_chay.Controllers
{   

    public class AdminController : Controller
    {
        private readonly AppDBContext _db;

        public AdminController(AppDBContext db) => _db = db;

        // Dashboard tổng quan
        public async Task<IActionResult> Dashboard()
        {
            var model = new AdminDashboardViewModel
            {
                TotalProducts = await _db.Products.CountAsync(),
                TotalCategories = await _db.Categories.CountAsync(),
                TotalOrders = await _db.Orders.CountAsync(),
                TotalUsers = await _db.Users.CountAsync()
            };
            return View(model);
        }

        #region Products
        public async Task<IActionResult> Products()
        {
            var products = await _db.Products.Include(p => p.Category).ToListAsync();
            return View(products);
        }

        public IActionResult CreateProduct() => View();

        // GET: /Admin/ProductCreate
        public IActionResult ProductCreate()
        {
            var vm = new ProductViewModel
            {
                Categories = _db.Categories.ToList() ?? new List<Category>()
            };
            return View(vm);
        }

        // POST: /Admin/ProductCreate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductCreate(ProductViewModel vm)
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
            return RedirectToAction("Dashboard");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(Product model)
        {
            if (!ModelState.IsValid) return View(model);

            _db.Products.Update(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Products));
        }

        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost, ActionName("DeleteProduct")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProductConfirmed(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product != null)
            {
                _db.Products.Remove(product);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Products));
        }
        #endregion

        #region Categories
        public async Task<IActionResult> Categories()
        {
            var categories = await _db.Categories.ToListAsync();
            return View(categories);
        }

        public IActionResult CreateCategory() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(Category model)
        {
            if (!ModelState.IsValid) return View(model);

            _db.Categories.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Categories));
        }

        public async Task<IActionResult> EditCategory(int id)
        {
            var category = await _db.Categories.FindAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(Category model)
        {
            if (!ModelState.IsValid) return View(model);

            _db.Categories.Update(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Categories));
        }

        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _db.Categories.FindAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }

        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategoryConfirmed(int id)
        {
            var category = await _db.Categories.FindAsync(id);
            if (category != null)
            {
                _db.Categories.Remove(category);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Categories));
        }
        #endregion

        #region Orders
        public async Task<IActionResult> Orders()
        {
            var orders = await _db.Orders.Include(o => o.User).ToListAsync();
            return View(orders);
        }

        public async Task<IActionResult> DetailsOrder(int id)
        {
            var order = await _db.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null) return NotFound();
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, string status)
        {
            var order = await _db.Orders.FindAsync(orderId);
            if (order == null) return NotFound();

            order.Status = status;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Orders));
        }
        #endregion

        #region Users
        public async Task<IActionResult> Users()
        {
            var users = await _db.Users.ToListAsync();
            return View(users);
        }
        #endregion
    }
}
