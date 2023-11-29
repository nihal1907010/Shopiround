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
            List<Product> products = context.Products.Include("Shop").ToList();
            string[] filePaths = Directory.GetFiles(Path.Combine(_webHostEnvironment.WebRootPath, "images", "backgrounds"));
            List<string> files = new List<string>();
            foreach (string filePath in filePaths)
            {
                files.Add(Path.GetRelativePath(_webHostEnvironment.WebRootPath, filePath));
            }

            // User and Shop information
            ApplicationUser? user = context.ApplicationUsers.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            Shop? shop = null;
            if (user != null)
            {
                shop = context.Shops.Where(x => x.ApplicationUserId == user.Id).FirstOrDefault();
            }


            ViewBag.backgrounds = files;
            ViewBag.user = user;
            ViewBag.shop = shop;
            return View(products);
        }
        [Authorize]
        public IActionResult UserProfile()
        {
            ApplicationUser user = unitOfWork.ApplicationUserRepository.Get(u => u.UserName == User.Identity.Name);
            if (user == null)
            {
                return new RedirectToPageResult("/Identity/Account/Login");
            }
            UserProfile userProfile = context.UserProfiles.FirstOrDefault(u => u.userId == user.Id);

            return View(userProfile);
        }

        [HttpPost]
        public IActionResult UserProfile(UserProfile userProfile, IFormFile? file)
        {
            ApplicationUser user = unitOfWork.ApplicationUserRepository.Get(u => u.UserName == User.Identity.Name);
            userProfile.userId = user.Id;

            // Check if the user profile already exists in the database
            UserProfile existingProfile = context.UserProfiles.FirstOrDefault(u => u.userId == userProfile.userId);

            if (existingProfile != null)
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
                    existingProfile.ImageURL = @"\images\User\" + fileName;
                }
                // Update other properties if needed
                existingProfile.Name = userProfile.Name;
                existingProfile.Email = userProfile.Email;
                existingProfile.MobileNo = userProfile.MobileNo;
                existingProfile.Address = userProfile.Address;

                context.UserProfiles.Update(existingProfile);
            }
            else
            {
                // If the profile doesn't exist, add a new one
                if (file != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\User");
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    userProfile.ImageURL = @"\images\User\" + fileName;
                }
                else
                {
                    userProfile.ImageURL = @"\images\shop\default_shop.png";
                }

                context.UserProfiles.Add(userProfile);
            }

            context.SaveChanges();

            return RedirectToAction("Index");
        }





        public IActionResult Search(string txt)
        {
            if (!string.IsNullOrWhiteSpace(txt))
            {
                string searchTerm = txt.ToLower().Replace(" ", ""); // Remove spaces and make it lowercase

                KeywordsCount keywordsCount = context.KeywordsCounts.FirstOrDefault(a => a.Keyword == txt);

                    if(keywordsCount != null)
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
            return View(cartItems);
        }
        public IActionResult PlaceOrder()
        {
            LocationVM locationVM = new LocationVM();
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
            foreach(CartItem cartItem in cartItems)
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
            return View(cartItems);
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
            List<CartItem> cartItems = context.CartItems.Include(c => c.Product).ThenInclude(s => s.Shop).Where(c => c.UserId == user.Id && c.Online == onlinex).ToList();
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
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
