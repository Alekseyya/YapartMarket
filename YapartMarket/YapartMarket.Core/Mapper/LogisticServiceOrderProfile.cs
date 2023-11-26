using AutoMapper;
using YapartMarket.Core.DTO;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Core.Mapper
{
    public class LogisticServiceOrderProfile : Profile
    {
        public LogisticServiceOrderProfile()
        {
            CreateMap<LogisticsServiceOrderResultDTO, LogisticServiceOrder>();
        }
    }
}
