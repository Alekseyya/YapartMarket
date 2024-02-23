using Newtonsoft.Json;

namespace YapartMarket.Core.DTO
{
    public sealed class AliExpressTokenInfoDTO
	{
        [JsonProperty("access_token")]
        public string? AccessToken { get; set; }
        [JsonProperty("user_id")]
        public string? UserId { get; set; }
        [JsonProperty("expire_time")]
        public long ExpireTime { get; set; }
    }
}
