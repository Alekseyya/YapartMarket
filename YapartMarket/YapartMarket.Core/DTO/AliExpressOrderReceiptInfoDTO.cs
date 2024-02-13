using Newtonsoft.Json;

namespace YapartMarket.Core.DTO
{
    public sealed class AliExpressReceiptRoot
    {
        [JsonProperty("aliexpress_solution_order_receiptinfo_get_response")]
        public AliExpressReceiptInfoResult? AliExpressReceiptInfoResult { get; set; }
    }

    public sealed class AliExpressReceiptInfoResult
    {
        [JsonProperty("result")]
        public AliExpressOrderReceiptInfoDTO? AliExpressOrderReceiptInfoDto { get; set; }
        [JsonProperty("request_id")]
        public string? RequestId { get; set; }
    }
    public sealed class AliExpressOrderReceiptInfoDTO
    {
        [JsonProperty("country_name")]
        public string? CountryName { get; set; }
        [JsonProperty("mobile_no")]
        public string? Mobile { get; set; }
        [JsonProperty("contact_person")]
        public string? ContractPerson { get; set; }
        [JsonProperty("phone_country")]
        public string? PhoneCountry { get; set; }
        [JsonProperty("phone_area")]
        public string? PhoneArea { get; set; }
        [JsonProperty("province")]
        public string? Province { get; set; }
        [JsonProperty("address")]
        public string? Address { get; set; }
        [JsonProperty("phone_number")]
        public string? PhoneNumber { get; set; }
        [JsonProperty("fax_number")]
        public string? FaxNumber { get; set; }
        [JsonProperty("detail_address")]
        public string? DetailAddress { get; set; }
        [JsonProperty("city")]
        public string? City { get; set; }
        [JsonProperty("country")]
        public string? Country { get; set; }
        [JsonProperty("address2")]
        public string? Address2 { get; set; }
        [JsonProperty("fax_country")]
        public string? FaxCountry { get; set; }
        [JsonProperty("zip")]
        public string? Zip { get; set; }
        [JsonProperty("fax_area")]
        public string? FaxArea { get; set; }
        [JsonProperty("localized_address")]
        public string? LocalizedAddress { get; set; }

    }
}
