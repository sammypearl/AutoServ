using Models;
using Persistence.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public class ApplicationUserRepository : IApplicationUser
    {
        private readonly AutoServDbContext _db;

        public ApplicationUserRepository(AutoServDbContext db)
        {
            _db = db;
        }

        public async Task Add(ApplicationUser user)
        {
            _db.Add(user);
            await _db.SaveChangesAsync();
        }

        public async Task Deactivate(ApplicationUser user)
        {
            user.IsActive = false;
            _db.Update(user);
            await _db.SaveChangesAsync();
        }

        public ApplicationUser GetByName(string name)
        {
            return _db.ApplicationUsers.FirstOrDefault(user => user.UserName == name);
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return _db.ApplicationUsers;
        }

        public ApplicationUser GetById(string id)
        {
            return _db.ApplicationUsers.FirstOrDefault(user => user.Id == id);
        }

       

        public async Task SetProfileImage(string id, Uri uri)
        {
            var user = GetById(id);
           // user.ProfileImageUrl = uri.AbsoluteUri;
            _db.Update(user);
            await _db.SaveChangesAsync();
        }

        

        
    }
}
