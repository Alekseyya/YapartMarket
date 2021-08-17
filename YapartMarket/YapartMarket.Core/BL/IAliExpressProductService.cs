using System.Collections.Generic;

namespace YapartMarket.Core.BL
{
    public interface IAliExpressProductService
    {
        void UpdateInventoryProducts(List<long> productIds);
        string GetProducts();
        string GetProduct(long productId);
        void UpdatePriceProduct(List<long> productIds);
    }
}
