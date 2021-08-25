
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository.IRepository
{
    public interface IMakeRepository : IRepository<Make>
    {
        IEnumerable<SelectListItem> GetModelListForDropDown();
        void Update(Make model);
        

    }
}
