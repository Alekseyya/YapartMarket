using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AutoMapper;
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
    public class OrderSizeCargoPlaceServiceTests
    {
        private readonly IConfiguration _configuration;
        private readonly IOptions<AliExpressOptions> _aliExpressOption;
        private readonly Mock<IAliExpressOrderSizeCargoPlaceRepository> _mockOrderSizeCargoPlaceRepository;
        private readonly Mock<IMapper> _mockMapper;

        public OrderSizeCargoPlaceServiceTests()
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
            _mockOrderSizeCargoPlaceRepository = new Mock<IAliExpressOrderSizeCargoPlaceRepository>();
        }
        [Fact]
        public void GetRequest_OrderId_Success()
        {
            //Arrange
            var orderId = 5029384194863751;
            var orderSizeCargoPlaceService = new AliExpressOrderSizeCargoPlaceService(_aliExpressOption, _mockMapper.Object, _mockOrderSizeCargoPlaceRepository.Object);
            //Act
            var result = orderSizeCargoPlaceService.GetRequest(orderId);
            //Assert
            Assert.NotNull(result);
        }
    }
}
