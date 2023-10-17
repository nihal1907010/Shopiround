using Microsoft.AspNetCore.Mvc;
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

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            this.unitOfWork = unitOfWork;
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
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
