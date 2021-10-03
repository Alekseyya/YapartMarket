using YapartMarket.Core.DTO;

namespace YapartMarket.Core.BL
{
    public interface IAliExpressOrderReceiptInfoService
    {
        AliExpressOrderReceiptInfoDTO GetReceiptInfo(long orderId);
    }
}
