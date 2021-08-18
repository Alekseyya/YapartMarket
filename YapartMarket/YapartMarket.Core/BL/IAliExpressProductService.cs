using System.Collections.Generic;
using YapartMarket.Core.DTO;

namespace YapartMarket.Core.BL
{
    public interface IAliExpressProductService
    {
        void UpdateInventoryProducts(List<long> productIds);
        string GetProducts(); 
        AliExpressProductDTO GetProduct(long productId);
        void UpdatePriceProduct(List<long> productIds);
    }
}
