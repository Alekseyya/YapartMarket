using System.Collections.Generic;
using System.Threading.Tasks;
using YapartMarket.Core.DTO;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Core.BL
{
    public interface ILogisticServiceOrderService
    {
        List<LogisticsServiceOrderResultDTO> GetLogisticServiceOrderRequest(long orderId);
        Task ProcessLogisticServiceOrderAsync(long orderId, List<LogisticsServiceOrderResultDTO> logisticsServiceOrderResultDtos);
        Task<LogisticServiceOrder> GetLogisticServiceOrder(long orderId);
    }
}
