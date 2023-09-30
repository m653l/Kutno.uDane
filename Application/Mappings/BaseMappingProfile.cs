using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings
{
    public abstract class BaseMappingProfile : Profile
    {
        protected readonly IServiceProvider _serviceProvider;

        public BaseMappingProfile(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            AddProfiles();
        }

        protected abstract void AddProfiles();
    }
}
