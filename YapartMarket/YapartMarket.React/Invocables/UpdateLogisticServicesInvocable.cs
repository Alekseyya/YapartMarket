using System.Linq;
using System.Threading.Tasks;
using Coravel.Invocable;
using Microsoft.Extensions.Logging;
using YapartMarket.Core.BL;

namespace YapartMarket.React.Invocables
{
    public class UpdateLogisticServicesInvocable : IInvocable
    {
        private readonly IAliExpressLogisticRedefiningService _aliExpressLogisticRedefiningService;
        private readonly ILogger<UpdateLogisticServicesInvocable> _logger;

        public UpdateLogisticServicesInvocable(IAliExpressLogisticRedefiningService aliExpressLogisticRedefiningService,
            ILogger<UpdateLogisticServicesInvocable> logger)
        {
            _aliExpressLogisticRedefiningService = aliExpressLogisticRedefiningService;
            _logger = logger;
        }
        public async Task Invoke()
        {
            _logger.LogInformation("Получение информации о логистических сервисах.");
           var logisticRedefiningServices = _aliExpressLogisticRedefiningService.LogisticsRedefiningListLogisticsServiceRequest();
           if (logisticRedefiningServices.Any())
           {
               await _aliExpressLogisticRedefiningService.ProcessLogisticRedefining(logisticRedefiningServices);
           }
        }
    }
}
