using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Xunit;
using YapartMarket.Core.Config;
using YapartMarket.Core.Extensions;
using YapartMarket.Core.Models.Azure;
using YapartMarket.Data.Implementation.Azure;

namespace YapartMarket.UnitTests.YapartMarket.Data
{
    public class TestAzureAliExpressOrderReceiptInfo
    {
        private readonly IConfiguration _configuration;
        public TestAzureAliExpressOrderReceiptInfo()
        {
            TypeMapper.Initialize("YapartMarket.Core.Models.Azure");
            _configuration = (IConfiguration)new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true).Build();
        }

        [Fact]
        public async Task TestAzureAliExpressOrderReceiptInfo_InsertAsync_AddOrderRinf()
        {
            //arrange
            var aliExPressOrderReceipt = new AliExpressOrderReceiptInfo()
            {
                OrderId = 5013511820952050,
                Address = "Pushln 4",
                CountryName = "SPB"
            };
            var aliExpressOrderRecInfo = new AzureAliExpressOrderReceiptInfoRepository( new Logger<AzureAliExpressOrderReceiptInfoRepository>(new LoggerFactory()),"dbo.order_receipt_infos", _configuration.GetConnectionString("SQLServerConnectionString"));
            //act
            await aliExpressOrderRecInfo.InsertAsync(aliExPressOrderReceipt);
            var result = await aliExpressOrderRecInfo.GetAsync("select * from dbo.order_receipt_infos where order_id = @order_id", new {order_id = aliExPressOrderReceipt.OrderId});
            //assert
            Assert.NotNull(result.IsAny());
        }
    }
}
