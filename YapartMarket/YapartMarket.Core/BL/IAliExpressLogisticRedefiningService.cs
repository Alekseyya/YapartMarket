using System.Collections.Generic;
using System.Threading.Tasks;
using YapartMarket.Core.DTO;

namespace YapartMarket.Core.BL
{
    public interface IAliExpressLogisticRedefiningService
    {
        List<AliExpressOrderLogisticDTO> LogisticsRedefiningListLogisticsServiceRequest();
        Task ProcessLogisticRedefining(List<AliExpressOrderLogisticDTO> aliExpressOrderLogisticDtos);
    }
}
