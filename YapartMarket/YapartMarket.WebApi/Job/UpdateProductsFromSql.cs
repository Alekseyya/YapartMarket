using Quartz;
using System.Threading.Tasks;
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

        public Task Execute(IJobExecutionContext context)
        {
            throw new System.NotImplementedException();
        }
        //public async Task Execute(IJobExecutionContext context)
        //{
        //    //await _productService.UpdateProductFromSql();
        //}
    }
}
