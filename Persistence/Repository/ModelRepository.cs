
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
    public class ModelRepository : Repository<Model>, IModelRepository
    {
        private readonly AutoServDbContext _db;
        public ModelRepository(AutoServDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetModelListForDropDown()
        {
            return _db.Models.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
        }

       

        public void Update(Model model)
        {
            var objFromDb = _db.Models
                .FirstOrDefault(m => m.Id == model.Id);

            objFromDb.Name = model.Name;
            objFromDb.Make = model.Make;
            objFromDb.MakeID = model.MakeID;

            _db.SaveChanges();
        }
    }
}
