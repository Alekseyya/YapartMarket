using YapartMarket.Core.BL;
using YapartMarket.Core.Data;
using YapartMarket.Core.Data.Interfaces;
using YapartMarket.Core.Models;

namespace YapartMarket.BL.Implementation
{
   public class GroupService : GenericService<Group, int, IGroupRepository>, IGroupService
    {
        public GroupService(IRepositoryFactory repositoryFactory) : base(repositoryFactory)
        {
        }
    }
}
