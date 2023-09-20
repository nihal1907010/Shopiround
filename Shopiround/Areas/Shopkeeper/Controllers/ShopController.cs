using Microsoft.AspNetCore.Mvc;

namespace Shopiround.Areas.Shopkeeper.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
    }
}
