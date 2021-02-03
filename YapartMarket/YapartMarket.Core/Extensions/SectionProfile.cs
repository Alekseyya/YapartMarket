using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using YapartMarket.Core.AccessModels;
using YapartMarket.Core.Models;

namespace YapartMarket.Core.Extensions
{
    public class SectionProfile : Profile
    {
        public SectionProfile()
        {
            CreateMap<IGrouping<string, AccessProductType>, Section>()
                .ForMember(section => section.Name, accessProductType => accessProductType.MapFrom(a =>a.Key))
                .ForMember(section => section.Categories, accessProductType => accessProductType.MapFrom(a => a.ToList()));
            CreateMap<AccessProductType, Category>().ForMember(category => category.Name, accessProductType => accessProductType.MapFrom(a => a.Kategoria));
        }
    }
}
