using System.Collections.Generic;
using System.Threading.Tasks;
using YapartMarket.Core.BL.Queries;
using YapartMarket.Core.DTO;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Core.BL
{
    public interface IAliExpressLogisticRedefiningService : IAliExpressOrderQueries
    {
        List<AliExpressOrderLogisticDTO> LogisticsRedefiningListLogisticsServiceRequest();
        Task ProcessLogisticRedefining(List<AliExpressOrderLogisticDTO> aliExpressOrderLogisticDtos);
        Task<AliExpressOrderLogisticRedefining> GetRedefining(long orderId);
    }
}
