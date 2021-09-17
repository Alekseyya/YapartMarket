using AutoMapper;
using YapartMarket.Core.Models.Azure;
using YapartMarket.React.ViewModels;

namespace YapartMarket.React.Mapper
{
    public class AliExpressOrderViewModelProfile : Profile
    {
        public AliExpressOrderViewModelProfile()
        {
            CreateMap<AliExpressOrderDetail, AliExpressOrderDetailViewModel>()
                .ForMember(x => x.LogisticsServiceName, y => y.MapFrom(t => t.LogisticsServiceName))
                .ForMember(x => x.SendGoodsOperator, y => y.MapFrom(t => nameof(t.SendGoodsOperator)))
                .ForMember(x => x.ShowStatus, y => y.MapFrom(t => nameof(t.ShowStatus)));
            CreateMap<AliExpressOrder, AliExpressOrderViewModel>()
                .ForMember(x => x.LogisticsStatus, y => y.MapFrom(t => nameof(t.LogisticsStatus)))
                .ForMember(x => x.BizType, y => y.MapFrom(t => nameof(t.BizType)))
                .ForMember(x => x.OrderStatus, y => y.MapFrom(t => nameof(t.OrderStatus)))
                .ForMember(x => x.FundStatus, y => y.MapFrom(t => nameof(t.FundStatus)))
                .ForMember(x => x.FrozenStatus, y => y.MapFrom(t => nameof(t.FrozenStatus)))
                .ForMember(x => x.AliExpressOrderDetails, y => y.MapFrom(t => t.AliExpressOrderDetails));
        }
    }
}
