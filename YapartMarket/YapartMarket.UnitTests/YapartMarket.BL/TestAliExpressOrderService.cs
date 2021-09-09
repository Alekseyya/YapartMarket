using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using YapartMarket.BL.Implementation;
using YapartMarket.Core.Config;
using YapartMarket.Core.Extensions;

namespace YapartMarket.UnitTests.YapartMarket.BL
{
    public class TestAliExpressOrderService
    {
        private readonly IOptions<AliExpressOptions> _aliExpressOption;
        private readonly IConfiguration _configuration;
        private Mock<ILogger<AliExpressOrderService>> _mockLogger;

        public TestAliExpressOrderService()
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
            _mockLogger = new Mock<ILogger<AliExpressOrderService>>();
        }

        [Fact]
        public void TestAliExpressOrderService_QueryOrderDetail_Deserialize()
        {
            //arrange
            var aliExpressOrderService = new AliExpressOrderService(_mockLogger.Object, _aliExpressOption);
            //act
            var aliExpressOrderList = aliExpressOrderService.QueryOrderDetail(new DateTime(2021, 09,01).StartOfDay(), DateTime.Today.EndOfDay());
            //assert
            Assert.NotNull(aliExpressOrderList);
        }

        [Fact]
        public void TestAliExpressOrderService_QueryOrderDetail_ReturnEmptyOrder()
        {
            //arrange
            var aliExpressOrderService = new AliExpressOrderService(_mockLogger.Object, _aliExpressOption);
            //act
            var aliExpressOrderList = aliExpressOrderService.QueryOrderDetail(new DateTime(2021, 01, 01).StartOfDay(), new DateTime(2021, 01, 01).EndOfDay());
            //assert
            Assert.Null(aliExpressOrderList);
        }
    }
}
