using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using YapartMarket.Core.BL;
using YapartMarket.Core.Data;
using YapartMarket.Core.Data.Interfaces;
using YapartMarket.Core.Data.Interfaces.Access;
using YapartMarket.Core.Models;

namespace YapartMarket.BL.Implementation
{
    public class SectionService: GenericService<Section, int, ISectionRepository>, ISectionService
    {
        private readonly IMapper _mapper;

        public SectionService(IRepositoryFactory repositoryFactory, IMapper mapper) : base(repositoryFactory)
        {
            _mapper = mapper;
        }

        public async Task AddAccessProductTypes()
        {
            //var sectionRepository = RepositoryFactory.GetRepository<ISectionRepository>();
            ////Todo переделать AccessProduct* классы вставить в конструктор просто ConnectionString
            ////var accessProductTypeRepository = new AccessProductTypeRepository(new Appse);
            //var sectionsInDataBase = sectionRepository.GetAll();
            ////var accessProductsTypeDB = await accessProductTypeRepository.GetAllAsync();
            //if (sectionRepository.GetCount() != 0)
            //    await sectionRepository.AddAsync(_mapper.Map<Section>(accessProductsTypeDB.GroupBy(x => x.KodRazd)));
            //else
            //{
            //    //Добавить только новые записи
            //    foreach (var accessProductType in accessProductsTypeDB.Where(accessProductType => !sectionsInDataBase.Any(sectionInDatabase => sectionInDatabase.AccessProductTypeId == accessProductType.Tip)))
            //    {
            //        sectionRepository.Add(_mapper.Map<Section>(accessProductType));
            //    }
            //}
        }
    }
}
