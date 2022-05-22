using System.IO;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using YapartMarket.BL.Implementation.AliExpress;
using YapartMarket.Core.Config;

namespace YapartMarket.UnitTests.YapartMarket.BL.AliExpress
{
    public class RedefiningDomesticLogisticsCompanyServiceTests
    {
        private readonly IConfiguration _configuration;
        private readonly IOptions<AliExpressOptions> _aliExpressOption;
        private readonly Mock<ILogger<RedefiningDomesticLogisticsCompanyService>> _mockLogger;
        private readonly Mock<IMapper> _mockMapper;

        public RedefiningDomesticLogisticsCompanyServiceTests()
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
            _mockLogger = new Mock<ILogger<RedefiningDomesticLogisticsCompanyService>>();
            _mockMapper = new Mock<IMapper>();
        }
        [Fact]
        public void GetRequest_Deserialize_Success()
        {
            //Arrange
            var redefiningDomesticLogisticsCompanyService = new RedefiningDomesticLogisticsCompanyService(_mockLogger.Object, _aliExpressOption, _mockMapper.Object);
            //Act
            var result = redefiningDomesticLogisticsCompanyService.GetRequest();
            //Assert
            Assert.NotNull(result);
        }
    }
}
