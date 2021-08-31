using System.Threading.Tasks;
using Coravel.Invocable;
using Microsoft.Extensions.Logging;
using YapartMarket.Core.BL;

namespace YapartMarket.React.Invocables
{
    public class UpdateInventoryAliExpress : IInvocable
    {
        private readonly IAliExpressProductService _aliExpressProductService;
        private readonly ILogger<UpdateInventoryAliExpress> _logger;

        public UpdateInventoryAliExpress(IAliExpressProductService aliExpressProductService, ILogger<UpdateInventoryAliExpress> logger)
        {
            _aliExpressProductService = aliExpressProductService;
            _logger = logger;
        }
        public async Task Invoke()
        {
            _logger.LogInformation("получение продуктов для обновления");
            var products = await _aliExpressProductService.ListProductsForUpdateInventory();
            _logger.LogInformation("обновление остатков");
            _aliExpressProductService.UpdateInventoryProducts(products);
        }
    }
}
