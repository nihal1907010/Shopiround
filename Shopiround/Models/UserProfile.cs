using System;
using System.ComponentModel.DataAnnotations;

namespace Shopiround.Models
{
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }
        public string userId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string ImageURL { get; set; }
        public string Address {  get; set; }

    }
}
