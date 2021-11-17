using AutoMapper;
using YapartMarket.Core.DTO;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Core.Mapper
{
    public sealed class AliExpressLogisticOrderDetailProfile : Profile
    {
        public AliExpressLogisticOrderDetailProfile()
        {
            CreateMap<AliExpressLogisticsOrderDetailDto, AliExpressLogisticOrderDetail>();
        }
    }
}
