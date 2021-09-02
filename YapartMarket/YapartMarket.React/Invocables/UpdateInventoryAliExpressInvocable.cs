using System;
using System.Threading.Tasks;
using Coravel.Invocable;
using Microsoft.Extensions.Logging;
using YapartMarket.Core.BL;

namespace YapartMarket.React.Invocables
{
    public class UpdateInventoryAliExpressInvocable : IInvocable
    {
        private readonly IAliExpressProductService _aliExpressProductService;
        private readonly ILogger<UpdateInventoryAliExpressInvocable> _logger;

        public UpdateInventoryAliExpressInvocable(IAliExpressProductService aliExpressProductService, ILogger<UpdateInventoryAliExpressInvocable> logger)
        {
            _aliExpressProductService = aliExpressProductService;
            _logger = logger;
        }
        public async Task Invoke()
        {
            Console.WriteLine("получение продуктов для обновления");
            _logger.LogInformation("получение продуктов для обновления!");
            var products = await _aliExpressProductService.ListProductsForUpdateInventory();
            Console.WriteLine("обновление остатков");
            _logger.LogInformation("обновление остатков!");
            _aliExpressProductService.UpdateInventoryProducts(products);
        }
    }
}
