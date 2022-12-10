using System.Collections.Generic;
using System.Linq;
using YapartMarket.Core.DateStructures;
using YapartMarket.Core.DTO.AliExpress.OrderGetResponse;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Core
{
    public sealed class OrderDeserializer : OrderMessageDeserializer<IReadOnlyList<AliExpressOrder>>
    {
        LogisticsStatus GetLogisticStatus(string? status)
        {
            if (status == null)
                return LogisticsStatus.UNKNOWN;
            return status.ToLower() switch
            {
                "wait_seller_send_goods" => LogisticsStatus.WAIT_SELLER_SEND_GOODS,
                "seller_send_part_goods" => LogisticsStatus.SELLER_SEND_PART_GOODS,
                "seller_send_goods" => LogisticsStatus.SELLER_SEND_GOODS,
                "buyer_accept_goods" => LogisticsStatus.BUYER_ACCEPT_GOODS,
                "no_logistics" => LogisticsStatus.NO_LOGISTICS
            };
        }

        BizType GetBizType(string? status)
        {
            if(status == null)
                return BizType.UNKNOWN;
            return status.ToLower() switch
            {
                "ae_common" => BizType.AE_COMMON,
                "ae_trial" => BizType.AE_TRIAL,
                "ae_recharge" => BizType.AE_RECHARGE,
            };
        }

        OrderStatus GetOrderStatus(string orderStatus)
        {
            if(orderStatus == null)
                return OrderStatus.UNKNOWN;
            return orderStatus.ToLower() switch
            {
                "place_order_success" => OrderStatus.PLACE_ORDER_SUCCESS,
                "in_cancel" => OrderStatus.IN_CANCEL,
                "wait_seller_send_goods" => OrderStatus.WAIT_SELLER_SEND_GOODS,
                "seller_part_send_goods" => OrderStatus.SELLER_PART_SEND_GOODS,
                "wait_buyer_accept_goods" => OrderStatus.WAIT_BUYER_ACCEPT_GOODS,
                "fund_processing" => OrderStatus.FUND_PROCESSING,
                "in_issue" => OrderStatus.IN_ISSUE,
                "in_frozen" => OrderStatus.IN_FROZEN,
                "wait_seller_examine_money" => OrderStatus.WAIT_SELLER_EXAMINE_MONEY,
                "risk_control" => OrderStatus.RISK_CONTROL,
                "finish" => OrderStatus.FINISH,
            };
        }

        FundStatus GetFundStatus(string? status)
        {
            if(status == null)
                return FundStatus.UNKNOWN;
            return status.ToLower() switch
            {
                "not_pay" => FundStatus.NOT_PAY,
                "pay_success" => FundStatus.PAY_SUCCESS,
                "wait_seller_check" => FundStatus.WAIT_SELLER_CHECK
            };
        }
        FrozenStatus GetFrozenStatus(string status)
        {
            if(status == null)
                return FrozenStatus.UNKNOWN;
            return status.ToLower() switch
            {
                "no_frozen" => FrozenStatus.NO_FROZEN,
                "in_frozen" => FrozenStatus.IN_FROZEN
            };
        }

        ShipperType GetShipperType(string shipperType)
        {
            if(shipperType == null)
                return ShipperType.UNKNOWN;
            return shipperType.ToLower() switch
            {
                "seller_send_goods" => ShipperType.SELLER_SEND_GOODS,
                "warehouse_send_goods" => ShipperType.WAREHOUSE_SEND_GOODS
            };
        }
        protected override IReadOnlyList<AliExpressOrder> CreateInstanceFromMessage(IReadOnlyList<OrderDto> orders)
        {
            var aliOrders = new List<AliExpressOrder>();
            foreach (var order in orders)
            {
                aliOrders.Add(new AliExpressOrder()
                {
                    SellerSignerFullName = order.SellerSignerFullname,
                    SellerLoginId = order.seller_login_id,
                    OrderId = order.order_id,
                    LogisticsStatus = GetLogisticStatus(order.logistics_status),
                    BizType = GetBizType(order.biz_type),
                    GmtPayTime = GetDateTime(order.gmt_pay_time),
                    EndReason = order.end_reason,
                    TotalProductCount = order.product_list.order_product_dto.Select(x=> x.product_count).Aggregate((a, b) => a + b),
                    TotalPayAmount = order.product_list.order_product_dto.Select(t => GetDecimal(t.total_product_amount.amount)).Aggregate((a, b) => a + b),
                    OrderStatus = GetOrderStatus(order.order_status),
                    GmtCreate = GetDateTime(order.gmt_create),
                    GmtUpdate = GetDateTime(order.gmt_update),
                    FundStatus = GetFundStatus(order.fund_status),
                    FrozenStatus = GetFrozenStatus(order.frozen_status),
                    AliExpressOrderDetails = GetOrderDetails(order.product_list.order_product_dto).ToList()
                });
            }
            return aliOrders;
        }
        IReadOnlyList<AliExpressOrderDetail> GetOrderDetails(IReadOnlyList<OrderProductDto> orderDetails)
        {
            var aliOrderDetails = new List<AliExpressOrderDetail>();
            foreach (var orderDetail in orderDetails)
            {
                aliOrderDetails.Add(new AliExpressOrderDetail()
                {
                    LogisticsServiceName = orderDetail.logistics_service_name,
                    //OrderId = orderDetail.order_id,
                    ProductCount = orderDetail.product_count,
                    ProductId = orderDetail.product_id,
                    ProductName = orderDetail.product_name,
                    ProductUnitPrice = GetDecimal(orderDetail.product_unit_price.amount),
                    SendGoodsOperator = GetShipperType(orderDetail.send_goods_operator),
                    ShowStatus = GetOrderStatus(orderDetail.show_status),
                    GoodsPrepareDays = orderDetail.goods_prepare_time,
                    TotalProductAmount = GetDecimal(orderDetail.total_product_amount.amount)
                });
            }
            return aliOrderDetails;
        }
    }
}
