using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Seller 
    {
        
        public int SellerId { get; set; }
        [Required]
        [StringLength(255)]
        [DisplayName("Seller Name")]
        public string Name { get; set; }
        [Required]
        [StringLength(255)]
        [DisplayName("Seller Title")]
        public string Title { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email ID")]
        [StringLength(50)]
        public string SellerEmail { get; set; }
        public int Age { get; set; }
        public string Sex { get; set; }
        [Required]
        [StringLength(255)]
        [DisplayName("Company Name")]
        public string Company { get; set; }
        public string Address { get; set; }
        [Phone]
        [StringLength(15)]
        public string SellerPhone { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        [Phone]
        [Display(Name = "Phone Number 2")]
        [StringLength(15)]
        public string PhoneNo2 { get; set; }
        public string PostalCode { get; set; }
        [Display(Name = "Profile Image")]
        [StringLength(100)]
        public string ProfileImagePath { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; } = new HashSet<Vehicle>();

    }
}
