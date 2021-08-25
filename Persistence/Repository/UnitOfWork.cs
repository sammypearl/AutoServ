
using Persistence.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AutoServDbContext _db;
        public UnitOfWork(AutoServDbContext db)
        {
            _db = db;
            Make = new MakeRepository(_db);
            Vehicle = new VehicleRepository(_db);
            Model = new ModelRepository(_db);
            Seller = new SellerRepository(_db);

        }
        public IMakeRepository Make { get; private set; }

        public IModelRepository Model { get; private set; }

        public IVehicleRepository Vehicle { get; private set; }

        public ISellerRepository Seller { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void SaveAsync()
        {
            _db.SaveChangesAsync();
        }
    }
}
