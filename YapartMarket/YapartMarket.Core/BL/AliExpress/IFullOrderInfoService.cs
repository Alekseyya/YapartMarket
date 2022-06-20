using System.Threading.Tasks;
using YapartMarket.Core.DTO.AliExpress.FullOrderInfo;

namespace YapartMarket.Core.BL.AliExpress
{
    public interface IFullOrderInfoService
    {
        Task<Root> GetRequest(long orderId, long? flag = null);
    }
}
