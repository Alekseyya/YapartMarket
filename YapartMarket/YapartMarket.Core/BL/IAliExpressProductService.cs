using System.Collections.Generic;
using YapartMarket.Core.DTO;

namespace YapartMarket.Core.BL
{
    public interface IAliExpressProductService
    {
        void UpdateInventoryProducts(List<long> productIds);
        IEnumerable<AliExpressProductDTO> GetProducts();
        AliExpressProductDTO GetProduct(long productId);
        void UpdatePriceProduct(List<long> productIds);
        AliExpressProductDTO ProductStringToDTO(string json);
        void ProcessAliExpressProductId(List<AliExpressProductDTO> aliExpressProducts);
    }
}
