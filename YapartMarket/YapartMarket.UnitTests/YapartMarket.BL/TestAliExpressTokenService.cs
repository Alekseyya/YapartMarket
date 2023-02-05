using Microsoft.Extensions.Options;
using Xunit;
using YapartMarket.BL.Implementation;
using YapartMarket.Core.Config;

namespace YapartMarket.UnitTests.YapartMarket.BL
{
    public class TestAliExpressTokenService
    {
        private readonly IOptions<AliExpressOptions> option;
        public TestAliExpressTokenService()
        {
            option = Options.Create<AliExpressOptions>(new AliExpressOptions()
            {
                AppKey = "32974644",
                AppSecret = "067237b4a92136723992d89bc877d75a",
                AuthorizationCode = "0_oifwMrouDFF4PS7CXXl5mv3933424",
                ReturnUrl = "https://yapart.azurewebsites.net/api/AliExpressToken"
            });
        }
        [Fact]
        public void TextAliExpressTokenService_DeserializeToObject()
        {
            //Arrange
            var aliExpressTokenService = new AliExpressTokenService(option);
            //Act
            var result = aliExpressTokenService.GetAccessToken();
            //Assert
            Assert.NotEmpty(result.AccessToken);
        }
    }
}
