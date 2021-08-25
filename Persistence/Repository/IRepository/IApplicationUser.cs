using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository.IRepository
{
    public interface IApplicationUser
    {
        ApplicationUser GetById(string id);
        IEnumerable<ApplicationUser> GetAll();
        Task Add(ApplicationUser user);
        Task Deactivate(ApplicationUser user);
        Task SetProfileImage(string id, Uri uri);
    }
}
