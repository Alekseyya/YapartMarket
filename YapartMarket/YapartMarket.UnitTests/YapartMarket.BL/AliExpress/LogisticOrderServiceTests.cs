using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using Top.Api.Util;
using Xunit;
using Xunit.Abstractions;
using YapartMarket.BL.Implementation;
using YapartMarket.Core.Config;
using YapartMarket.Core.Data.Interfaces.Azure;
using YapartMarket.React.ViewModels.Goods;

namespace YapartMarket.UnitTests.YapartMarket.BL.AliExpress
{
    public class LogisticOrderServiceTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IConfiguration _configuration;
        private readonly IOptions<AliExpressOptions> _aliExpressOption;
        private readonly Mock<ILogger<LogisticServiceOrderService>> _mockLogger;
        private readonly Mock<ILogisticServiceOrderRepository> _mockLogisticServiceOrderRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly HttpClient _client;

        public LogisticOrderServiceTests(ITestOutputHelper testOutputHelper)
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
            _mockLogger = new Mock<ILogger<LogisticServiceOrderService>>();
            _mockMapper = new Mock<IMapper>();
            _mockLogisticServiceOrderRepository = new Mock<ILogisticServiceOrderRepository>();
            var mockFactory = new Mock<IHttpClientFactory>();
            var clientHandlerStub = new DelegatingHandlerStub();
            _client = new HttpClient(clientHandlerStub);
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(_client);
            IHttpClientFactory factory = mockFactory.Object;
        }


        public class DelegatingHandlerStub : DelegatingHandler
        {
            private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _handlerFunc;

            public DelegatingHandlerStub()
            {
                _handlerFunc = (request, cancellationToken) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
            }

            public DelegatingHandlerStub(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handlerFunc)
            {
                _handlerFunc = handlerFunc;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return _handlerFunc(request, cancellationToken);
            }
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
        [Fact]
        public async Task GetLogisticServiceOrderRequest_OrderIdTop_Success()
        {
            //Arrange
            var dic = new Dictionary<string, string>();
            dic.Add("method", "aliexpress.logistics.redefining.getonlinelogisticsservicelistbyorderid");
            dic.Add("v", "2.0");
            dic.Add("sign_method ", "hmac");
            dic.Add("app_key", _aliExpressOption.Value.AppKey);
            dic.Add("format","json");
            dic.Add("timestamp", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
            dic.Add("session", _aliExpressOption.Value.AccessToken);
            dic.Add("locale", "ru_RU");
            dic.Add("order_id", 5029907170423630.ToString());
            dic.Add("sign", TopUtils.SignTopRequest(dic, _aliExpressOption.Value.AppSecret, "hmac"));
            var sellarParam = @"{
  ""seller_address_id"": 123456
}";
            var solutionService = @"{
  ""service_param"": {
    ""code"": ""DOOR_PICKUP""
  },
  ""solution_code"": ""AE_RU_MP_RUPOST_PH3_FR""
}";
            dic.Add("seller_param", sellarParam);
            dic.Add("solution_service_res_param", solutionService);
            var url = $"https://eco.taobao.com/router/rest?{HttpUtility.UrlEncode(string.Join("&", dic.Select(kvp => $"{kvp.Key}={kvp.Value}")))}";

            var content = new StringContent("", Encoding.UTF8, "application/json");
            var result = await _client.PostAsync(url, content);
            string resultContent = await result.Content.ReadAsStringAsync();
            //return JsonConvert.DeserializeObject<SuccessfulResponse>(resultContent);
            _testOutputHelper.WriteLine(resultContent);

            //var logisticServiceOrderService = new LogisticServiceOrderService(_mockLogger.Object, _aliExpressOption, _mockMapper.Object, _mockLogisticServiceOrderRepository.Object);
            ////Act
            //var result = logisticServiceOrderService.GetLogisticServiceOrderRequest(orderId);
            ////Assert
            //Assert.NotNull(result);
        }
    }
}
