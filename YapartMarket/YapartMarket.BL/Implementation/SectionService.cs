using YapartMarket.Core.BL;
using YapartMarket.Core.Data;
using YapartMarket.Core.Data.Interfaces;
using YapartMarket.Core.Models;

namespace YapartMarket.BL.Implementation
{
    public class SectionService: GenericService<Section, int, ISectionRepository>, ISectionService
    {
        public SectionService(IRepositoryFactory repositoryFactory) : base(repositoryFactory)
        {
        }
    }
}
