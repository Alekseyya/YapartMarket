using AutoMapper;
using YapartMarket.Core.DTO;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Core.Mapper
{
    public sealed class AliExpressLogisticRedefiningProfile : Profile
    {
        public AliExpressLogisticRedefiningProfile()
        {
            CreateMap<AliExpressOrderLogisticDTO, AliExpressOrderLogisticRedefining>();
        }
    }
}
