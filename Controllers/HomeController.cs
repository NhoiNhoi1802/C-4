using Ếch_ăn_chay.Data;
using Ếch_ăn_chay.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ếch_ăn_chay.Data;
using Ếch_ăn_chay.Models;

namespace Ếch_ăn_chay.Controllers
{
    public class HomeController : Controller
    {

        private readonly AppDBContext _db;
        public HomeController(AppDBContext db) => _db = db;

            public async Task<IActionResult> Index()
            {
                var products = await _db.Products.Include(p => p.Category).Take(12).ToListAsync();
                return View(products);
            }
    }
}
