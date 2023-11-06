using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopiround.Data;
using Newtonsoft.Json;
using Shopiround.Data;
using Shopiround.Repository.IRepository;
using Shopiround.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;

namespace Shopiround.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {

        private readonly IUnitOfWork unitOfWork;

        private readonly ILogger<HomeController> _logger;
        public readonly IWebHostEnvironment _webHostEnvironment;
        public ApplicationDbContext context;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            this.unitOfWork = unitOfWork;
            this.context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        
        public IActionResult Index()
        {

            List<Product> products = context.Products.Include("Shop").ToList();
            string[] filePaths = Directory.GetFiles(Path.Combine(_webHostEnvironment.WebRootPath, "images", "backgrounds"));
            List<string> files = new List<string>();
            foreach (string filePath in filePaths)
            {
                files.Add(Path.GetRelativePath(_webHostEnvironment.WebRootPath, filePath));
            }
            ViewBag.backgrounds = files;
            return View(products);
        }



        public IActionResult Search(string txt)
        {
            if (!string.IsNullOrWhiteSpace(txt))
            {
                string searchTerm = txt.ToLower().Replace(" ", ""); // Remove spaces and make it lowercase
                List<Product> products = context.Products.Include("Shop").Include("Reviews").Include("Questions").Where(j => j.Name.ToLower().Replace(" ", "").Contains(searchTerm))
                    .ToList();
                return View("Search", products);
            }

            return View("Search");
        }


        public IActionResult Privacy()
        {
            int Id = 1;
            if (Id != null)
            {
                Product product = unitOfWork.ProductRepository.Get(p => p.Id == Id, includeProperties: "Shop,Reviews");
                return View(product);
            }
            else
            {
                return NotFound();
            }
            return View();
        }
        [Authorize]
        public IActionResult ViewCart()
        {
            
            ApplicationUser user = unitOfWork.ApplicationUserRepository.Get(u => u.UserName == User.Identity.Name, includeProperties: "Shop,CartItems");
            if (user == null)
            {
                return new RedirectToPageResult("/Identity/Account/Login");
            }
            List<CartItem> cartItems = context.CartItems.Include(c => c.Product).ThenInclude(s => s.Shop).Where(c => c.UserId == user.Id).ToList();
            return View(cartItems);
        }
        [HttpGet]
        public IActionResult DeleteCartItem(int id)
        {
            CartItem item = context.CartItems.Find(id);
            CartItem cartItem = unitOfWork.CartItemRepository.Get(c => c.Id == id);


            ApplicationUser user = unitOfWork.ApplicationUserRepository.Get(u => u.UserName == User.Identity.Name, includeProperties: "Shop,CartItems");
            
            unitOfWork.CartItemRepository.Remove(cartItem);
            unitOfWork.Save();
            List<CartItem> cartItems = context.CartItems.Include(c => c.Product).ThenInclude(s => s.Shop).Where(c => c.UserId == user.Id).ToList();
            return View("ViewCart", cartItems);
        }

        public IActionResult NearYou()
        {
            List<Product> products = context.Products.Include("Shop").Include("Reviews").Include("Questions").ToList();
            return View(products);
        }
        public IActionResult OrderOnline()
        {
            List<Product> products = context.Products.Include("Shop").Include("Reviews").Include("Questions").Where(p => p.Shop.AcceptOnlineOrders == true).ToList();
            return View(products);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
