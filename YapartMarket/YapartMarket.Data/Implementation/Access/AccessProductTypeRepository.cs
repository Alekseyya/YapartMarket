using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using YapartMarket.Core.AccessModels;
using YapartMarket.Core.Data.Interfaces.Access;

namespace YapartMarket.Data.Implementation.Access
{
    public class AccessProductTypeRepository : AccessGenericRepository<AccessProductType>, IAccessProductTypeRepository
    {
        private readonly AppSettings _appSettings;
        public AccessProductTypeRepository(AppSettings appSettings) : base("Tipi", appSettings.ConnectionStringAccess)
        {
            _appSettings = appSettings;
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
