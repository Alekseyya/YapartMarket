﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YapartMarket.Core.Data.Interfaces.Azure
{
    public interface IAzureCommandGenericRepository<T>
    {
        Task InsertAsync(string sql, IEnumerable<object> inserts);
        Task<IEnumerable<int>> InsertOutputAsync(string sql, IEnumerable<object> inserts);
        Task Update(string sql, object action);
    }
}
