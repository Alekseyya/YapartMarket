using YapartMarket.Core.BL;
using YapartMarket.Core.Data;
using YapartMarket.Core.Data.Interfaces;
using YapartMarket.Core.Models;

namespace YapartMarket.BL.Implementation
{
   public class PictureService : GenericService<Picture, int, IPictureRepository>, IPictureService
    {
        public PictureService(IRepositoryFactory repositoryFactory) : base(repositoryFactory)
        {
        }
    }
}
