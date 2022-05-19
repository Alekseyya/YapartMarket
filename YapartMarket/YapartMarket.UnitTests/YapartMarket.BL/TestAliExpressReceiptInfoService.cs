using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using YapartMarket.BL.Implementation;
using YapartMarket.Core.BL;
using YapartMarket.Core.Config;
using YapartMarket.Core.Data.Interfaces.Azure;
using YapartMarket.Core.Mapper;

namespace YapartMarket.UnitTests.YapartMarket.BL
{
    public class TestAliExpressOrderReceiptInfoService
    {
        private readonly IOptions<AliExpressOptions> _aliExpressOption;
        private readonly IConfiguration _configuration;
        private Mock<IMapper> _mockMapper;
        private IMapper _mapper;
        private readonly IOptions<Connections> _connections;
        private readonly Mock<IAzureAliExpressOrderReceiptInfoRepository> _mockOrderReceiptInfoRepository;

        public TestAliExpressOrderReceiptInfoService()
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
            _mockOrderReceiptInfoRepository = new Mock<IAzureAliExpressOrderReceiptInfoRepository>();

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AliExpressOrderProfile());
            });
            _mapper = mockMapper.CreateMapper();
        }
        [Fact]
        public void GetReceiptInfo_CallWithCorrectValue_ReturnCategory()
        {
            //Arrange
            var orderId = 5029293096328871;
            var orderReceiptInfoService = new AliExpressOrderReceiptInfoService(_aliExpressOption, _mockOrderReceiptInfoRepository.Object, _mapper);
            //Act
            var result = orderReceiptInfoService.GetReceiptInfo(orderId);
            //Assert
            Assert.NotNull(result);
        }
    }
}
