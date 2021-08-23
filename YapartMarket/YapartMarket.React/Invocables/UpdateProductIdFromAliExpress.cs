using System.Threading.Tasks;
using Coravel.Invocable;
using YapartMarket.Core.BL;

namespace YapartMarket.React.Invocables
{
    public class UpdateProductIdFromAliExpress : IInvocable
    {
        private readonly IAliExpressProductService _aliExpressProductService;

        public UpdateProductIdFromAliExpress(IAliExpressProductService aliExpressProductService)
        {
            _aliExpressProductService = aliExpressProductService;
        }
        public Task Invoke()
        {
            var products = _aliExpressProductService.GetProductsAliExpress();
            _aliExpressProductService.ProcessUpdateDatabaseAliExpressProductId(products);
            return Task.CompletedTask;
        }
    }
}
