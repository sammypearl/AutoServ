using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models;
using Persistence.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public class SellerRepository : Repository<Seller>, ISellerRepository
    {
        private readonly AutoServDbContext _db;
        public SellerRepository(AutoServDbContext db) : base (db)
        {
            _db = db;
        }
        public IEnumerable<SelectListItem> GetModelListForDropDown()
        {
            return _db.Sellers.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.SellerId.ToString()
            });
        }
        public IQueryable<Vehicle> SellerVehiclesAsync(int sellerId)
        {
            var vehicles = _db.Vehicles.AsNoTracking().Where(x => x.SellerId == sellerId);

            return vehicles;
        }

        public void Update(Seller model)
        {
            var objFromDb = _db.Sellers
                .FirstOrDefault(m => m.SellerId == model.SellerId);

            objFromDb.SellerId = model.SellerId;
            objFromDb.Name = model.Name;
            objFromDb.Title = model.Title;
            objFromDb.SellerEmail = model.SellerEmail;
            objFromDb.Age = model.Age;
            objFromDb.Sex = model.Sex;
            objFromDb.Company = model.Company;
            objFromDb.Address = model.Address;
            objFromDb.SellerPhone = model.SellerPhone;
            objFromDb.City = model.City;
            objFromDb.Country = model.Country;
            objFromDb.PhoneNo2 = model.PhoneNo2;
            objFromDb.PostalCode = model.PostalCode;
            objFromDb.ProfileImagePath = model.ProfileImagePath;


            _db.SaveChanges();
        }
    }
}
