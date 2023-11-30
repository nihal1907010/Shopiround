using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopiround.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public Shop Shop { get; set; }
        public List<CartItem> CartItems { get; set; }
        public List<SavedItem> SavedItems { get; set; }
        public string? MobileNo { get; set; }
        public string? Address { get; set; }    
        public string? ImageURL { get; set; }
    }
}
