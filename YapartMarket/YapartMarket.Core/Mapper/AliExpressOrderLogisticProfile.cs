using AutoMapper;
using YapartMarket.Core.DTO;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Core.Mapper
{
    public class AliExpressOrderLogisticProfile : Profile
    {
        public AliExpressOrderLogisticProfile()
        {
            CreateMap<AliExpressOrderLogisticDTO, AliExpressOrderLogistic>();
        }
    }
}
