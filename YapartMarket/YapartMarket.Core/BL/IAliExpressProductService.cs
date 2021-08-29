using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YapartMarket.Core.DTO;

namespace YapartMarket.Core.BL
{
    public interface IAliExpressProductService
    {
        Task ProcessDataFromAliExpress();
        void UpdateInventoryProducts(IEnumerable<AliExpressProductDTO> aliExpressProducts);
        Task<IEnumerable<AliExpressProductDTO>> ExceptProductsFromDataBase(IEnumerable<AliExpressProductDTO> products);
        IEnumerable<AliExpressProductDTO> GetProductsAliExpress(Expression<Func<AliExpressProductDTO, bool>> conditionFunction = null);
        AliExpressProductDTO GetProduct(long productId);
        void UpdatePriceProduct(List<long> productIds);
        AliExpressProductDTO ProductStringToDTO(string json);
        List<AliExpressProductDTO> SetInventoryFromDatabase(List<AliExpressProductDTO> aliExpressProducts);
        void ProcessUpdateDatabaseAliExpressProductId(IEnumerable<AliExpressProductDTO> aliExpressProducts);
    }
}
