﻿using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using YapartMarket.Core.DateStructures;
using YapartMarket.Core.JsonConverters;

namespace YapartMarket.Core.DTO
{
    public class AliExpressGetOrderRoot
    {
        [JsonProperty("aliexpress_solution_order_get_response")]
        public AliExpressSolutionOrderGetResponseDTO AliExpressSolutionOrderGetResponseDTO { get; set; }
    }
    public class AliExpressSolutionOrderGetResponseDTO
    {
        [JsonProperty("result")]
        public AliExpressSolutionOrderGetResponseResultDTO AliExpressSolutionOrderGetResponseResultDto { get; set; }
        [JsonProperty("request_id")]
        public string RequestId { get; set; }
    }

    [JsonConverter(typeof(AliExpressSolutionOrderGetResponseResultConverter))]
    public class AliExpressSolutionOrderGetResponseResultDTO
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        public List<AliExpressOrderListDTO> AliExpressOrderListDTOs { get; set; }
        [JsonProperty("total_count")]
        public int TotalCount { get; set; }
        [JsonProperty("total_page")]
        public int TotalPage { get; set; }
    }

    [JsonConverter(typeof(AliExpressOrderDetailConverter))]
    public class AliExpressOrderListDTO
    {
        [JsonProperty("seller_signer_fullname")]
        public string SellerSignerFullName { get; set; }
        [JsonProperty("seller_login_id")]
        public string SellerLoginId { get; set; }
        public List<AliExpressOrderProductDTO> AliExpressOrderProducts { get; set; }
        [JsonProperty("pay_amount.amount")]
        public int PayAmount { get; set; }
        [JsonProperty("order_status")]
        public OrderStatus OrderStatus { get; set; }
        [JsonProperty("order_id")]
        public long OrderId { get; set; }
        [JsonProperty("logistics_status")]
        public string LogisticsStatus { get; set; }
        [JsonProperty("gmt_update")]
        public DateTime GmtUpdate { get; set; }
        [JsonProperty("gmt_pay_time")] 
        public DateTime GmtPayTime { get; set; }
        [JsonProperty("gmt_create")]
        public DateTime GmtCreate { get; set; }
        [JsonProperty("fund_status")]
        public string FundStatus { get; set; }
        [JsonProperty("frozen_status")]
        public FrozenStatus FrozenStatus { get; set; }
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

    [JsonConverter(typeof(AliExpressOrderProductConverter))]
    public class AliExpressOrderProductDTO
    {
        [JsonProperty("logistics_service_name")]
        public string LogisticsServiceName { get; set; }
        [JsonProperty("order_id")]
        public long OrderId { get; set; }
        [JsonProperty("product_count")]
        public int ProductCount { get; set; }
        [JsonProperty("product_id")]
        public string ProductId { get; set; }
        [JsonProperty("product_name")]
        public string ProductName { get; set; }
        public double ProductUnitPrice { get; set; }
        [JsonProperty("send_goods_operator")]
        public ShipperType SendGoodsOperator { get; set; }
        [JsonProperty("show_status")]
        public OrderStatus ShowStatus { get; set; }
        [JsonProperty("goods_prepare_time")] 
        public int GoodsPrepareTime { get; set; }
        public double TotalProductAmount { get; set; }
    }
}
