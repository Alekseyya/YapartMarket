using AutoMapper;
using YapartMarket.Core.DTO;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Core.Mapper
{
    public sealed class AliExpressLogisticOrderDetailProfile : Profile
    {
        public AliExpressLogisticOrderDetailProfile()
        {
            CreateMap<AliExpressLogisticsOrderDetailDto, AliExpressLogisticOrderDetail>()
                .ForMember(x => x.OrderId, t => t.MapFrom(y => y.TradeOrderId))
                .ForMember(x => x.LogisticOrderId, t => t.MapFrom(y => y.LogisticsOrderId))
                .ForMember(x => x.OutOrderCode, t => t.MapFrom(y => y.OutOrderCode))
                .ForMember(x => x.GmtCreate, t => t.MapFrom(y => y.GmtCreate))
                .ForMember(x => x.LogisticStatus, t => t.MapFrom(y => y.LogisticStatus));
        }
    }
}
