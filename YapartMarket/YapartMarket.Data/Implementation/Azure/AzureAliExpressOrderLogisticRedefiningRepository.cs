using System;
using System.Collections.Generic;
using System.Text;
using YapartMarket.Core.Data.Interfaces.Azure;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Data.Implementation.Azure
{
    public class AzureAliExpressOrderLogisticRedefiningRepository : AzureGenericRepository<AliExpressOrderLogisticRedefining>, IAzureAliExpressOrderLogisticRedefiningRepository
    {
        public AzureAliExpressOrderLogisticRedefiningRepository(string tableName, string connectionString) : base(tableName, connectionString)
        {
        }
    }
}
