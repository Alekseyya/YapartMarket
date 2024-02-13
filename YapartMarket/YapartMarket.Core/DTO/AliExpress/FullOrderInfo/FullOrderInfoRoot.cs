using System.Collections.Generic;
using Newtonsoft.Json;

namespace YapartMarket.Core.DTO.AliExpress.FullOrderInfo
{
    public class AeopTpChildOrderDto
    {
        [JsonProperty("afflicate_fee_rate")]
        public string? afflicate_fee_rate { get; set; }
        [JsonProperty("child_order_id")]
        public string? child_order_id { get; set; }
        [JsonProperty("escrow_fee_rate")]
        public string? escrow_fee_rate { get; set; }
        [JsonProperty("extend_map")]
        public string? extend_map { get; set; }
        [JsonProperty("frozen_status")]
        public string? frozen_status { get; set; }
        [JsonProperty("fund_status")]
        public string? fund_status { get; set; }
        [JsonProperty("goods_prepare_time")]
        public int goods_prepare_time { get; set; }
        [JsonProperty("id")]
        public long id { get; set; }
        [JsonProperty("init_order_amt")]
        public InitOrderAmt? init_order_amt { get; set; }
        [JsonProperty("issue_status")]
        public string? issue_status { get; set; }
        [JsonProperty("loan_info")]
        public LoanInfo? loan_info { get; set; }
        [JsonProperty("logistics_service_name")]
        public string? logistics_service_name { get; set; }
        [JsonProperty("logistics_type")]
        public string? logistics_type { get; set; }
        [JsonProperty("lot_num")]
        public int lot_num { get; set; }
        [JsonProperty("order_sort_id")]
        public int order_sort_id { get; set; }
        [JsonProperty("order_status")]
        public string? order_status { get; set; }
        [JsonProperty("product_attributes")]
        public string? product_attributes { get; set; }
        [JsonProperty("product_count")]
        public int product_count { get; set; }
        [JsonProperty("product_id")]
        public long product_id { get; set; }
        [JsonProperty("product_img_url")]
        public string? product_img_url { get; set; }
        [JsonProperty("product_name")]
        public string? product_name { get; set; }
        [JsonProperty("product_price")]
        public ProductPrice? product_price { get; set; }
        [JsonProperty("product_snap_url")]
        public string? product_snap_url { get; set; }
        [JsonProperty("product_unit")]
        public string? product_unit { get; set; }
        [JsonProperty("refund_info")]
        public RefundInfo? refund_info { get; set; }
        [JsonProperty("send_goods_operator")]
        public string? send_goods_operator { get; set; }
        [JsonProperty("sku_code")]
        public string? sku_code { get; set; }
        [JsonProperty("snapshot_small_photo_path")]
        public string? snapshot_small_photo_path { get; set; }
        [JsonProperty("tags")]
        public Tags? tags { get; set; }
    }

    public class AeopTpOrderProductInfoDto
    {
        [JsonProperty("category_id")]
        public string? category_id { get; set; }
        [JsonProperty("product_id")]
        public long product_id { get; set; }
        [JsonProperty("product_name")]
        public string? product_name { get; set; }
        [JsonProperty("quantity")]
        public int quantity { get; set; }
        [JsonProperty("sku")]
        public string? sku { get; set; }
        [JsonProperty("unit_price")]
        public UnitPrice? unit_price { get; set; }
    }

    public class AliexpressTradeNewRedefiningFindorderbyidResponse
    {
        [JsonProperty("target")]
        public Target? target { get; set; }
        [JsonProperty("time_stamp")]
        public string? time_stamp { get; set; }
        [JsonProperty("request_id")]
        public string? request_id { get; set; }
    }

    public class BuyerInfo
    {
        [JsonProperty("country")]
        public string? country { get; set; }
        [JsonProperty("first_name")]
        public string? first_name { get; set; }
        [JsonProperty("last_name")]
        public string? last_name { get; set; }
        [JsonProperty("login_id")]
        public string? login_id { get; set; }
    }

