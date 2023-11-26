using YapartMarket.Core.Data.Interfaces.Azure;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Data.Implementation.Azure
{
    public sealed class LogisticServiceOrderRepository : AzureGenericRepository<LogisticServiceOrder>, ILogisticServiceOrderRepository
    {
        public LogisticServiceOrderRepository(string tableName, string connectionString) : base(tableName, connectionString)
        {
        }
    }
}
