using YapartMarket.Core.BL;
using YapartMarket.Core.Data;

namespace YapartMarket.BL.Implementation
{
   public class PictureService : RepositoryAwareServiceBase, IPictureService
    {
        public PictureService(IRepositoryFactory repositoryFactory) : base(repositoryFactory)
        {
        }
    }
}
