using Quartz;
using YapartMarket.Core.BL;

namespace YapartMarket.WebApi.Job
{
    public sealed class UpdateProductsFromSql : IJob
    {
        private readonly IAliExpressProductService _productService;

        public UpdateProductsFromSql(IAliExpressProductService productService)
        {
            _productService = productService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            //await _productService.UpdateProductFromSql();
        }
    }
}
