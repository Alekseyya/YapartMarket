using System.Collections.Generic;
using System.Threading.Tasks;
using YapartMarket.Core.BL;
using YapartMarket.Core.Data.Interfaces.Azure;
using YapartMarket.Core.Models.Azure;

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
