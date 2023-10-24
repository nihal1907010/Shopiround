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
    }
}
