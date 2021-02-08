using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;
using YapartMarket.BL.Implementation;
using YapartMarket.Core.AccessModels;
using YapartMarket.Core.Data;
using YapartMarket.Core.Data.Interfaces;
using YapartMarket.Core.Data.Interfaces.Access;
using YapartMarket.Core.Models;
using YapartMarket.Data;
using YapartMarket.Data.Implementation;

namespace YapartMarket.UnitTests.YapartMarket.BL
{
    public class TestSectionService
    {
        private readonly Mock<IRepositoryFactory> _mockRepositoryFactory;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ISectionRepository> _mockSectionRepository;
        private readonly Mock<IAccessProductTypeRepository> _mockAccessProductTypeRepository;
        private readonly string _connectionString;
        public TestSectionService()
        {
            var config = (IConfiguration)new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true).Build();
            _connectionString = config["Connection:DbConnectionString"];

            _mockAccessProductTypeRepository = new Mock<IAccessProductTypeRepository>(MockBehavior.Strict);
            _mockSectionRepository = new Mock<ISectionRepository>(MockBehavior.Strict);
            _mockRepositoryFactory = new Mock<IRepositoryFactory>(MockBehavior.Strict);
            _mockRepositoryFactory.Setup(x => x.GetRepository<ISectionRepository>()).Returns(_mockSectionRepository.Object);
            _mockMapper = new Mock<IMapper>(MockBehavior.Strict);
        }

        [Fact]
        public async void Test_AddAccessProductTypes_AddSections()
        {
            //Arrange
            var sectionService = new SectionService(_mockRepositoryFactory.Object, _mockMapper.Object);
            _mockRepositoryFactory.Setup(x=>x.GetRepository<IAccessProductTypeRepository>()).Returns(_mockAccessProductTypeRepository.Object);
            _mockSectionRepository.Setup(x => x.GetAll()).Returns(new List<Section>());
            _mockAccessProductTypeRepository.Setup(x => x.GetAllAsync()).Returns(Task.FromResult(new List<AccessProductType>().AsEnumerable()));

            //Act
            await sectionService.AddAccessProductTypes();

            //Assert
        }

        //Проверка записалась ли запись в бд
        [Fact]
        public async void Test_AddAcessProductType_AddSectionInDb()
        {
            var ob = new DbContextOptionsBuilder<YapartContext>()
                .UseNpgsql(_connectionString).Options;
            var dbAccessor = (IYapartDbAccessor)new YapartSingletonDbAccessor(ob);
            var repositoryFactory = (IRepositoryFactory) new YapartRepositoryFactory(dbAccessor);

            var sectionService = new SectionService(repositoryFactory, null);
            await sectionService.AddAccessProductTypes();


        }


    }
}
