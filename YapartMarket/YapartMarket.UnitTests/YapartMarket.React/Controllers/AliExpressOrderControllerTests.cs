using System.IO;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using YapartMarket.Core.BL;
using YapartMarket.Core.BL.AliExpress;
using YapartMarket.Core.Config;
using YapartMarket.React.Controllers;
using YapartMarket.React.Invocables;

namespace YapartMarket.UnitTests.YapartMarket.React.Controllers
{
    public class AliExpressOrderControllerTests
    {
        private readonly IConfiguration _configuration;
        private readonly IOptions<AliExpressOptions> _aliExpressOption;
        private readonly Mock<ILogger<UpdateOrdersFromAliExpressInvocable>> _mockLogger;
        private readonly Mock<IAliExpressOrderService> _mockOrderService;
        private readonly Mock<IAliExpressOrderReceiptInfoService> _mockOrderReceiptInfoService;
        private readonly Mock<IAliExpressLogisticRedefiningService> _mockLogisticRedefiningService;
        private readonly Mock<IAliExpressLogisticOrderDetailService> _mockLogisticOrderDetailService;
        private readonly Mock<IAliExpressProductService> _mockProductService;
        private readonly Mock<IAliExpressCategoryService> _mockCategoryService;
        private readonly Mock<IAliExpressOrderFullfilService> _mockOrderFullfilService;
        private readonly Mock<ILogisticServiceOrderService> _mockLogisticServiceOrderService;
        private readonly Mock<ILogisticWarehouseOrderService> _mockWarehouseOrderService;
        private readonly Mock<IMapper> _mockMapper;

        public AliExpressOrderControllerTests()
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

            _mockLogger = new Mock<ILogger<UpdateOrdersFromAliExpressInvocable>>();
            _mockOrderService = new Mock<IAliExpressOrderService>();
            _mockOrderReceiptInfoService = new Mock<IAliExpressOrderReceiptInfoService>();
            _mockLogisticRedefiningService = new Mock<IAliExpressLogisticRedefiningService>();
            _mockLogisticOrderDetailService = new Mock<IAliExpressLogisticOrderDetailService>();
            _mockProductService = new Mock<IAliExpressProductService>();
            _mockCategoryService = new Mock<IAliExpressCategoryService>();
            _mockOrderFullfilService = new Mock<IAliExpressOrderFullfilService>();
            _mockLogisticServiceOrderService = new Mock<ILogisticServiceOrderService>();
            _mockWarehouseOrderService = new Mock<ILogisticWarehouseOrderService>();
            _mockMapper = new Mock<IMapper>();
        }
        [Fact]
        public void UpdateOrders_SuccessDeserialize()
        {
            //Arrange
            var orderController = new AliExpressOrderController(_mockOrderService.Object, _mockOrderReceiptInfoService.Object, _mockLogisticRedefiningService.Object,
                _mockLogisticOrderDetailService.Object,_mockProductService.Object, _mockCategoryService.Object, _mockOrderFullfilService.Object, _mockLogisticServiceOrderService.Object,
                _mockWarehouseOrderService.Object,_mockLogger.Object, _mockMapper.Object);
            //Act
            var updateOrders = orderController.UpdateOrders();
            //Assert
            var okResult = (OkResult)updateOrders.Result;
            Assert.NotNull(okResult);
        }
    }
}
