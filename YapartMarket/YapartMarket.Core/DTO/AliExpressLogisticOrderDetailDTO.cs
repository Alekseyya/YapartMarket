using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace YapartMarket.Core.DTO
{
    public class AliExpressLogisticsOrderDetailResponseRoot
    {
        [JsonProperty("aliexpress_logistics_querylogisticsorderdetail_response")]
        public AliExpressLogisticsOrderDetailResponse? AliExpressLogisticsOrderDetailResponse { get; set; }
    }

    public class AliExpressLogisticsOrderDetailResponse
    {
        [JsonProperty("result")]
        public AliExpressLogisticsOrderDetailResponseResult? AliExpressLogisticsOrderDetailResponseResult { get; set; }
    }

    public class AliExpressLogisticsOrderDetailResponseResult
    {
        [JsonProperty("result_list")]
        public AliExpressLogisticsOrderDetailResultList? AliExpressLogisticsOrderDetailResultList { get; set; }
    }

    public class AliExpressLogisticsOrderDetailResultList
    {
        [JsonProperty("aeop_logistics_order_detail_dto")]
        public List<AliExpressLogisticsOrderDetailDto>? AliExpressLogisticsOrderDetailDtos { get; set; }
    }

    public class AliExpressLogisticsOrderDetailDto
    {
        [JsonProperty("channel_code")]
        public string? ChannelCode { get; set; }
        [JsonProperty("gmt_create")]
        public DateTime GmtCreate { get; set; }
        [JsonProperty("logistics_order_id")]
        public long LogisticsOrderId { get; set; }
        [JsonProperty("logistics_status")]
        public string? LogisticStatus { get; set; }
        [JsonProperty("out_order_code")]
        public string? OutOrderCode { get; set; }
        [JsonProperty("trade_order_id")] 
        public long TradeOrderId { get; set; }

    }
}
