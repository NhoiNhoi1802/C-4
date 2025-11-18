using Ếch_ăn_chay.Data;
using Ếch_ăn_chay.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ếch_ăn_chay.Controllers
{
    public class OrdersController : Controller
    {
        private readonly AppDBContext _db;
        public OrdersController(AppDBContext db) => _db = db;

        // GET: Orders/Create
        public IActionResult Create()
        {
            var order = new Order
            {
                OrderDetails = new List<OrderDetail> { new OrderDetail() } // tạo 1 chi tiết mặc định
            };
            return View(order);
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order)
        {
            if (!ModelState.IsValid) return View(order);

            // Tính tổng tiền
            order.TotalAmount = order.OrderDetails.Sum(d => d.Quantity * d.UnitPrice);

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index)); // hoặc hiện thông báo thành công
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var orders = await _db.Orders.Include(o => o.OrderDetails)
                                         .ThenInclude(od => od.Product)
                                         .ToListAsync();
            return View(orders);
        }
    }
}
