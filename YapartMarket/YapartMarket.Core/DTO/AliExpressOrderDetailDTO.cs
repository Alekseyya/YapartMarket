using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace YapartMarket.Core.DTO
{
    public class AliExpressOrderDetailDTO
    {
        [JsonProperty("seller_signer_fullname")]
        public string SellerSignerFullName { get; set; }
        [JsonProperty("seller_login_id")]
        public string SellerLoginId { get; set; }
        [JsonProperty("product_list.order_product_dto")]
        public List<AliExpressOrderProduct> AliExpressOrderProducts { get; set; }
        [JsonProperty("pay_amount.amount")]
        public int PayAmount { get; set; }
        [JsonProperty("order_status")]
        public string OrderStatus { get; set; }
        [JsonProperty("order_id")]
        public long OrderId { get; set; }
        [JsonProperty("logistics_status")]
        public string LogisticsStatus { get; set; }
        [JsonProperty("logistics_status")]
        public string GmtUpdate { get; set; }
        [JsonProperty("gmt_pay_time")] 
        public string GmtPayTime { get; set; }
        [JsonProperty("gmt_create")]
        public string GmtCreate { get; set; }
        [JsonProperty("fund_status")]
        public string FundStatus { get; set; }
        [JsonProperty("frozen_status")]
        public string FrozenStatus { get; set; }
        /// <summary>
        /// Причина завершения заказа
        /// </summary>
        [JsonProperty("end_reason")]
        public string EndReason { get; set; }
        [JsonProperty("buyer_singer_fullname")]
        public string BuyerSingerFullname { get; set; }
        [JsonProperty("buyer_login_id")] 
        public string BuyerLoginId { get; set; }
        [JsonProperty("biz_type")]
        public string BizType { get; set; }
    }

    public class AliExpressOrderProduct
    {
        [JsonProperty("show_status")]
        public string ShowStatus { get; set; } // todo сериализацию в enum
        [JsonProperty("send_goods_time")]
        public string SendGoodsTime { get; set; }
        [JsonProperty("product_name")]
        public string ProductName { get; set; }
        [JsonProperty("product_id")]
        public string ProductId { get; set; }
        [JsonProperty("product_count")]
        public int ProductCount { get; set; }
        [JsonProperty("order_id")]
        public int OrderId { get; set; }
        [JsonProperty("goods_prepare_time")] 
        public int GoodsPrepareTime { get; set; }
    }
}
