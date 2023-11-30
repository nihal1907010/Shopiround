using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Shopiround.Data;
using Shopiround.Models;
using System.Collections.Generic;
using System.Linq;

namespace Shopiround.Areas.User.Controllers
{
    [Area("User")]
    public class ChatController : Controller
    {
        public ApplicationDbContext applicationDbContext;
        public ChatController(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        [Authorize]
        public IActionResult Home(string ownerId, int productId)
        {
            ApplicationUser applicationUser =
                applicationDbContext.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (applicationUser == null)
            {
                return new RedirectToPageResult("/Identity/Account/Login");
            }
            MessageVM messageVM = new MessageVM
            {
                SenderId = applicationUser.Id,
                ReceiverId = ownerId,
                Product = applicationDbContext.Products.Where(p => p.Id == productId).Include("Shop").FirstOrDefault(),
                OldMessages = applicationDbContext.Message.Where(m => m.SenderId == applicationUser.Id || m.ReceiverId == ownerId).ToList()
            };
            return View(messageVM);
        }
        public IActionResult HomeShop()
        {
            ApplicationUser applicationUser =
                applicationDbContext.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (applicationUser == null)
            {
                return new RedirectToPageResult("/Identity/Account/Login");
            }
            List<Message> allMessages = applicationDbContext.Message.Where(m => m.ReceiverId == applicationUser.Id).Include(m => m.Sender).Include(m => m.Product).ToList();
            var latestMessages = applicationDbContext.Message
                    .Where(m => m.ReceiverId == applicationUser.Id)
                    .Include(m => m.Sender).Include(m => m.Product)
                    .GroupBy(m => new { m.SenderId, m.ProductId })
                    .Select(g => g.OrderByDescending(m => m.SendTime).First())
                    .ToList();
            return View(latestMessages);
        }
    }
}
