using YapartMarket.Core.DTO;

namespace YapartMarket.Core.BL
{
    public interface IAliExpressTokenService
    {
       AliExpressTokenInfoDTO GetAccessToken();
    }
}
