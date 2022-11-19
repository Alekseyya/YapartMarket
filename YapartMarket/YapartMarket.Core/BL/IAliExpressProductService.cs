using System.Collections.Generic;
using System.Threading.Tasks;
using YapartMarket.Core.DTO;
using YapartMarket.Core.DTO.AliExpress;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Core.BL
{
    public interface IAliExpressProductService
    {
        Task ProcessDataFromAliExpress();
        Task ProcessUpdateProductsSku();
        Task UpdateInventoryProducts(IEnumerable<Product> products);
        Task<IEnumerable<AliExpressProductDTO>> ExceptProductsFromDataBase(IEnumerable<AliExpressProductDTO> products);
        ProductInfoResult GetProductInfo(long productId);
        Task ProcessUpdateProducts(IReadOnlyList<long> productIds);
        Task ProcessUpdateProduct(long productId);
        Task ProcessUpdateDatabaseAliExpressProductId();
        Task<IEnumerable<Product>> ListProductsForUpdateInventory();
        Task<List<ProductInfoResult>> GetProductsFromAli(IReadOnlyList<long> productIds);
    }
}
