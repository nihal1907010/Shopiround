using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Shopiround.DataAccess.Repository;
using Shopiround.DataAccess.Repository.IRepository;
using Shopiround.Models.Models;

namespace Shopiround.Areas.Shopkeeper.Controllers
{
    [Area("Shopkeeper")]
    public class ProductController : Controller
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            this.unitOfWork = unitOfWork;
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> products = unitOfWork.ProductRepository.GetAll().ToList();
            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product, IFormFile? file)
        {

            if (file != null)
            {
                string wwwRootPath = webHostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string productPath = Path.Combine(wwwRootPath, @"images\product");
                using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                };
                product.ImageURL = @"\images\product\" + fileName;
            }
            else
            {
                product.ImageURL = @"\images\shop\default_shop.png";
            }
            product.ShopId = 1;
            unitOfWork.ProductRepository.Add(product);
            unitOfWork.Save();


            return RedirectToAction("Index");
        }

        public IActionResult Show(int? Id)
        {
            if(Id != null)
            {
                Product product = unitOfWork.ProductRepository.Get(p=> p.Id == Id, includeProperties: "Shop");
                return View(product);
            }
            else
            {
                return NotFound();
            }

            

        }


    }
}
