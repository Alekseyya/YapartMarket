using Quartz;
using System.Threading.Tasks;
using YapartMarket.Core.BL;

namespace YapartMarket.WebApi.Job
{
    public sealed class UpdateInventoryJob : IJob
    {
        private readonly IAliExpressProductService _productService;

        public UpdateInventoryJob(IAliExpressProductService productService)
        {
            _productService = productService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            await _productService.ProcessUpdateStocks();
        }
    }
}
