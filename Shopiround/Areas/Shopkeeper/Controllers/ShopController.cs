using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopiround.Repository.IRepository;
using Shopiround.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Shopiround.Data;
using Shopiround.Repository;
using Shopiround.Models.Statistics;
using Microsoft.EntityFrameworkCore;

namespace Shopiround.Areas.Shopkeeper.Controllers
{
    [Area("Shopkeeper")]
    public class ShopController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public readonly IWebHostEnvironment _webHostEnvironment;
        public ApplicationDbContext context;
        public ShopController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, ApplicationDbContext context)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            this.context = context;
        }
        public IActionResult Index()
        {
            var topProductCounts = context.ProductCounts
            .OrderByDescending(pc => pc.Count)
            .Take(5).Include("Product")
            .ToList();

          /*  List<Product> topProducts = new List<Product>();

            foreach (var productCount in topProductCounts)
            {
                // Assuming ProductId in ProductCount corresponds to Id in the Product table
                Product product = _unitOfWork.ProductRepository.Get(p => p.Id == productCount.ProductId, includeProperties: "Shop,Reviews");

                if (product != null)
                {
                    topProducts.Add(product);
                }
            }*/

            var MostSearchedKeyword = context.KeywordsCounts
            .OrderByDescending(pc => pc.Count)
            .Take(5)
            .ToList();

           


            ViewData["ProductCount"] = topProductCounts;
            ViewData["KeywordCount"] = MostSearchedKeyword;



           /* ShopkeeperHomeVM shopkeeperHomeVM = new ShopkeeperHomeVM()
            {
                MostSearchedKeyword = MostSearchedKeyword,
                PopularProducts = topProducts
            };*/
    
            return View();
        }
        [Authorize]
        public IActionResult Create()
        {
            ApplicationUser applicationUser = _unitOfWork.ApplicationUserRepository.Get(u => u.UserName == User.Identity.Name);
            if (applicationUser == null)
            {
                return new RedirectToPageResult("/Identity/Account/Login");
            }
            var viewModel = new Shop
            {
                OwnerName = applicationUser.Name
            };
            return View(viewModel);
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
            ApplicationUser applicationUser = _unitOfWork.ApplicationUserRepository.Get(u => u.UserName == User.Identity.Name);
            _unitOfWork.ShopRepository.Add(shop);
            applicationUser.Shop = shop;
            _unitOfWork.ApplicationUserRepository.Update(applicationUser);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }
    }
}
