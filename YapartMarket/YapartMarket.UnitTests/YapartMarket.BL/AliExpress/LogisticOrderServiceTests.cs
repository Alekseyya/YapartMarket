using System.IO;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using YapartMarket.BL.Implementation;
using YapartMarket.Core.Config;
using YapartMarket.Core.Data.Interfaces.Azure;

namespace YapartMarket.UnitTests.YapartMarket.BL.AliExpress
{
    public class LogisticOrderServiceTests
    {
        private readonly IConfiguration _configuration;
        private readonly IOptions<AliExpressOptions> _aliExpressOption;
        private readonly Mock<ILogger<LogisticServiceOrderService>> _mockLogger;
        private readonly Mock<ILogisticServiceOrderRepository> _mockLogisticServiceOrderRepository;
        private readonly Mock<IMapper> _mockMapper;

        public LogisticOrderServiceTests()
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
            _mockLogger = new Mock<ILogger<LogisticServiceOrderService>>();
            _mockMapper = new Mock<IMapper>();
            _mockLogisticServiceOrderRepository = new Mock<ILogisticServiceOrderRepository>();
        }
        [Fact]
        public void GetLogisticServiceOrderRequest_OrderId_Success()
        {
            //Arrange
            var orderId = 5029384194863751;
            var logisticServiceOrderService = new LogisticServiceOrderService(_mockLogger.Object, _aliExpressOption, _mockMapper.Object, _mockLogisticServiceOrderRepository.Object);
            //Act
            var result = logisticServiceOrderService.GetLogisticServiceOrderRequest(orderId);
            //Assert
            Assert.NotNull(result);
        }
    }
}
