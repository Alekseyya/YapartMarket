using Quartz;
using YapartMarket.Core.BL;

namespace YapartMarket.WebApi.Job
{
    public sealed class UpdateInventoryJon : IJob
    {
        private readonly IAliExpressProductService _productService;

        public UpdateInventoryJon(IAliExpressProductService productService)
        {
            _productService = productService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            await _productService.ProcessUpdateStocks();
        }
    }
}
