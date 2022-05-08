using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using Xunit;
using YapartMarket.BL.Implementation;
using YapartMarket.Core.BL;
using YapartMarket.Core.Config;
using YapartMarket.Core.Data.Interfaces.Azure;
using YapartMarket.Core.Mapper;

namespace YapartMarket.UnitTests.YapartMarket.BL
{
    public class TestAliExpressCategoryService
    {
        private readonly IOptions<AliExpressOptions> _aliExpressOption;
        private readonly IConfiguration _configuration;
        private Mock<IMapper> _mockMapper;
        private IMapper _mapper;
        private Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly Mock<IAzureAliExpressProductRepository> _mockProductRepository;
        private readonly Mock<IAliExpressProductService> _mockProductService;
        private readonly IOptions<Connections> _connections;

        public TestAliExpressCategoryService()
        {
            _configuration = (IConfiguration)new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true).Build();
            _aliExpressOption = Options.Create(new AliExpressOptions()
            {
                AppKey = _configuration["AliExpress:AppKey"],
                AppSecret = _configuration["AliExpress:AppSecret"],
                AccessToken = _configuration["AliExpress:AccessToken"],
                HttpsEndPoint = _configuration["AliExpress:HttpsEndPoint"],
            });
            _connections = Options.Create(new Connections()
            {
                SQLServerConnectionString = _configuration["ConnectionStrings:SQLServerConnectionString"]
            });
            _mockMapper = new Mock<IMapper>();
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _mockProductRepository = new Mock<IAzureAliExpressProductRepository>();
            _mockProductService = new Mock<IAliExpressProductService>();

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AliExpressOrderProfile());
            });
            _mapper = mockMapper.CreateMapper();
        }
        [Fact]
        public async Task QueryCategoryThreeAsync_CallWithCorrectValue_ReturnCategory()
        {
            //Arrange
            var categoryId = 200003311;
            var aliCategoryService = new AliExpressCategoryService(_mapper,_connections, _aliExpressOption, _mockCategoryRepository.Object, _mockProductRepository.Object, _mockProductService.Object);
            //Act
            var result = await aliCategoryService.QueryCategoryAsync(categoryId);
            //Assert
            Assert.NotNull(result);
        }
    }
}
