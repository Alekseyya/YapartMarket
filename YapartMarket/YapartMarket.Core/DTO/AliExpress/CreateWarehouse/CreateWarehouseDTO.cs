using System.Collections.Generic;
using Newtonsoft.Json;

namespace YapartMarket.React.ViewModels.AliExpress
{
    public class AddressParam
    {
        [JsonProperty("country_code")]
        public string country_code { get; set; }
        [JsonProperty("province")]
        public string province { get; set; }
        [JsonProperty("city")]
        public string city { get; set; }
        [JsonProperty("street")]
        public string street { get; set; }
        [JsonProperty("detail_address")]
        public string detail_address { get; set; }
        [JsonProperty("district")]
        public string district { get; set; }
        [JsonProperty("country_name")]
        public string country_name { get; set; }
        [JsonProperty("zip_code")]
        public string zip_code { get; set; }
    }

    public class Features
    {
        [JsonProperty("zip_code")]
        public string warehouse_code { get; set; }
    }

    public class ItemParam
    {
        [JsonProperty("quantity")]
        public int quantity { get; set; }
        [JsonProperty("total_price")]
        public int total_price { get; set; }
        [JsonProperty("item_id")]
        public long item_id { get; set; }
        [JsonProperty("item_features")]
        public string item_features { get; set; }
        [JsonProperty("weight")]
        public int weight { get; set; }
        [JsonProperty("unit_price")]
        public int unit_price { get; set; }
        [JsonProperty("local_name")]
        public string local_name { get; set; }
        [JsonProperty("english_name")]
        public string english_name { get; set; }
        [JsonProperty("currency")]
        public string currency { get; set; }
        [JsonProperty("sku")]
        public string sku { get; set; }
    }

    public class OrderParam
    {
        [JsonProperty("sender_param")]
        public SenderParam sender_param { get; set; }
        [JsonProperty("solution_param")]
        public SolutionParam solution_param { get; set; }
        [JsonProperty("package_params")]
        public List<PackageParam> package_params { get; set; }
        [JsonProperty("seller_info_param")]
        public SellerInfoParam seller_info_param { get; set; }
        [JsonProperty("receiver_param")]
        public ReceiverParam receiver_param { get; set; }
        [JsonProperty("returner_param")]
        public ReturnerParam returner_param { get; set; }
        [JsonProperty("trade_order_param")]
        public TradeOrderParam trade_order_param { get; set; }
        [JsonProperty("pickup_info_param")]
        public PickupInfoParam pickup_info_param { get; set; }
    }

    public class PackageParam
    {
        [JsonProperty("item_params")]
        public List<ItemParam> item_params { get; set; }
    }

    public class PickupInfoParam
    {
        [JsonProperty("seller_address_id")]
        public long seller_address_id { get; set; }
    }

    public class ReceiverParam
    {
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("telephone")]
        public string telephone { get; set; }
        [JsonProperty("mobile_phone")]
        public string mobile_phone { get; set; }
        [JsonProperty("address_param")]
        public AddressParam address_param { get; set; }
    }

    public class ReturnerParam
    {
        [JsonProperty("seller_address_id")]
        public long seller_address_id { get; set; }
    }

    public class CreateWarehouseDTO
    {
        [JsonProperty("locale")]
        public string locale { get; set; }
        [JsonProperty("order_param")]
        public OrderParam order_param { get; set; }
    }

    public class SellerInfoParam
    {
        [JsonProperty("top_user_key")]
        public string top_user_key { get; set; }
    }

    public class SenderParam
    {
        [JsonProperty("seller_address_id")]
        public long seller_address_id { get; set; }
    }

    public class ServiceParam
    {
        [JsonProperty("features")]
        public Features features { get; set; }
        [JsonProperty("code")]
        public string code { get; set; }
    }

    public class SolutionParam
    {
        [JsonProperty("service_params")]
        public List<ServiceParam> service_params { get; set; }
        [JsonProperty("solution_code")]
        public string solution_code { get; set; }
    }

    public class TradeOrderParam
    {
        [JsonProperty("trade_order_id")]
        public long trade_order_id { get; set; }
    }
}
