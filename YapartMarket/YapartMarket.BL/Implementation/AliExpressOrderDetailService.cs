using YapartMarket.Core.BL;
using YapartMarket.Core.Data.Interfaces.Azure;

namespace YapartMarket.BL.Implementation
{
    public class AliExpressOrderDetailService : IAliExpressOrderDetailService
    {
        private readonly IAzureAliExpressOrderRepository _azureAliExpressOrderRepository;

        public AliExpressOrderDetailService(IAzureAliExpressOrderRepository azureAliExpressOrderRepository)
        {
            _azureAliExpressOrderRepository = azureAliExpressOrderRepository;
        }
    }
}
