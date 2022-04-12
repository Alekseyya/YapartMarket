using System.Linq;
using System.Threading.Tasks;
using Coravel.Invocable;
using Microsoft.Extensions.Logging;
using YapartMarket.Core.BL;

namespace YapartMarket.React.Invocables
{
    public class UpdateLogisticRedefiningInvocable : IInvocable
    {
        private readonly IAliExpressLogisticRedefiningService _aliExpressLogisticRedefiningService;
        private readonly ILogger<UpdateLogisticRedefiningInvocable> _logger;

        public UpdateLogisticRedefiningInvocable(IAliExpressLogisticRedefiningService aliExpressLogisticRedefiningService, ILogger<UpdateLogisticRedefiningInvocable> logger)
        {
            _aliExpressLogisticRedefiningService = aliExpressLogisticRedefiningService;
            _logger = logger;
        }
        public async Task Invoke()
        {
            _logger.LogInformation("Получение информации о логистических сервисах, для всех заказов.");
            var logisticRedefiningServices = _aliExpressLogisticRedefiningService.LogisticsRedefiningListLogisticsServiceRequest();
            if (logisticRedefiningServices.Any())
                await _aliExpressLogisticRedefiningService.ProcessLogisticRedefining(logisticRedefiningServices);
        }
    }
}
