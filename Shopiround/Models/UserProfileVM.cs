using System;
using System.ComponentModel.DataAnnotations;

namespace Shopiround.Models
{
    public class UserProfileVM
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? MobileNo { get; set; }
        public string? ImageURL { get; set; }
        public string? Address {  get; set; }

    }
}
