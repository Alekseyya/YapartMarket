using System.Threading.Tasks;
using Coravel.Invocable;
using Microsoft.Extensions.Logging;
using YapartMarket.Core.BL;

namespace YapartMarket.React.Invocables
{
    public class UpdateProductIdFromAliExpressInvocable : IInvocable
    {
        private readonly IAliExpressProductService _aliExpressProductService;
        private readonly ILogger<UpdateProductIdFromAliExpressInvocable> _logger;

        public UpdateProductIdFromAliExpressInvocable(IAliExpressProductService aliExpressProductService, ILogger<UpdateProductIdFromAliExpressInvocable> logger)
        {
            _aliExpressProductService = aliExpressProductService;
            _logger = logger;
        }
        public async Task Invoke()
        {
            _logger.LogInformation("Получение данных о продуктах с AliExpress");
            await _aliExpressProductService.ProcessDataFromAliExpress();
            _logger.LogInformation("Обновление productAliExpressId в таблице Product");
            await _aliExpressProductService.ProcessUpdateDatabaseAliExpressProductId();
        }
    }
}
