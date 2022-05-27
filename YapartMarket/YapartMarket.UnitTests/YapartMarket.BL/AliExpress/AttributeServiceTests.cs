using System.IO;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using YapartMarket.BL.Implementation.AliExpress;
using YapartMarket.Core.Config;

namespace YapartMarket.UnitTests.YapartMarket.BL.AliExpress
{
    public sealed class AttributeServiceTests
    {
        private readonly IConfiguration _configuration;
        private readonly IOptions<AliExpressOptions> _aliExpressOption;
        private readonly Mock<IMapper> _mockMapper;

        public AttributeServiceTests()
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
        public void GetRequest_CN_Success()
        {
            //Arrange
            var categoryId = 200000466;
            var location = "zh_CN";
            var domesticLogisticsCompanyService = new AttributeService(_aliExpressOption, _mockMapper.Object);
            //Act
            var result = domesticLogisticsCompanyService.GetRequest(categoryId, location);
            //Assert
            Assert.NotNull(result);
        }
    }
}
