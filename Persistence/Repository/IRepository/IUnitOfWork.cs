using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IMakeRepository Make { get; }
        IModelRepository Model { get; }
        ISellerRepository Seller { get; }
        IVehicleRepository Vehicle { get; }
        void Save();
        void SaveAsync();

    }
}
