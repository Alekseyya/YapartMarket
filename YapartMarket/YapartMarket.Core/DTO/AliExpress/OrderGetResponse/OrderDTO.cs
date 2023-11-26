using System.Collections.Generic;
using Newtonsoft.Json;

namespace YapartMarket.Core.DTO.AliExpress.OrderGetResponse
{
    public class AliexpressSolutionOrderGetResponse
    {
        [JsonProperty("result")]
        public Result result { get; set; }
    }

    public class EscrowFee
    {
        [JsonProperty("currency_code")]
        public string currency_code { get; set; }
        [JsonProperty("amount")]
        public string amount { get; set; }
    }

    public class LoanAmount
    {
        [JsonProperty("currency_code")]
        public string currency_code { get; set; }
        [JsonProperty("amount")]
        public string amount { get; set; }
    }

    public class LogisticsAmount
    {
        [JsonProperty("currency_code")]
        public string currency_code { get; set; }
        [JsonProperty("amount")]
        public string amount { get; set; }
    }

    public class OrderDto
    {
        [JsonProperty("timeout_left_time")]
        public int timeout_left_time { get; set; }
        [JsonProperty("seller_signer_fullname")]
        public string SellerSignerFullname { get; set; }
        [JsonProperty("seller_operator_login_id")]
        public string SellerOperatorLoginId { get; set; }
        [JsonProperty("seller_login_id")]
        public string seller_login_id { get; set; }
        [JsonProperty("product_list")]
        public ProductList product_list { get; set; }
        [JsonProperty("phone")]
        public bool phone { get; set; }
        [JsonProperty("payment_type")]
        public string payment_type { get; set; }
        [JsonProperty("pay_amount")]
        public PayAmount pay_amount { get; set; }
        [JsonProperty("order_status")]
        public string order_status { get; set; }
        [JsonProperty("order_id")]
        public long order_id { get; set; }
        [JsonProperty("order_detail_url")]
        public string order_detail_url { get; set; }
        [JsonProperty("logistics_status")]
        public string logistics_status { get; set; }
        [JsonProperty("logisitcs_escrow_fee_rate")]
        public string logisitcs_escrow_fee_rate { get; set; }
        [JsonProperty("loan_amount")]
        public LoanAmount loan_amount { get; set; }
        [JsonProperty("left_send_good_min")]
        public string left_send_good_min { get; set; }
        [JsonProperty("left_send_good_hour")]
        public string left_send_good_hour { get; set; }
        [JsonProperty("left_send_good_day")]
        public string left_send_good_day { get; set; }
        [JsonProperty("issue_status")]
        public string issue_status { get; set; }
        [JsonProperty("has_request_loan")]
        public bool has_request_loan { get; set; }
        [JsonProperty("gmt_update")]
        public string gmt_update { get; set; }
        [JsonProperty("gmt_send_goods_time")]
        public string gmt_send_goods_time { get; set; }
        [JsonProperty("gmt_pay_time")]
        public string gmt_pay_time { get; set; }
        [JsonProperty("gmt_create")]
        public string gmt_create { get; set; }
        [JsonProperty("fund_status")]
        public string fund_status { get; set; }
        [JsonProperty("frozen_status")]
        public string frozen_status { get; set; }
        [JsonProperty("escrow_fee_rate")]
        public int escrow_fee_rate { get; set; }
        [JsonProperty("escrow_fee")]
        public EscrowFee escrow_fee { get; set; }
        [JsonProperty("end_reason")]
        public string end_reason { get; set; }
        [JsonProperty("buyer_signer_fullname")]
        public string buyer_signer_fullname { get; set; }
        [JsonProperty("buyer_login_id")]
        public string buyer_login_id { get; set; }
        [JsonProperty("biz_type")]
        public string biz_type { get; set; }
        [JsonProperty("offline_pickup_type")]
        public string offline_pickup_type { get; set; }
    }

