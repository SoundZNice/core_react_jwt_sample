using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using AutoMapper;
using Core3.Application.Interfaces.Mapping;

namespace Core3.Application.Infrastructure.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            LoadStandardMappings();
            LoadCustomMappings();
        }

        private void LoadStandardMappings()
        {
            IList<Map> mapsFrom = MapperProfileHelper.LoadStandardMappings(Assembly.GetExecutingAssembly());

            foreach (Map map in mapsFrom)
            {
                CreateMap(map.Source, map.Destination).ReverseMap();
            }
        }

        private void LoadCustomMappings()
        {
            IList<IHaveCustomMapping> mapsFrom = MapperProfileHelper.LoadCustomMappings(Assembly.GetExecutingAssembly());

            foreach (IHaveCustomMapping map in mapsFrom)
            {
                map.CreateMappings(this);
            }
        }
    }
}
