using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.ViewModels
{
    public class ApplicationUserModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string ProfileImageUrl { get; set; }
        public int Rating { get; set; }
        public bool IsActive { get; set; }
    }
}
