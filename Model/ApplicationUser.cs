using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class ApplicationUser : IdentityUser
    {
       
        [Display(Name = "Office Phone")]
        public string PhoneNumber2 { get; set; }
        public int Rating { get; set; }
     // public string ProfileImageUrl { get; set; }
        public DateTime MemberSince { get; set; }
        public bool IsActive { get; set; }
        [NotMapped]
        public bool IsAdmin { get; set; }

    }
}
