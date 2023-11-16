using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopiround.Data;
using Shopiround.Models;
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
        public IActionResult Home(string ownerId)
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
                ReceiverId = ownerId
            };
            return View(messageVM);
        }
    }
}
