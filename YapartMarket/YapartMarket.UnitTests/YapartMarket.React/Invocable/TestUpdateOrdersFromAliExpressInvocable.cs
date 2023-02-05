using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using YapartMarket.Core.BL;
using YapartMarket.Core.BL.AliExpress;
using YapartMarket.Core.Config;
using YapartMarket.Core.Extensions;
using YapartMarket.React.Invocables;

namespace YapartMarket.UnitTests.YapartMarket.React.Invocable
{
    public class TestUpdateOrdersFromAliExpressInvocable
    {
        private readonly IOptions<AliExpressOptions> _aliExpressOption;
        private readonly IConfiguration _configuration;
        private Mock<IAliExpressOrderReceiptInfoService> _mockOrderReceiptInfoService;
        private Mock<IAliExpressOrderService> _mockOrderService;
        private Mock<IMapper> _mockMapper;
        private Mock<ILogger<UpdateOrdersFromAliExpressInvocable>> _mockLoggerUpdateOrdersFromAliExpressInvocable;
        private readonly Mock<IAliExpressLogisticRedefiningService> _mockRedefiningService;
        private readonly Mock<IAliExpressLogisticOrderDetailService> _mockLogisticOrderDetailService;
        private readonly Mock<IAliExpressOrderFullfilService> _mockOrderFullfilService;
        private readonly Mock<ILogisticServiceOrderService> _mockLogisticServiceOrder;
        private readonly Mock<IAliExpressProductService> _mockProductService;
        private readonly Mock<IAliExpressCategoryService> _mockCategoryService;
        private readonly Mock<ILogisticWarehouseOrderService> _mockLogisticWarehouseOrder;

        public TestUpdateOrdersFromAliExpressInvocable()
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
            _mockOrderReceiptInfoService = new Mock<IAliExpressOrderReceiptInfoService>();
            _mockOrderService = new Mock<IAliExpressOrderService>();
            _mockRedefiningService = new Mock<IAliExpressLogisticRedefiningService>();
            _mockLogisticOrderDetailService = new Mock<IAliExpressLogisticOrderDetailService>();
            _mockOrderFullfilService = new Mock<IAliExpressOrderFullfilService>();
            _mockLoggerUpdateOrdersFromAliExpressInvocable = new Mock<ILogger<UpdateOrdersFromAliExpressInvocable>>();
            _mockProductService = new Mock<IAliExpressProductService>();
            _mockCategoryService = new Mock<IAliExpressCategoryService>();
            _mockLogisticServiceOrder = new Mock<ILogisticServiceOrderService>();
            _mockLogisticWarehouseOrder = new Mock<ILogisticWarehouseOrderService>();
            _mockMapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task TestUpdateOrdersFromAliExpressInvocable_Invoke_IntegrateTest()
        {
            //arrange
            var dateTimeNow = DateTime.UtcNow;
            _mockOrderService.Setup(s => s.QueryOrderDetail(dateTimeNow.AddDays(-20).StartOfDay(), dateTimeNow.AddDays(+1).EndOfDay(), null));
            //_mockOrderService.Verify(s=>s.QueryOrderDetail(dateTimeNow.AddDays(-20).StartOfDay(), dateTimeNow.AddDays(+1).EndOfDay(), null));
            var updateOrdersFromAliExpressInvocable = new UpdateOrdersFromAliExpressInvocable(_mockOrderService.Object,
                _mockOrderReceiptInfoService.Object,
                _mockRedefiningService.Object,
                _mockLogisticOrderDetailService.Object,
                _mockOrderFullfilService.Object, 
                _mockProductService.Object,
                _mockCategoryService.Object,
                _mockLogisticServiceOrder.Object,
                _mockLogisticWarehouseOrder.Object,
                _mockLoggerUpdateOrdersFromAliExpressInvocable.Object, _mockMapper.Object);
            //act
            //assert
            try
            {
                await updateOrdersFromAliExpressInvocable.Invoke();
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
