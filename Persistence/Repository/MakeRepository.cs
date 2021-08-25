
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using Persistence.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public class MakeRepository : Repository<Make>, IMakeRepository
    {
        private readonly AutoServDbContext _db;
        public MakeRepository(AutoServDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetModelListForDropDown()
        {
            return _db.Makes.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
        }

        public void Update(Make model)
        {
            var objFromDb = _db.Makes
                .FirstOrDefault(m => m.Id == model.Id);

            objFromDb.Name = model.Name;
            objFromDb.Model = model.Model;

            _db.SaveChanges();
        }
    }
}
