using System.Collections.Generic;
using System.Threading.Tasks;
using YapartMarket.Core.DTO;
using YapartMarket.Core.Models.Raw;
using Product = YapartMarket.Core.Models.Azure.Product;


namespace YapartMarket.Core.BL
{
    public interface IAliExpressProductService
    {
        Task<UpdateStocksResponse> ProcessUpdateStocksAsync();
        Task<IEnumerable<AliExpressProductDTO>> ExceptProductsFromDataBaseAsync(IEnumerable<AliExpressProductDTO> products);
        Task ProcessUpdateDatabaseAliExpressProductIdAsync();
        Task<IEnumerable<Product>> ListProductsForUpdateInventoryAsync();
        Task ProcessUpdateProductSkuAsync();
    }
}
