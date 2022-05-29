using YapartMarket.Core.DTO.AliExpress.FullOrderInfo;

namespace YapartMarket.Core.BL.AliExpress
{
    public interface IFullOrderInfoService
    {
       Root GetRequest(long orderId, long? flag = null);
    }
}
