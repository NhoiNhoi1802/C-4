using Ếch_ăn_chay.Data;
using Ếch_ăn_chay.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Ếch_ăn_chay.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDBContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(AppDBContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // Hiển thị giỏ hàng
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Auth");

            var items = await _db.CartItems
                                 .Include(c => c.Product)
                                 .Where(c => c.UserId == user.Id)
                                 .ToListAsync();

            return View(items);
        }

        // Thêm vào giỏ
        [HttpPost]
        public async Task<IActionResult> Add(int productId, int quantity = 1)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Auth");

            var cartItem = await _db.CartItems
                                    .FirstOrDefaultAsync(c => c.UserId == user.Id && c.ProductId == productId);

            if (cartItem != null)
                cartItem.Quantity += quantity;
            else
                _db.CartItems.Add(new CartItem
                {
                    ProductId = productId,
                    UserId = user.Id,
                    Quantity = quantity
                });

            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // Xóa sản phẩm khỏi giỏ
        [HttpPost]
        public async Task<IActionResult> Remove(int cartItemId)
        {
            var item = await _db.CartItems.FindAsync(cartItemId);
            if (item != null) _db.CartItems.Remove(item);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
