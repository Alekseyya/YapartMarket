using AutoMapper;
using YapartMarket.Core.DateStructures;
using YapartMarket.Core.Models.Azure;
using YapartMarket.React.ViewModels;

namespace YapartMarket.React.Mapper
{
    public class AliExpressOrderViewModelProfile : Profile
    {
        public AliExpressOrderViewModelProfile()
        {
            CreateMap<OrderStatus, string>().ConvertUsing(src => src.ToString());
            CreateMap<AliExpressOrderDetail, AliExpressOrderDetailViewModel>()
                .ForMember(x => x.LogisticsServiceName, y => y.MapFrom(t => t.LogisticsServiceName))
                .ForMember(x => x.ProductCount, y => y.MapFrom(t => t.ProductCount))
                .ForMember(x => x.TotalProductAmount, y => y.MapFrom(t => t.TotalProductAmount))
                .ForMember(x => x.ProductUnitPrice, y => y.MapFrom(t => t.ItemPrice))
                .ForMember(x => x.ShowStatus, y => y.MapFrom(t => t.ShowStatus))
                .ForMember(x => x.ProductId, y => y.MapFrom(t => t.ProductId))
                .ForMember(x => x.Sku, y => y.MapFrom(t => t.Product.Sku));
            CreateMap<AliExpressOrder, AliExpressOrderViewModel>()
                .ForMember(x => x.OrderStatus, y => y.MapFrom(t => t.OrderStatus))
                .ForMember(x => x.FundStatus, y => y.MapFrom(t => t.PaymentStatus))
                .ForMember(x => x.AliExpressOrderDetails, y => y.MapFrom(t => t.AliExpressOrderDetails));
        }
    }
}
