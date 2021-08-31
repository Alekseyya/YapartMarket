﻿using System.Threading.Tasks;
using Coravel.Invocable;
using Microsoft.Extensions.Logging;
using YapartMarket.Core.BL;

namespace YapartMarket.React.Invocables
{
    public class UpdateProductIdFromAliExpress : IInvocable
    {
        private readonly IAliExpressProductService _aliExpressProductService;
        private readonly ILogger<UpdateProductIdFromAliExpress> _logger;

        public UpdateProductIdFromAliExpress(IAliExpressProductService aliExpressProductService, ILogger<UpdateProductIdFromAliExpress> logger)
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