    public class ChildOrderExtInfoList
    {
        [JsonProperty("aeop_tp_order_product_info_dto")]
        public List<AeopTpOrderProductInfoDto>? aeop_tp_order_product_info_dto { get; set; }
    }

    public class ChildOrderList
    {
        [JsonProperty("aeop_tp_child_order_dto")]
        public List<AeopTpChildOrderDto>? aeop_tp_child_order_dto { get; set; }
    }

    public class Currency
    {
        [JsonProperty("currency_code")]
        public string? currency_code { get; set; }
        [JsonProperty("default_fraction_digits")]
        public int default_fraction_digits { get; set; }
        [JsonProperty("display_name")]
        public string? display_name { get; set; }
        [JsonProperty("numeric_code")]
        public int numeric_code { get; set; }
        [JsonProperty("symbol")]
        public string? symbol { get; set; }
    }

    public class InitOderAmount
    {
        [JsonProperty("amount")]
        public string? amount { get; set; }
        [JsonProperty("cent")]
        public int cent { get; set; }
        [JsonProperty("cent_factor")]
        public int cent_factor { get; set; }
        [JsonProperty("currency")]
        public Currency? currency { get; set; }
        [JsonProperty("currency_code")]
        public string? currency_code { get; set; }
    }

    public class InitOrderAmt
    {
        [JsonProperty("amount")]
        public string? amount { get; set; }
        [JsonProperty("cent")]
        public int cent { get; set; }
        [JsonProperty("cent_factor")]
        public int cent_factor { get; set; }
        [JsonProperty("currency")]
        public Currency? currency { get; set; }
        [JsonProperty("currency_code")]
        public string? currency_code { get; set; }
    }

    public class LoanInfo
    {
    }

    public class LogisticInfoList
    {
    }

    public class LogisticsAmount
    {
        [JsonProperty("amount")]
        public string? amount { get; set; }
        [JsonProperty("cent")]
        public int cent { get; set; }
        [JsonProperty("cent_factor")]
        public int cent_factor { get; set; }
        [JsonProperty("currency")]
        public Currency? currency { get; set; }
        [JsonProperty("currency_code")]
        public string? currency_code { get; set; }
    }

    public class NewOrderAmount
    {
        [JsonProperty("amount")]
        public string? amount { get; set; }
        [JsonProperty("currency_code")]
        public string? currency_code { get; set; }
    }

    public class OprLogDtoList
    {
    }

    public class OrderAmount
    {
        [JsonProperty("amount")]
        public string? amount { get; set; }
        [JsonProperty("cent")]
        public int cent { get; set; }
        [JsonProperty("cent_factor")]
        public int cent_factor { get; set; }
        [JsonProperty("currency")]
        public Currency? currency { get; set; }
        [JsonProperty("currency_code")]
        public string? currency_code { get; set; }
    }

    public class ProductPrice
    {
        [JsonProperty("amount")]
        public string? amount { get; set; }
        [JsonProperty("cent")]
        public int cent { get; set; }
        [JsonProperty("cent_factor")]
        public int cent_factor { get; set; }
        [JsonProperty("currency")]
        public Currency? currency { get; set; }
        [JsonProperty("currency_code")]
        public string? currency_code { get; set; }
    }

    public class ReceiptAddress
    {
        [JsonProperty("address2")]
        public string? address2 { get; set; }
        [JsonProperty("city")]
        public string? city { get; set; }
        [JsonProperty("contact_person")]
        public string? contact_person { get; set; }
        [JsonProperty("country")]
        public string? country { get; set; }
        [JsonProperty("detail_address")]
        public string? detail_address { get; set; }
        [JsonProperty("mobile_no")]
        public string? mobile_no { get; set; }
        [JsonProperty("phone_country")]
        public string? phone_country { get; set; }
        [JsonProperty("province")]
        public string? province { get; set; }
        [JsonProperty("zip")]
        public string? zip { get; set; }
    }

    public class RefundInfo
    {
    }

