using System;
using System.Reflection;
using AutoMapper;
using YapartMarket.Core.DateStructures;
using YapartMarket.Core.Extensions;
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
                .ForMember(x => x.SendGoodsOperator, y => y.MapFrom(t => t.SendGoodsOperator))
                .ForMember(x => x.ShowStatus, y => y.MapFrom(t => t.ShowStatus))
                .ForMember(x => x.ProductId, y => y.MapFrom(t => t.ProductId))
                .ForMember(x => x.Sku, y => y.MapFrom(t => t.Product.Sku));
            CreateMap<AliExpressOrder, AliExpressOrderViewModel>()
                .ForMember(x => x.LogisticsStatus, y => y.MapFrom(t => t.LogisticsStatus))
                .ForMember(x => x.BizType, y => y.MapFrom(t => t.BizType))
                .ForMember(x => x.OrderStatus, y => y.MapFrom(t => t.OrderStatus))
                .ForMember(x => x.FundStatus, y => y.MapFrom(t => t.FundStatus))
                .ForMember(x => x.FrozenStatus, y => y.MapFrom(t => t.FrozenStatus))
                .ForMember(x => x.AliExpressOrderDetails, y => y.MapFrom(t => t.AliExpressOrderDetails));
        }
    }
}
