using Xunit;
using YapartMarket.Core.Extensions;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.UnitTests.YapartMarket.Data.Extensions
{
    public class TestDapperExtensions
    {
        [Fact]
        public void TestDapperExtensions_InsertString_CreateString()
        {
            //arrange
            var aliOrder = new AliExpressOrder()
            {
                OrderId = 1
            };
            //act
            var sql = aliOrder.InsertString("dbo.orders");
            //assert
            Assert.Equal("INSERT INTO dbo.orders (order_id) values(@order_id)", sql);
        }
    }
}
