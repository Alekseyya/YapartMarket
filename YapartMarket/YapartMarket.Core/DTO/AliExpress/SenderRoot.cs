using System.Collections.Generic;
using Newtonsoft.Json;

namespace YapartMarket.Core.DTO.AliExpress
{
    public sealed class SenderRoot
    {
        [JsonProperty("aliexpress_logistics_redefining_getlogisticsselleraddresses_response")]
        public Sender Sender { get; set; }
    }

    public sealed class Sender
    {
        [JsonProperty("sender_seller_address_list")]
        public SenderSellerAddressList SenderSellerAddressList { get; set; }
    }

    public class SenderSellerAddressList
    {
        [JsonProperty("senderselleraddresslist")]
        public List<SenderSellerAddress> SenderSellerAddress { get; set; }
    }

    public class SenderSellerAddress
    {
        [JsonProperty("street_address")]
        public string StreetAddress { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("fax")]
        public string Fax { get; set; }
        [JsonProperty("street")]
        public string Street { get; set; }
        [JsonProperty("member_type")]
        public string MemberType { get; set; }
        [JsonProperty("postcode")]
        public string Postcode { get; set; }
        [JsonProperty("address_id")]
        public long AddressId { get; set; }
        [JsonProperty("trademanage_id")]
        public string TradeManageId { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("county")]
        public string County { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("province")]
        public string Province { get; set; }
        [JsonProperty("mobile")]
        public string Mobile { get; set; }
        [JsonProperty("language")]
        public string Language { get; set; }
    }
}
