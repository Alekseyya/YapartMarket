using Quartz;
using System.Threading.Tasks;
using YapartMarket.Core.BL;

namespace YapartMarket.WebApi.Job
{
    public sealed class CreateLogisticOrderJob : IJob
    {
        private readonly IAliExpressOrderService orderService;

        public CreateLogisticOrderJob(IAliExpressOrderService orderService)
        {
            this.orderService = orderService;
        }
        public async Task Execute(IJobExecutionContext context) => await orderService.CreateLogisticOrderAsync();
    }
}
