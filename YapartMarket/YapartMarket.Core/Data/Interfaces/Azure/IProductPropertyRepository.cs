using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Core.Data.Interfaces.Azure
{
    public interface IProductPropertyRepository : IAzureQueriesGenericRepository<ProductProperty>, IAzureCommandGenericRepository<ProductProperty>
    {
    }
}
