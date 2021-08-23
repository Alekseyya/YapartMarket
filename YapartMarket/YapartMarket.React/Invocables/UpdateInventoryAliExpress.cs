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
        public Task Invoke()
        {
            _logger.LogInformation("получение продуктов для обновления");
            var products = _aliExpressProductService.GetProductsAliExpress();
            _logger.LogInformation("обновление остатков");
            _aliExpressProductService.UpdateInventoryProducts(products);
            return Task.CompletedTask;
        }
    }
}
