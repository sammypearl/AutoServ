using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Models;
using Persistence.Data;

namespace Persistence
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Model, ModelResources>();           
        }
    }
}
