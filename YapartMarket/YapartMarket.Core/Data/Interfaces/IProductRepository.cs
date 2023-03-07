using System.Threading.Tasks;
using YapartMarket.Core.Models;

namespace YapartMarket.Core.Data.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product, int>
    {
    }
}
