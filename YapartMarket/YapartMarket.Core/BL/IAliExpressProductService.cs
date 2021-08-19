using System.Collections.Generic;
using YapartMarket.Core.DTO;

namespace YapartMarket.Core.BL
{
    public interface IAliExpressProductService
    {
        void UpdateInventoryProducts(List<AliExpressProductDTO> aliExpressProducts);
        IEnumerable<AliExpressProductDTO> GetProductsAliExpress();
        AliExpressProductDTO GetProduct(long productId);
        void UpdatePriceProduct(List<long> productIds);
        AliExpressProductDTO ProductStringToDTO(string json);
        List<AliExpressProductDTO> SetInventoryFromDatabase(List<AliExpressProductDTO> aliExpressProducts);
        void ProcessUpdateDatabaseAliExpressProductId(IEnumerable<AliExpressProductDTO> aliExpressProducts);
    }
}
