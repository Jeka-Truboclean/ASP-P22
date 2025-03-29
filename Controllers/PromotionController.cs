using ASP_P22.Data;
using ASP_P22.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP_P22.Controllers
{
    public class PromotionController : Controller
    {
        private readonly DataContext _context;

        public PromotionController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var promotions = await _context.Promotions.ToListAsync();
            return View(promotions);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Promotion promotion)
        {
            if (ModelState.IsValid)
            {
                _context.Promotions.Add(promotion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(promotion);
        }
    }
}
