using System.Collections.Generic;
using System.Threading.Tasks;
using YapartMarket.Core.DTO;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Core.BL
{
   public interface IAliExpressLogisticOrderDetailService
   {
       List<AliExpressLogisticsOrderDetailDto> GetLogisticOrderDetailRequest(long orderId);
       Task ProcessLogisticsOrderDetailAsync(List<AliExpressLogisticsOrderDetailDto> aliExpressLogisticsOrderDetailDtos);
       Task<AliExpressLogisticOrderDetail> GetDetail(long orderId);
   }
}
