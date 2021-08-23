using Newtonsoft.Json;

namespace YapartMarket.Core.DTO
{
    public class AliExpressError
    {
        [JsonProperty("error_response")]
        public AliExpressErrorMessage AliExpressErrorMessage { get; set; }
    }

    public class AliExpressErrorMessage
    {
        [JsonProperty("msg")]
        public string Message { get; set; }
        [JsonProperty("code")]
        public int Code { get; set; }
        [JsonProperty("sub_msg")]
        public string SubMessage { get; set; }
        [JsonProperty("sub_code")]
        public string SubCode { get; set; }
    }
}
