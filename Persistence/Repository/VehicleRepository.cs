
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using Persistence.Repository.IRepository;
using Persistence.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public class VehicleRepository : Repository<Vehicle>, IVehicleRepository
    {
        private readonly AutoServDbContext _db;
      //  private readonly IWebHostEnvironment _hostEnvironment;
        public VehicleRepository(AutoServDbContext db) : base(db)
        {
            _db = db;
           // _hostEnvironment = hostEnvironment;
        }

       
        

        public IEnumerable<SelectListItem> GetVehicleListForDropDown()
        {
            throw new NotImplementedException();
        }

        public void Update(Vehicle model)
        {
            var objFromDb = _db.Vehicles
               .FirstOrDefault(m => m.Id == model.Id);

            

            _db.SaveChanges();
        }

        // public string UploadImageIfAvailable(VehicleViewModel model)
        // {
        //    string uniqueFileName = null;
        //    if (model.Photo != null)
        //    {

        //        string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images");
        //        uniqueFileName = Guid.NewGuid().ToString() + "-" + model.Photo.FileName;
        //        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
        //        using (var fileStream = new FileStream(filePath, FileMode.Create))
        //        {
        //            model.Photo.CopyTo(fileStream);
        //        }

        //    }

        //    return uniqueFileName;
        //}
    }
}
