using AutoMapper;
using YapartMarket.Core.DTO.Yandex;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Core.Mapper
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ItemDto, Product>()
                .ForMember(product => product.Sku, itemDto => itemDto.MapFrom(x => x.Sku))
                .ForMember(product => product.Count, itemDto => itemDto.MapFrom(x => x.Count));
        }
    }
}
