using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using YapartMarket.Core.Models;
using YapartMarket.React.ViewModels;

namespace YapartMarket.React.App_Start
{
    //public class AutoMapperConfig
    //{
    //}
    public class ProductProfile : Profile
    {
        //можно поставить Reverse Mapping для обратной конвертации
        public ProductProfile()
        {
            CreateMap<Product, ProductViewModel>()
                .ForMember(prodViewModel => prodViewModel.Brand, prod => prod.MapFrom(src=>src.Brand.Name))
                .ForMember(prodViewModel => prodViewModel.Picture,  prod => prod.MapFrom(src=>src.Brand.Picture.Path));
        }
    }
}
