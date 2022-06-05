using YapartMarket.Core.BL;
using YapartMarket.Core.Data.Interfaces.Azure;

namespace YapartMarket.BL.Implementation
{
    public class AliExpressOrderDetailService : IAliExpressOrderDetailService
    {
        private readonly IAliExpressOrderRepository _aliExpressOrderRepository;

        public AliExpressOrderDetailService(IAliExpressOrderRepository aliExpressOrderRepository)
        {
            _aliExpressOrderRepository = aliExpressOrderRepository;
        }
    }
}