    public class OrderProductDto
    {
        [JsonProperty("total_product_amount")]
        public TotalProductAmount total_product_amount { get; set; }
        [JsonProperty("son_order_status")]
        public string son_order_status { get; set; }
        [JsonProperty("sku_code")]
        public string sku_code { get; set; }
        [JsonProperty("show_status")]
        public string show_status { get; set; }
        [JsonProperty("send_goods_time")]
        public string send_goods_time { get; set; }
        [JsonProperty("send_goods_operator")]
        public string send_goods_operator { get; set; }
        [JsonProperty("product_unit_price")]
        public ProductUnitPrice product_unit_price { get; set; }
        [JsonProperty("product_unit")]
        public string product_unit { get; set; }
        [JsonProperty("product_standard")]
        public string product_standard { get; set; }
        [JsonProperty("product_snap_url")]
        public string product_snap_url { get; set; }
        [JsonProperty("product_name")]
        public string product_name { get; set; }
        [JsonProperty("product_img_url")]
        public string product_img_url { get; set; }
        [JsonProperty("product_id")]
        public long product_id { get; set; }
        [JsonProperty("product_count")]
        public int product_count { get; set; }
        [JsonProperty("order_id")]
        public long order_id { get; set; }
        [JsonProperty("money_back3x")]
        public bool money_back3x { get; set; }
        [JsonProperty("memo")]
        public string memo { get; set; }
        [JsonProperty("logistics_type")]
        public string logistics_type { get; set; }
        [JsonProperty("logistics_service_name")]
        public string logistics_service_name { get; set; }
        [JsonProperty("logistics_amount")]
        public LogisticsAmount logistics_amount { get; set; }
        [JsonProperty("issue_status")]
        public string issue_status { get; set; }
        [JsonProperty("issue_mode")]
        public string issue_mode { get; set; }
        [JsonProperty("goods_prepare_time")]
        public int goods_prepare_time { get; set; }
        [JsonProperty("fund_status")]
        public string fund_status { get; set; }
        [JsonProperty("freight_commit_day")]
        public string freight_commit_day { get; set; }
        [JsonProperty("escrow_fee_rate")]
        public string escrow_fee_rate { get; set; }
        [JsonProperty("delivery_time")]
        public string delivery_time { get; set; }
        [JsonProperty("child_id")]
        public long child_id { get; set; }
        [JsonProperty("can_submit_issue")]
        public bool can_submit_issue { get; set; }
        [JsonProperty("buyer_signer_last_name")]
        public string buyer_signer_last_name { get; set; }
        [JsonProperty("buyer_signer_first_name")]
        public string buyer_signer_first_name { get; set; }
        [JsonProperty("afflicate_fee_rate")]
        public string afflicate_fee_rate { get; set; }
    }

    public class PayAmount
    {
        [JsonProperty("currency_code")]
        public string currency_code { get; set; }
        [JsonProperty("amount")]
        public string amount { get; set; }
    }

    public class ProductList
    {
        [JsonProperty("order_product_dto")]
        public List<OrderProductDto> order_product_dto { get; set; }
    }

    public class ProductUnitPrice
    {
        [JsonProperty("currency_code")]
        public string currency_code { get; set; }
        [JsonProperty("amount")]
        public string amount { get; set; }
    }

    public class Result
    {
        [JsonProperty("error_message")]
        public string error_message { get; set; }
        [JsonProperty("total_count")]
        public int total_count { get; set; }
        [JsonProperty("target_list")]
        public TargetList target_list { get; set; }
        [JsonProperty("page_size")]
        public int page_size { get; set; }
        [JsonProperty("error_code")]
        public string error_code { get; set; }
        [JsonProperty("current_page")]
        public int current_page { get; set; }
        [JsonProperty("total_page")]
        public int total_page { get; set; }
        [JsonProperty("success")]
        public bool success { get; set; }
        [JsonProperty("time_stamp")]
        public string time_stamp { get; set; }
    }

    public class OrderRootDto
    {
        [JsonProperty("aliexpress_solution_order_get_response")]
        public AliexpressSolutionOrderGetResponse aliexpress_solution_order_get_response { get; set; }
    }

    public class TargetList
    {
        [JsonProperty("order_dto")]
        public List<OrderDto> order_dto { get; set; }
    }

    public class TotalProductAmount
    {
        [JsonProperty("currency_code")]
        public string currency_code { get; set; }
        [JsonProperty("amount")]
        public string amount { get; set; }
    }
}
