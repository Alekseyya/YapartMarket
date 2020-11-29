using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using YapartMarket.Core.AccessModels;
using YapartMarket.Core.Data.Interfaces.Access;

namespace YapartMarket.Data.Implementation.Access
{
    public class AccessProductRepository : IAccessProductRepository<AccessProduct>
    {
        private readonly AppSettings _appSettings;

        public AccessProductRepository(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public async Task<List<AccessProduct>> Get()
        {
            var sql = "select * from Tovari";
            using (var connection = new OleDbConnection(_appSettings.ConnectionAccess))
            {
                connection.Open();
                var accessProducts = await connection.QueryAsync<AccessProduct>(sql);
                return (List<AccessProduct>)accessProducts;
            }
        }

        public List<AccessProduct> GetInnerJoin()
        {
            var sql = "select ti.*, to.* from Tipi ti inner join Tovari to on ti.Tip = to.Tip";
            using (IDbConnection connection = new OleDbConnection(_appSettings.ConnectionAccess))
            {
                connection.Open();
                var productTypes = connection.Query<AccessProduct,AccessProductType, AccessProduct>(sql,
                    (product, productType) => {
                        product.AccessProductType = productType;
                        return product;
                    }).AsList();
                return (List<AccessProduct>)productTypes;
            }
        }
    }
}
