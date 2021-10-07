using System.Threading.Tasks;
using YapartMarket.Core.DTO;

namespace YapartMarket.Core.BL
{
    public interface IAliExpressOrderReceiptInfoService
    {
        AliExpressOrderReceiptInfoDTO GetReceiptInfo(long orderId);
        Task InsertOrderReceipt(long orderId, AliExpressOrderReceiptInfoDTO orderInfoDto);
    }
}
