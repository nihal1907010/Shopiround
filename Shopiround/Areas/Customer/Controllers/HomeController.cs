using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopiround.Data;
using Newtonsoft.Json;
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
using Shopiround.Models.Statistics;
using Microsoft.AspNetCore.Http;

namespace Shopiround.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<HomeController> _logger;
        public readonly IWebHostEnvironment _webHostEnvironment;
        public ApplicationDbContext context;

        public HomeController(ILogger<HomeController> logger,
                                IUnitOfWork unitOfWork,
                                ApplicationDbContext context,
                                IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            this.unitOfWork = unitOfWork;
            this.context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> products = GetAllProducts();

            string[] filePaths = Directory.GetFiles(Path.Combine(_webHostEnvironment.WebRootPath, "images", "backgrounds"));
            List<string> files = new List<string>();
            foreach (string filePath in filePaths)
            {
                files.Add(Path.GetRelativePath(_webHostEnvironment.WebRootPath, filePath));
            }
            ViewBag.backgrounds = files;
            // User and Shop information
            ApplicationUser? user = context.ApplicationUsers.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            Shop? shop = null;
            if (user != null)
            {
                shop = context.Shops.Where(x => x.ApplicationUserId == user.Id).FirstOrDefault();
            }
            ViewBag.user = user;
            ViewBag.shop = shop;
            return View(products);
        }

        public List<Product> GetAllProducts()
        {
            List<Product> openTodayProducts = new List<Product>();

            string todayName = DateTime.Today.DayOfWeek.ToString();
            int currentTime = DateTime.Now.Hour;

            List<Shop> Shops = context.Shops.ToList();

            foreach (var shop in Shops)
            {
                bool isOpenToday = false;

                switch (todayName)
                {
                    case "Sunday":
                        isOpenToday = shop.Sunday;
                        break;
                    case "Monday":
                        isOpenToday = shop.Monday;
                        break;
                    case "Tuesday":
                        isOpenToday = shop.Tuesday;
                        break;
                    case "Wednesday":
                        isOpenToday = shop.Wednesday;
                        break;
                    case "Thursday":
                        isOpenToday = shop.Thursday;
                        break;
                    case "Friday":
                        isOpenToday = shop.Friday;
                        break;
                    case "Saturday":
                        isOpenToday = shop.Saturday;
                        break;
                }


                var b = TimeSpan.Parse(shop.OpeningTime);

                Boolean openNow = currentTime >= int.Parse(shop.OpeningTime) && currentTime <= int.Parse(shop.ClosingTime);



                if (isOpenToday && openNow)
                {
                    List<Product> productsForThisShop = context.Products
                                .Where(p => p.Quantity > 0 && p.ShopId == shop.ShopId)
                                .ToList();

                    openTodayProducts.AddRange(productsForThisShop);
                }
            }
            // User and Shop information
            ApplicationUser? user = context.ApplicationUsers.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            Shop? shopx = null;
            if (user != null)
            {
                shopx = context.Shops.Where(x => x.ApplicationUserId == user.Id).FirstOrDefault();
            }
            ViewBag.user = user;
            ViewBag.shop = shopx;
            return openTodayProducts;
        }




        [Authorize]
        public IActionResult UserProfile()
        {
            ApplicationUser user = unitOfWork.ApplicationUserRepository.Get(u => u.UserName == User.Identity.Name);
            if (user == null)
            {
                return new RedirectToPageResult("/Identity/Account/Login");
            }
            UserProfileVM userProfile = new UserProfileVM
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email,
                MobileNo = user.MobileNo,
                ImageURL = user.ImageURL,
                Address = user.Address
            };
            // User and Shop information
            Shop? shop = null;
            if (user != null)
            {
                shop = context.Shops.Where(x => x.ApplicationUserId == user.Id).FirstOrDefault();
            }
            ViewBag.user = user;
            ViewBag.shop = shop;
            return View(userProfile);
        }

        [HttpPost]
        public IActionResult UserProfile(UserProfileVM userProfile, IFormFile? file)
        {
            ApplicationUser user = unitOfWork.ApplicationUserRepository.Get(u => u.UserName == User.Identity.Name);

            if (user != null)
            {
                // If the profile exists, update it
                if (file != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\User");
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    user.ImageURL = @"\images\User\" + fileName;
                }
                // Update other properties if needed
                user.Name = userProfile.Name;
                //existingProfile.Email = userProfile.Email;
                user.MobileNo = userProfile.MobileNo;
                user.Address = userProfile.Address;

                context.ApplicationUsers.Update(user);
            }


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





        public IActionResult Search(string txt)
        {
            if (!string.IsNullOrWhiteSpace(txt))
            {
                string searchTerm = txt.ToLower().Replace(" ", ""); // Remove spaces and make it lowercase

                KeywordsCount keywordsCount = context.KeywordsCounts.FirstOrDefault(a => a.Keyword == txt);

                if (keywordsCount != null)
                {

                    keywordsCount.Count++;
                    context.KeywordsCounts.Update(keywordsCount);

                }
                else
                {
                    KeywordsCount keywordsCount1 = new KeywordsCount()
                    {
                        Keyword = txt,
                        Count = 1
                    };
                    context.KeywordsCounts.Add(keywordsCount1);
                }
                context.SaveChanges();


                List<Product> products = context.Products.Include("Shop").Include("Reviews").Include("Questions").Where(j => j.Name.ToLower().Replace(" ", "").Contains(searchTerm))
                    .ToList();
                return View("Search", products);
            }
            // User and Shop information
            ApplicationUser? user = context.ApplicationUsers.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            Shop? shop = null;
            if (user != null)
            {
                shop = context.Shops.Where(x => x.ApplicationUserId == user.Id).FirstOrDefault();
            }
            ViewBag.user = user;
            ViewBag.shop = shop;
            return View("Search");
        }
        [Authorize]
        public IActionResult ViewCart(Boolean online = false)
        {

            ApplicationUser user = unitOfWork.ApplicationUserRepository.Get(u => u.UserName == User.Identity.Name, includeProperties: "Shop,CartItems");
            if (user == null)
            {
                return new RedirectToPageResult("/Identity/Account/Login");
            }
            List<CartItem> cartItems = context.CartItems.Where(c => c.Online == online).Include(c => c.Product).ThenInclude(s => s.Shop).Where(c => c.UserId == user.Id).ToList();
            ViewBag.online = online;

            // User and Shop information
            Shop? shop = null;
            if (user != null)
            {
                shop = context.Shops.Where(x => x.ApplicationUserId == user.Id).FirstOrDefault();
            }
            ViewBag.user = user;
            ViewBag.shop = shop;
            return View(cartItems);
        }
        [Authorize]
        public IActionResult OnlineCart()
        {
            ApplicationUser applicationUser = context.ApplicationUsers.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            if (applicationUser == null)
            {
                return new RedirectToPageResult("/Identity/Account/Login");
            }
            List<CartItem> cartItems = context.CartItems.Where(c => c.Online && c.OrderPlaced == false).Include(c => c.Product).ThenInclude(s => s.Shop).Where(c => c.UserId == applicationUser.Id).ToList();


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
        public IActionResult PlaceOrder()
        {
            LocationVM locationVM = new LocationVM();

            // User and Shop information
            ApplicationUser? user = context.ApplicationUsers.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            Shop? shop = null;
            if (user != null)
            {
                shop = context.Shops.Where(x => x.ApplicationUserId == user.Id).FirstOrDefault();
            }
            ViewBag.user = user;
            ViewBag.shop = shop;
            return View(locationVM);
        }
        [HttpPost]
        public IActionResult PlaceOrder(LocationVM locationVM)
        {
            ApplicationUser applicationUser = context.ApplicationUsers.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            if (applicationUser == null)
            {
                return new RedirectToPageResult("/Identity/Account/Login");
            }
            List<CartItem> cartItems = context.CartItems.Where(c => c.Online && c.OrderPlaced == false).Include(c => c.Product).ThenInclude(s => s.Shop).Where(c => c.UserId == applicationUser.Id).ToList();
            List<DeliveryInformation> deliveryInformation = new List<DeliveryInformation>();
            foreach (CartItem cartItem in cartItems)
            {
                deliveryInformation.Add(new DeliveryInformation
                {
                    cartItemId = cartItem.Id,
                    Latitude = locationVM.Latitude,
                    Longitude = locationVM.Longitude
                });
                cartItem.OrderPlaced = true;
            }
            context.CartItems.UpdateRange(cartItems);
            context.DeliveryInformation.AddRange(deliveryInformation);
            context.SaveChanges();
            // User and Shop information
            ApplicationUser? user = context.ApplicationUsers.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            Shop? shop = null;
            if (user != null)
            {
                shop = context.Shops.Where(x => x.ApplicationUserId == user.Id).FirstOrDefault();
            }
            ViewBag.user = user;
            ViewBag.shop = shop;
            return RedirectToAction("Index");
        }
        [Authorize]
        public IActionResult OnlineOrders()
        {
            ApplicationUser applicationUser = context.ApplicationUsers.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            if (applicationUser == null)
            {
                return new RedirectToPageResult("/Identity/Account/Login");
            }
            List<CartItem> cartItems = context.CartItems.Where(c => c.Online).Include(c => c.Product).ThenInclude(s => s.Shop).Where(c => c.UserId == applicationUser.Id && c.OrderPlaced).ToList();
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

        [HttpPost]
        public ActionResult UpdateCart(int ProductId, int Quantity, bool online = false)
        {
            ApplicationUser user = unitOfWork.ApplicationUserRepository.Get(u => u.UserName == User.Identity.Name, includeProperties: "Shop,CartItems");
            List<CartItem> cartItems = context.CartItems.Where(c => c.Online == online)
                .Include(c => c.Product)
                .ThenInclude(s => s.Shop)
                .Where(c => c.UserId == user.Id)
                .ToList();

            // Find the specific CartItem that matches the given ProductId
            CartItem cartItemToUpdate = cartItems.FirstOrDefault(c => c.Product.Id == ProductId);

            if (cartItemToUpdate != null)
            {
                cartItemToUpdate.Quantity = Quantity;
                context.SaveChanges();
            }
            // User and Shop information
            Shop? shop = null;
            if (user != null)
            {
                shop = context.Shops.Where(x => x.ApplicationUserId == user.Id).FirstOrDefault();
            }
            ViewBag.user = user;
            ViewBag.shop = shop;
            return Json(new { added = true });
        }

        [Authorize]
        public IActionResult PurchasedItems()
        {

            ApplicationUser user = unitOfWork.ApplicationUserRepository.Get(u => u.UserName == User.Identity.Name, includeProperties: "Shop,CartItems");
            if (user == null)
            {
                return new RedirectToPageResult("/Identity/Account/Login");
            }
            List<PurchaseItem> purchaseItems = context.PurchaseItems.Include(c => c.Product).ThenInclude(s => s.Shop).Where(c => c.UserId == user.Id).ToList();
            // User and Shop information
            Shop? shop = null;
            if (user != null)
            {
                shop = context.Shops.Where(x => x.ApplicationUserId == user.Id).FirstOrDefault();
            }
            ViewBag.user = user;
            ViewBag.shop = shop;
            return View(purchaseItems);
        }

        public IActionResult ShowRoute()
        {
            ApplicationUser user = unitOfWork.ApplicationUserRepository.Get(u => u.UserName == User.Identity.Name, includeProperties: "Shop,CartItems");
            if (user == null)
            {
                return new RedirectToPageResult("/Identity/Account/Login");
            }
            List<CartItem> cartItems = context.CartItems.Include(c => c.Product).ThenInclude(s => s.Shop).Where(c => c.UserId == user.Id).ToList();
            // User and Shop information
            Shop? shop = null;
            if (user != null)
            {
                shop = context.Shops.Where(x => x.ApplicationUserId == user.Id).FirstOrDefault();
            }
            ViewBag.user = user;
            ViewBag.shop = shop;
            return View(cartItems);
        }
        public IActionResult ViewSaved()
        {
            ApplicationUser applicationUser =
                unitOfWork.ApplicationUserRepository.Get(user => user.UserName == User.Identity.Name,
                includeProperties: "Shop,SavedItems");
            if (applicationUser == null)
            {
                return new RedirectToPageResult("/Identity/Account/Login");
            }
            List<SavedItem> savedItems = context.SavedItems.
                Include(item => item.Product).
                ThenInclude(product => product.Shop).
                Where(item => item.UserId == applicationUser.Id).ToList();
            // User and Shop information
            ApplicationUser? user = context.ApplicationUsers.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            Shop? shop = null;
            if (user != null)
            {
                shop = context.Shops.Where(x => x.ApplicationUserId == user.Id).FirstOrDefault();
            }
            ViewBag.user = user;
            ViewBag.shop = shop;
            return View(savedItems);
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
            // User and Shop information
            Shop? shop = null;
            if (user != null)
            {
                shop = context.Shops.Where(x => x.ApplicationUserId == user.Id).FirstOrDefault();
            }
            ViewBag.user = user;
            ViewBag.shop = shop;
            return View("ViewCart", cartItems);
        }

        [HttpGet]
        public IActionResult SaveCartItem(int id)
        {
            CartItem item = context.CartItems.Find(id);
            CartItem cartItem = unitOfWork.CartItemRepository.Get(c => c.Id == id);

            SavedItem savedItem = new SavedItem
            {
                UserId = cartItem.UserId,
                ProductId = cartItem.ProductId
            };

            context.SavedItems.Add(savedItem);
            context.SaveChanges();


            ApplicationUser user = unitOfWork.ApplicationUserRepository.Get(u => u.UserName == User.Identity.Name, includeProperties: "Shop,CartItems");

            unitOfWork.CartItemRepository.Remove(cartItem);
            unitOfWork.Save();
            List<CartItem> cartItems = context.CartItems.Include(c => c.Product).ThenInclude(s => s.Shop).Where(c => c.UserId == user.Id).ToList();
            // User and Shop information
            Shop? shop = null;
            if (user != null)
            {
                shop = context.Shops.Where(x => x.ApplicationUserId == user.Id).FirstOrDefault();
            }
            ViewBag.user = user;
            ViewBag.shop = shop;
            return View("ViewCart", cartItems);
        }

        public IActionResult NearYou()
        {
            List<Product> products = context.Products.Include("Shop").Include("Reviews").Include("Questions").ToList();
            // User and Shop information
            ApplicationUser? user = context.ApplicationUsers.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            Shop? shop = null;
            if (user != null)
            {
                shop = context.Shops.Where(x => x.ApplicationUserId == user.Id).FirstOrDefault();
            }
            ViewBag.user = user;
            ViewBag.shop = shop;
            return View(products);
        }
        public IActionResult OrderOnline()
        {
            List<Product> products = context.Products.Include("Shop").Include("Reviews").Include("Questions").Where(p => p.Shop.AcceptOnlineOrders == true).ToList();
            // User and Shop information
            ApplicationUser? user = context.ApplicationUsers.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            Shop? shop = null;
            if (user != null)
            {
                shop = context.Shops.Where(x => x.ApplicationUserId == user.Id).FirstOrDefault();
            }
            ViewBag.user = user;
            ViewBag.shop = shop;
            return View(products);
        }
        [Authorize]
        public IActionResult DoneOfflineShopping(string onlinexxx = "false", string userId = null)
        {
            if (userId == null) userId = User.Identity.Name;
            Boolean onlinex = false;
            if (onlinexxx == "true") onlinex = true;
            ApplicationUser user = unitOfWork.ApplicationUserRepository.Get(u => u.UserName == userId, includeProperties: "Shop,CartItems");
            if (user == null)
            {
                return new RedirectToPageResult("/Identity/Account/Login");
            }
            List<Product> products = context.Products.ToList();
            List<CartItem> cartItems = context.CartItems.Include(c => c.Product).ThenInclude(s => s.Shop).Where(c => c.UserId == user.Id).ToList();

            //update quantity of product table
            foreach (var cartItem in cartItems)
            {
                Product matchingProduct = products.FirstOrDefault(p => p.Id == cartItem.ProductId);

                if (matchingProduct != null)
                {
                    int a = matchingProduct.Quantity;
                    matchingProduct.Quantity = a - cartItem.Quantity;
                    context.SaveChanges();
                }
            }
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult X()
        {
            return View();
        }
    }
}
