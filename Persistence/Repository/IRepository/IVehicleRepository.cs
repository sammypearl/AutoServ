
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using Persistence.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository.IRepository
{
    public interface IVehicleRepository : IRepository<Vehicle>
    {
        IEnumerable<SelectListItem> GetVehicleListForDropDown();
        void Update(Vehicle model);
       // Vehicle AddV(VehicleViewModel model);
       // public string UploadImageIfAvailable(VehicleViewModel model);


    }
}
