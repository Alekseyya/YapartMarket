using System.Collections.Generic;
using System.Linq;
using YapartMarket.Core.DateStructures;
using YapartMarket.Core.Models.Azure;
using YapartMarket.Core.Models.Raw;

namespace YapartMarket.Core
{
    public class OrderDeserializer : OrderMessageDeserializer<IReadOnlyList<AliExpressOrder>>
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
                return OrderStatus.Unknown;
            return orderStatus.ToLower() switch
            {
                "unknown" => OrderStatus.Unknown,
                "placeordersuccess" => OrderStatus.PlaceOrderSuccess,
                "paymentpending" => OrderStatus.PaymentPending,
                "waitexaminemoney" => OrderStatus.WaitExamineMoney,
                "waitgroup" => OrderStatus.WaitGroup,
                "waitsendgoods" => OrderStatus.WaitSendGoods,
                "partialsendgoods" => OrderStatus.PartialSendGoods,
                "waitacceptgoods" => OrderStatus.WaitAcceptGoods,
                "incancel" => OrderStatus.InCancel,
                "complete" => OrderStatus.Complete,
                "close" => OrderStatus.Close,
                "finish" => OrderStatus.Finish,
                "infrozen" => OrderStatus.InFrozen,
                "inissue" => OrderStatus.InIssue
            };
        }

        PaymentStatus GetPaymentStatus(string? status)
        {
            if(status == null)
                return PaymentStatus.Unknown;
            return status.ToLower() switch
            {
                "notpaid" => PaymentStatus.NotPaid,
                "hold" => PaymentStatus.Hold,
                "paid" => PaymentStatus.Paid,
                "cancelled" => PaymentStatus.Cancelled,
                "failed" => PaymentStatus.Failed
            };
        }
        protected override IReadOnlyList<AliExpressOrder> CreateInstanceFromMessage(IReadOnlyList<Order> orders)
        {
            var aliOrders = new List<AliExpressOrder>();
            foreach (var order in orders)
            {
                aliOrders.Add(new AliExpressOrder()
                {
                    BuyerName = order.buyer_name,
                    OrderId = order.id,
                    OrderStatus = GetOrderStatus(order.order_display_status),
                    CreateAt = GetDateTime(order.created_at),
                    UpdateAt = GetDateTime(order.updated_at),
                    PaidAt = GetDateTime(order.paid_at),
                    PaymentStatus = GetPaymentStatus(order.payment_status),
                    TotalProductCount = (int)order.order_lines.Select(x=> x.quantity).Aggregate((a, b) => a + b),
                    TotalPayAmount = GetDecimal(order.total_amount),
                    AliExpressOrderDetails = GetOrderDetails(order.order_lines).ToList()
                });
            }
            return aliOrders;
        }
        IReadOnlyList<AliExpressOrderDetail> GetOrderDetails(IReadOnlyList<OrderLine> orderDetails)
        {
            var aliOrderDetails = new List<AliExpressOrderDetail>();
            foreach (var orderDetail in orderDetails)
            {
                aliOrderDetails.Add(new AliExpressOrderDetail()
                {
                    ProductId = GetLong(orderDetail.item_id),
                    ProductName = orderDetail.sku_code,
                    ProductCount = (int)orderDetail.quantity,
                    ItemPrice = GetDecimal(orderDetail.item_price),
                    TotalProductAmount = GetDecimal(orderDetail.total_amount),
                });
            }
            return aliOrderDetails;
        }
    }
}
