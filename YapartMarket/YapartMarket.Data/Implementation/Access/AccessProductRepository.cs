using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using Dapper;
using YapartMarket.Core.AccessModels;
using YapartMarket.Core.Data.Interfaces.Access;

namespace YapartMarket.Data.Implementation.Access
{
    public class AccessProductRepository : AccessGenericRepository<AccessProduct>, IAccessProductRepository
    {
        private readonly AppSettings _appSettings;
        public AccessProductRepository(AppSettings appSettings) :base("Tovari", appSettings.ConnectionStringAccess)
        {
            _appSettings = appSettings;
        }

        public List<AccessProduct> GetInnerJoin()
        {
            var sql = "select Tovari.*, Tipi.* from Tovari INNER JOIN Tipi ON Tovari.Tip = Tipi.Tip";
            using (IDbConnection connection = new OleDbConnection(_appSettings.ConnectionAccess))
            {
                connection.Open();
                var productTypes = connection.Query<AccessProduct,AccessProductType, AccessProduct>(sql,
                    (product, productType) => {
                        product.AccessProductType = productType;
                        return product;
                    }).AsList();
                return productTypes;
            }
        }
    }
}
