using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopiround.DataAccess.Data;
using Newtonsoft.Json;
using Shopiround.DataAccess.Data;
using Shopiround.DataAccess.Repository.IRepository;
using Shopiround.Models.Models;
using System.Diagnostics;

namespace Shopiround.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {

        private readonly IUnitOfWork unitOfWork;

        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, ApplicationDbContext context)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public IActionResult Index()
        {

            List<Product> products = unitOfWork.ProductRepository.GetAll().ToList();
            return View(products);
        }


        [HttpGet]
        public IActionResult Search(string name)
        {
            Console.WriteLine("G");

            if(name!=null)
            {
                List<Product> searchedProducts =  unitOfWork.ProductRepository.GetAll().Where(j => j.Name.ToLower().Contains(name.ToLower())).ToList() ;
                return Json(new { searchedProducts });
                            }
            return View("Search");
        }

        public IActionResult Privacy()
        {
            int Id = 1;
            if (Id != null)
            {
                Product product = _unitOfWork.ProductRepository.Get(p => p.Id == Id, includeProperties: "Shop,Reviews");
                return View(product);
            }
            else
            {
                return NotFound();
            }
            return View();
        }
        public IActionResult ViewCart()
        {
            
            ApplicationUser user = _unitOfWork.ApplicationUserRepository.Get(u => u.UserName == User.Identity.Name, includeProperties: "Shop,CartItems");
            List<CartItem> cartItems = _context.CartItems.Include(c => c.Product).ThenInclude(s => s.Shop).Where(c => c.UserId == user.Id).ToList();
            return View(cartItems);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
