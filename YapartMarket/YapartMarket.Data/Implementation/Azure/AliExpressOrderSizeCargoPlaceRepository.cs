﻿using YapartMarket.Core.Data.Interfaces.Azure;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Data.Implementation.Azure
{
    public class AliExpressOrderSizeCargoPlaceRepository :  AzureGenericRepository<AliExpressExpressOrderSizeCargoPlace>, IAliExpressOrderSizeCargoPlaceRepository
    {
        public AliExpressOrderSizeCargoPlaceRepository(string tableName, string connectionString) : base(tableName, connectionString)
        {
        }
    }
}
