using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YapartMarket.Core.DTO;
using YapartMarket.Core.DTO.AliExpress;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Core.BL
{
    public interface IAliExpressProductService
    {
        Task ProcessDataFromAliExpress();
        void UpdateInventoryProducts(IEnumerable<Product> products);
        Task<IEnumerable<AliExpressProductDTO>> ExceptProductsFromDataBase(IEnumerable<AliExpressProductDTO> products);
        ProductInfoResult GetProductInfo(long productId);
        Task ProcessUpdateDatabaseAliExpressProductId();
        Task<IEnumerable<Product>> ListProductsForUpdateInventory();
    }
}
