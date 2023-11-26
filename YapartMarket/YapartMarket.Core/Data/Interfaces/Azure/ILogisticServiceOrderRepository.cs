using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Core.Data.Interfaces.Azure
{
    public interface ILogisticServiceOrderRepository : IAzureQueriesGenericRepository<LogisticServiceOrder>, IAzureCommandGenericRepository<LogisticServiceOrder>
    {
    }
}
