using System.IO;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;
using YapartMarket.BL.Implementation.AliExpress;
using YapartMarket.Core.Config;
using YapartMarket.Core.Data.Interfaces.Azure;

namespace YapartMarket.UnitTests.YapartMarket.BL.AliExpress
{
    public class OrderSizeCargoPlaceServiceTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IConfiguration _configuration;
        private readonly IOptions<AliExpressOptions> _aliExpressOption;
        private readonly Mock<IAliExpressOrderSizeCargoPlaceRepository> _mockOrderSizeCargoPlaceRepository;
        private readonly Mock<IMapper> _mockMapper;

        public OrderSizeCargoPlaceServiceTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
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
            _mockMapper = new Mock<IMapper>();
            _mockOrderSizeCargoPlaceRepository = new Mock<IAliExpressOrderSizeCargoPlaceRepository>();
        }
        [Fact]
        public void GetRequest_OrderId_Success()
        {
            //Arrange
            var orderId = 5029474366293600;
            var orderSizeCargoPlaceService = new OrderSizeCargoPlaceService(_aliExpressOption, _mockMapper.Object, _mockOrderSizeCargoPlaceRepository.Object);
            //Act
            var result = orderSizeCargoPlaceService.GetRequest(orderId);
            //Assert
            _testOutputHelper.WriteLine(JsonConvert.SerializeObject(result));
            Assert.NotNull(result);
        }
        [Fact]
        public void CreateLogisticsServicesId_CreateString_Success()
        {
            //Arrange
            var orderId = 5029408847452678;
            var orderSizeCargoPlaceService = new OrderSizeCargoPlaceService(_aliExpressOption, _mockMapper.Object, _mockOrderSizeCargoPlaceRepository.Object);
            var response = orderSizeCargoPlaceService.GetRequest(orderId);
            //Act
            var result = orderSizeCargoPlaceService.CreateLogisticsServicesId(response);
            //Assert
            _testOutputHelper.WriteLine(JsonConvert.SerializeObject(result));
            Assert.Contains(result, ";");
        }
    }
}