    public class Root
    {
        [JsonProperty("aliexpress_trade_new_redefining_findorderbyid_response")]
        public AliexpressTradeNewRedefiningFindorderbyidResponse? aliexpress_trade_new_redefining_findorderbyid_response { get; set; }
    }

    public class SellerOrderAmount
    {
        [JsonProperty("amount")]
        public string? amount { get; set; }
        [JsonProperty("currency_code")]
        public string? currency_code { get; set; }
    }

    public class Tags
    {
    }

    public class Target
    {
        [JsonProperty("buyer_info")]
        public BuyerInfo? buyer_info { get; set; }
        [JsonProperty("buyer_signer_fullname")]
        public string? buyer_signer_fullname { get; set; }
        [JsonProperty("buyerloginid")]
        public string? buyerloginid { get; set; }
        [JsonProperty("child_order_ext_info_list")]
        public ChildOrderExtInfoList? child_order_ext_info_list { get; set; }
        [JsonProperty("child_order_list")]
        public ChildOrderList? child_order_list { get; set; }
        [JsonProperty("cod")]
        public bool cod { get; set; }
        [JsonProperty("frozen_status")]
        public string? frozen_status { get; set; }
        [JsonProperty("fund_status")]
        public string? fund_status { get; set; }
        [JsonProperty("gmt_create")]
        public string? gmt_create { get; set; }
        [JsonProperty("gmt_modified")]
        public string? gmt_modified { get; set; }
        [JsonProperty("gmt_pay_success")]
        public string? gmt_pay_success { get; set; }
        [JsonProperty("id")]
        public long id { get; set; }
        [JsonProperty("init_oder_amount")]
        public InitOderAmount? init_oder_amount { get; set; }
        [JsonProperty("is_phone")]
        public bool is_phone { get; set; }
        [JsonProperty("issue_status")]
        public string? issue_status { get; set; }
        [JsonProperty("loan_info")]
        public LoanInfo? loan_info { get; set; }
        [JsonProperty("loan_status")]
        public string? loan_status { get; set; }
        [JsonProperty("logistic_info_list")]
        public LogisticInfoList? logistic_info_list { get; set; }
        [JsonProperty("logistics_amount")]
        public LogisticsAmount? logistics_amount { get; set; }
        [JsonProperty("logistics_status")]
        public string? logistics_status { get; set; }
        [JsonProperty("new_order_amount")]
        public NewOrderAmount? new_order_amount { get; set; }
        [JsonProperty("opr_log_dto_list")]
        public OprLogDtoList? opr_log_dto_list { get; set; }
        [JsonProperty("order_amount")]
        public OrderAmount? order_amount { get; set; }
        [JsonProperty("order_end_reason")]
        public string? order_end_reason { get; set; }
        [JsonProperty("order_status")]
        public string? order_status { get; set; }
        [JsonProperty("over_time_left")]
        public string? over_time_left { get; set; }
        [JsonProperty("pay_amount_by_settlement_cur")]
        public string? pay_amount_by_settlement_cur { get; set; }
        [JsonProperty("payment_type")]
        public string? payment_type { get; set; }
        [JsonProperty("receipt_address")]
        public ReceiptAddress? receipt_address { get; set; }
        [JsonProperty("refund_info")]
        public RefundInfo? refund_info { get; set; }
        [JsonProperty("seller_operator_login_id")]
        public string? seller_operator_login_id { get; set; }
        [JsonProperty("seller_order_amount")]
        public SellerOrderAmount? seller_order_amount { get; set; }
        [JsonProperty("seller_signer_fullname")]
        public string? seller_signer_fullname { get; set; }
        [JsonProperty("settlement_currency")]
        public string? settlement_currency { get; set; }
    }

    public class UnitPrice
    {
        [JsonProperty("amount")]
        public string? amount { get; set; }
        [JsonProperty("cent")]
        public int cent { get; set; }
        [JsonProperty("cent_factor")]
        public int cent_factor { get; set; }
        [JsonProperty("currency")]
        public Currency? currency { get; set; }
        [JsonProperty("currency_code")]
        public string? currency_code { get; set; }
    }
}
