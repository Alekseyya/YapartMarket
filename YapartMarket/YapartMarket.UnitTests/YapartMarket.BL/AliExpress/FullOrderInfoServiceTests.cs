using System.IO;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using Xunit.Abstractions;
using YapartMarket.BL.Implementation.AliExpress;
using YapartMarket.Core.BL.AliExpress;
using YapartMarket.Core.Config;

namespace YapartMarket.UnitTests.YapartMarket.BL.AliExpress
{
    public sealed class FullOrderInfoServiceTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IConfiguration _configuration;
        private readonly IOptions<AliExpressOptions> _aliExpressOption;
        private readonly Mock<ILogger<FullOrderInfoService>> _mockLogger;
        private readonly Mock<IFullOrderInfoService> _mockFullOrderInfoService;
        private readonly Mock<IMapper> _mockMapper;
        public FullOrderInfoServiceTests(ITestOutputHelper testOutputHelper)
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
            _mockLogger = new Mock<ILogger<FullOrderInfoService>>();
            _mockMapper = new Mock<IMapper>();
            _mockFullOrderInfoService = new Mock<IFullOrderInfoService>();
        }
        //[Fact]
        //public void GetRequest_OrderId_Success()
        //{
        //    //Arrange
        //    var orderId = 5029474366293600;
        //    var serviceOrderService = new FullOrderInfoService(_mockLogger.Object, _aliExpressOption, _mockMapper.Object);
        //    //Act
        //    var result = serviceOrderService.GetRequest(orderId);
        //    //Assert
        //    _testOutputHelper.WriteLine(result);
        //    Assert.NotNull(result);
        //}
    }
}
