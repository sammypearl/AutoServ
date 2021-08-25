using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository.IRepository
{
    public interface ISellerRepository : IRepository<Seller>
    {
        IEnumerable<SelectListItem> GetModelListForDropDown();
        void Update(Seller model);
        public IQueryable<Vehicle> SellerVehiclesAsync(int sellerId);

    }
}
