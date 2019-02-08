using YapartMarket.Core.BL;
using YapartMarket.Core.Data;

namespace YapartMarket.BL.Implementation
{
   public class GroupService : RepositoryAwareServiceBase, IGroupService
    {
        public GroupService(IRepositoryFactory repositoryFactory) : base(repositoryFactory)
        {
        }
    }
}
