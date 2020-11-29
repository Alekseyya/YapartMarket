using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Threading.Tasks;
using YapartMarket.Core.AccessModels;
using YapartMarket.Core.Data.Interfaces.Access;

namespace YapartMarket.Data.Implementation.Access
{
    public class AccessProductTypeRepository : IAccessProductTypeRepository<AccessProductType>
    {
        private readonly AppSettings _appSettings;
        public AccessProductTypeRepository(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }
        public async Task<List<AccessProductType>> Get()
        {
            var sql = "select * from Tipi";
            using (var connection = new OleDbConnection(_appSettings.ConnectionStringAccess))
            {
                connection.Open();
                var accessProducts = await connection.QueryAsync<AccessProductType>(sql);
                return (List<AccessProductType>)accessProducts;
            }
        }
        public List<AccessProductType> GetInnerJoin()
        {
            //var sql = "select ti.*, to.* from Tipi ti inner join Tovari to on ti.Tip = to.Tip";
            var sql = "select Tipi.*, Tovari.* from Tipi INNER JOIN Tovari ON Tipi.Tip = Tovari.Tip";
            using (IDbConnection connection = new OleDbConnection(_appSettings.ConnectionStringAccess))
            {
                connection.Open();
                var productTypes =  connection.Query<AccessProductType, AccessProduct, AccessProductType>(sql,
                    (productType, product) => { product.AccessProductType = productType;
                        return productType;
                    }, splitOn: "Tipi.Tip, Tovari.Tip").AsList();
                //connection.QueryMultiple<AccessProduct, AccessProductType>(sql, (a, b) => { return b; });
                //var a = connection.Query<AccessProductType, AccessProduct>(sql,(productType, product) => { return product;});
                return productTypes;
            }
        }
    }
}
