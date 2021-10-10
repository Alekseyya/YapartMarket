using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;
using YapartMarket.BL.Implementation;
using YapartMarket.Core.Config;
using YapartMarket.Core.Data.Interfaces.Azure;
using YapartMarket.Core.Mapper;
using YapartMarket.Data.Implementation.Azure;

namespace YapartMarket.UnitTests.YapartMarket.BL
{
    public class TestAliExpressOrderLogistic
    {
        private readonly IOptions<AliExpressOptions> _aliExpressOption;
        private readonly IConfiguration _configuration;
        private Mock<IAzureAliExpressOrderLogisticRedefiningRepository> _mockAzureAliExpressOrderLogistRepository;
        private IMapper _mapper;
        private readonly ITestOutputHelper _output;

        private Mock<ILogger<AliExpressLogisticRedefiningService>> _mockLogger;
        public TestAliExpressOrderLogistic(ITestOutputHelper output)
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
            _mockLogger = new Mock<ILogger<AliExpressLogisticRedefiningService>>();
            _mockAzureAliExpressOrderLogistRepository = new Mock<IAzureAliExpressOrderLogisticRedefiningRepository>();
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AliExpressOrderProfile());
                cfg.AddProfile(new AliExpressOrderLogisticProfile());
            });
            _mapper = mockMapper.CreateMapper();
            _output = output;
        }

        [Fact]
        public void TestAliExpressOrderLogistic_LogisticsRedefiningListLogisticsServiceRequest_GetResult()
        {
            //arrange
            var orderLogistRep = new AliExpressLogisticRedefiningService(_mockLogger.Object, _aliExpressOption, _mapper ,_mockAzureAliExpressOrderLogistRepository.Object);
            //act
            var result = orderLogistRep.LogisticsRedefiningListLogisticsServiceRequest();
            //assert
            _output.WriteLine(JsonConvert.SerializeObject(result));
        }
    }
}
