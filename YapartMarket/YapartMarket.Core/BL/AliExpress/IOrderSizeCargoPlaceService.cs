using System.Collections.Generic;
using YapartMarket.Core.DTO;

namespace YapartMarket.Core.BL
{
    public interface IOrderSizeCargoPlaceService
    {
        List<AliExpressOrderSizeCargoPlaceDTO> GetRequest(long orderId);
        string CreateLogisticsServicesId(List<AliExpressOrderSizeCargoPlaceDTO> orderSizeCargoPlaces);
    }
}
