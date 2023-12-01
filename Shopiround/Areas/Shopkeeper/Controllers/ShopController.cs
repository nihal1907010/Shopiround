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

            var MostSearchedKeyword = context.KeywordsCounts
            .OrderByDescending(pc => pc.Count)
            .Take(5)
            .ToList();

            ViewData["ProductCount"] = topProductCounts;
            ViewData["KeywordCount"] = MostSearchedKeyword;
            // User and Shop information
            ApplicationUser? user = context.ApplicationUsers.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            Shop? shop = null;
            if (user != null)
            {
                shop = context.Shops.Where(x => x.ApplicationUserId == user.Id).FirstOrDefault();
            }
            ViewBag.user = user;
            ViewBag.shop = shop;
            return View();
        }

        public IActionResult ShowAllPopularProduct()
        {
            var topProductCounts = context.ProductCounts
            .OrderByDescending(pc => pc.Count)
            .Include("Product").Include("Shop")
            .ToList();

            ViewData["ProductCount"] = topProductCounts;
            // User and Shop information
            ApplicationUser? user = context.ApplicationUsers.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            Shop? shop = null;
            if (user != null)
            {
                shop = context.Shops.Where(x => x.ApplicationUserId == user.Id).FirstOrDefault();
            }
            ViewBag.user = user;
            ViewBag.shop = shop;
            return View();
        }

        public IActionResult CustomerPopularProduct()
        {
            var topProductCounts = context.ProductCounts
            .OrderByDescending(pc => pc.Count)
            .Include("Product")
            .ToList();

            ViewData["ProductCount"] = topProductCounts;
            // User and Shop information
            ApplicationUser? user = context.ApplicationUsers.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            Shop? shop = null;
            if (user != null)
            {
                shop = context.Shops.Where(x => x.ApplicationUserId == user.Id).FirstOrDefault();
            }
            ViewBag.user = user;
            ViewBag.shop = shop;
            return View();
        }
        public IActionResult ShowAllKeywords()
        {
            var MostSearchedKeyword = context.KeywordsCounts
            .OrderByDescending(pc => pc.Count)
            .ToList();

            ViewData["KeywordCount"] = MostSearchedKeyword;
            // User and Shop information
            ApplicationUser? user = context.ApplicationUsers.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            Shop? shop = null;
            if (user != null)
            {
                shop = context.Shops.Where(x => x.ApplicationUserId == user.Id).FirstOrDefault();
            }
            ViewBag.user = user;
            ViewBag.shop = shop;
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
            // User and Shop information
            ApplicationUser? user = context.ApplicationUsers.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            Shop? shop = null;
            if (user != null)
            {
                shop = context.Shops.Where(x => x.ApplicationUserId == user.Id).FirstOrDefault();
            }
            ViewBag.user = user;
            ViewBag.shop = shop;
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
            // User and Shop information
            ApplicationUser? user = context.ApplicationUsers.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.user = user;
            ViewBag.shop = shop;
            return RedirectToAction("Index");
        }

        public IActionResult Profile()
        {
            ApplicationUser applicationUser = _unitOfWork.ApplicationUserRepository.Get(u => u.UserName == User.Identity.Name, includeProperties: "Shop");
            Shop shop = applicationUser.Shop;
            List<Product> products = context.Products.Where(p => p.ShopId == shop.ShopId).ToList();

            ShopProfile shopProfile = new ShopProfile
            {
                shop = shop,
                products = products
            };
            // User and Shop information
            ApplicationUser? user = context.ApplicationUsers.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.user = user;
            ViewBag.shop = shop;
            return View(shopProfile);
        }
        public IActionResult ShowUserInformation(string userId)
        {
            ApplicationUser user = _unitOfWork.ApplicationUserRepository.Get(u => u.Id == userId);
            ApplicationUser applicationUser = _unitOfWork.ApplicationUserRepository.Get(u => u.UserName == User.Identity.Name, includeProperties: "Shop");
            UserProfileVM userProfile = new UserProfileVM
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email,
                MobileNo = user.MobileNo,
                ImageURL = user.ImageURL,
                Address = user.Address
            };
            DeliveryInformation delivery = context.DeliveryInformation.Include(d => d.CartItem).Where(d => d.CartItem.UserId == user.Id).FirstOrDefault();
            ViewBag.deliveryInfo = delivery;
            // User and Shop information
            Shop? shop = null;
            if (user != null)
            {
                shop = context.Shops.Where(x => x.ApplicationUserId == applicationUser.Id).FirstOrDefault();
            }
            ViewBag.user = user;
            ViewBag.shop = shop;
            return View(userProfile);
        }

        public IActionResult OnlineOrdersHome()
        {
            ApplicationUser applicationUser = _unitOfWork.ApplicationUserRepository.Get(u => u.UserName == User.Identity.Name, includeProperties: "Shop");
            List<Product> products = context.Products.Where(p => p.ShopId == applicationUser.Shop.ShopId).ToList();
            List<CartItem> cartItems = context.CartItems.Include(c => c.Product).Include(c => c.User).Where(c => products.Contains(c.Product) && c.Online).ToList();
            List<OnlineOrderVM> onlineOrders = (from cart in cartItems
                                                group cart by cart.UserId into g
                                                select new OnlineOrderVM
                                                {
                                                    User = context.ApplicationUsers.Where(u => u.Id == g.Key).First(),
                                                    TotalOrders = g.Count(),
                                                    TotalPrice = (int)g.Sum(c => c.Product.Price)
                                                }).ToList();
            onlineOrders.Reverse();
            // User and Shop information
            ApplicationUser? user = context.ApplicationUsers.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            Shop? shop = null;
            if (user != null)
            {
                shop = context.Shops.Where(x => x.ApplicationUserId == user.Id).FirstOrDefault();
            }
            ViewBag.user = user;
            ViewBag.shop = shop;
            return View(onlineOrders);
        }
        [Authorize]
        public IActionResult DoneOnlineShopping(string userId)
        {
            ApplicationUser user = _unitOfWork.ApplicationUserRepository.Get(u => u.UserName == userId, includeProperties: "Shop,CartItems");
            List<CartItem> cartItems = context.CartItems.Include(c => c.Product).ThenInclude(s => s.Shop).Where(c => c.UserId == userId && c.Online).ToList();
            foreach (CartItem cartItem in cartItems)
            {
                PurchaseItem purchaseItem = new PurchaseItem
                {
                    UserId = cartItem.UserId,
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    Online = cartItem.Online,
                    Reserve = cartItem.Reserve,
                    PurchaseDate = DateTime.Now
                };
                context.PurchaseItems.Add(purchaseItem);
            }
            context.CartItems.RemoveRange(cartItems);
            context.SaveChanges();
            // User and Shop information
            Shop? shop = null;
            if (user != null)
            {
                shop = context.Shops.Where(x => x.ApplicationUserId == user.Id).FirstOrDefault();
            }
            ViewBag.user = user;
            ViewBag.shop = shop;
            return RedirectToAction("Index");
        }

        public IActionResult OnlineOrder(string userId)
        {
            List<CartItem> cartItems = context.CartItems.Where(c => c.Online).Include(c => c.Product).ThenInclude(s => s.Shop).Where(c => c.UserId == userId).ToList();
            ViewBag.userId = userId;
            // User and Shop information
            ApplicationUser? user = context.ApplicationUsers.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            Shop? shop = null;
            if (user != null)
            {
                shop = context.Shops.Where(x => x.ApplicationUserId == user.Id).FirstOrDefault();
            }
            ViewBag.user = user;
            ViewBag.shop = shop;
            return View(cartItems);
        }

        public IActionResult Profile(int? id)
        {

            if(id == null)
            {
                ApplicationUser applicationUser = _unitOfWork.ApplicationUserRepository.Get(u => u.UserName == User.Identity.Name, includeProperties: "Shop");
                Shop shop = applicationUser.Shop;
                List<Product> products = context.Products.Where(p => p.ShopId == shop.ShopId).ToList();

                ShopProfile shopProfile = new ShopProfile
                {
                    shop = shop,
                    products = products
                };

                return View(shopProfile);
            }
            else
            {
                Shop shop = context.Shops.Where(a=> a.ShopId == id).FirstOrDefault();
                List<Product> products = context.Products.Where(p => p.ShopId == shop.ShopId).ToList();

                ShopProfile shopProfile = new ShopProfile
                {
                    shop = shop,
                    products = products
                };

                return View(shopProfile);
            }
           
        }


        public IActionResult ShopNearby()
        {
            List<Shop> shops = context.Shops.ToList();
            return View(shops);
        }

    }
}
