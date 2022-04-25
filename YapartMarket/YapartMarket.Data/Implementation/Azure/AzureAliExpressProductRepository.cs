using System.Collections.Generic;
using YapartMarket.Core.Data.Interfaces.Azure;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Data.Implementation.Azure
{
    public class AzureAliExpressProductRepository : AzureGenericRepository<AliExpressProduct>, IAzureAliExpressProductRepository
    {
        public AzureAliExpressProductRepository(string tableName, string connectionString) : base(tableName, connectionString)
        {
        }
    }
}
