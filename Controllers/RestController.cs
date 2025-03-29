using Microsoft.AspNetCore.Mvc;

namespace ASP_P22.Controllers
{
    public class RestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
