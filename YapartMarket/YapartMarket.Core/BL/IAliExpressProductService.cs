using System.Collections.Generic;
using System.Threading.Tasks;
using YapartMarket.Core.DTO;
using YapartMarket.Core.Models.Raw;
using Product = YapartMarket.Core.Models.Azure.Product;


namespace YapartMarket.Core.BL
{
    public interface IAliExpressProductService
    {
        Task<UpdateStocksResponse> ProcessUpdateStocks();
        Task<IEnumerable<AliExpressProductDTO>> ExceptProductsFromDataBase(IEnumerable<AliExpressProductDTO> products);
        Task ProcessUpdateDatabaseAliExpressProductId();
        Task<IEnumerable<Product>> ListProductsForUpdateInventory();
        Task ProcessUpdateProductSku();
    }
}
