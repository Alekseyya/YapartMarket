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

    public class BrandProfile : Profile
    {
        public BrandProfile()
        {
            CreateMap<BrandViewModel, Brand>()
                .ForMember(brand => brand.Name, view => view.MapFrom(v => v.Name));
        }
    }

    public class BrandViewModelProfile : Profile
    {
        public BrandViewModelProfile()
        {
            CreateMap<Brand, BrandViewModel>()
                .ForMember(brv => brv.Name, br => br.MapFrom(b=>b.Name));
        }
    }
}
