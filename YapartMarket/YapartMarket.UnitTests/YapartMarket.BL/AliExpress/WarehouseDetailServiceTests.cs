using System.IO;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using YapartMarket.BL.Implementation;
using YapartMarket.BL.Implementation.AliExpress;
using YapartMarket.Core.Config;

namespace YapartMarket.UnitTests.YapartMarket.BL.AliExpress
{
    public class WarehouseDetailServiceTests
    {
        private readonly IConfiguration _configuration;
        private readonly IOptions<AliExpressOptions> _aliExpressOption;
        private readonly Mock<IMapper> _mockMapper;

        public WarehouseDetailServiceTests()
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
            _mockMapper = new Mock<IMapper>();
        }
        [Fact]
        public void GetRequest_Get_Success()
        {
            //Arrange
            var orderId = 5029384194863751;
            var warehouseDetailService = new WarehouseDetailService(_aliExpressOption, _mockMapper.Object);
            //Act
            warehouseDetailService.GetRequest(orderId);
            //Assert
            //Assert.NotNull(result);
        }
    }
}
