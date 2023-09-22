using Microsoft.AspNetCore.Mvc;
using Shopiround.DataAccess.Repository.IRepository;
using Shopiround.Models.Models;

namespace Shopiround.Areas.Shopkeeper.Controllers
{
    [Area("Shopkeeper")]
    public class ShopController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public readonly IWebHostEnvironment _webHostEnvironment;
        public ShopController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Shop> shops = _unitOfWork.ShopRepository.GetAll().ToList();
            return View(shops);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Shop shop, IFormFile? file)
        {
            if (file != null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string shopPath = Path.Combine(wwwRootPath, @"images\shop");
                using (var fileStream = new FileStream(Path.Combine(shopPath, fileName), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                };
                shop.ImageURL = @"\images\shop\" + fileName;
            }
            else
            {
                shop.ImageURL = @"\images\shop\default_shop.png";
            }
            _unitOfWork.ShopRepository.Add(shop);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }
    }
}
