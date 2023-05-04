using System.Collections.Generic;
using System.Linq;
using YapartMarket.Core.DateStructures;
using YapartMarket.Core.Models.Azure;
using YapartMarket.Core.Models.Raw;

namespace YapartMarket.Core
{
    public sealed class OrderDeserializer : OrderMessageDeserializer<IReadOnlyList<AliExpressOrder>>
    {
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
                    SkuId = GetLong(orderDetail.sku_id),
                    ProductName = orderDetail.sku_code,
                    ProductCount = (int)orderDetail.quantity,
                    ItemPrice = GetDecimal(orderDetail.item_price),
                    Height = (int)orderDetail.height,
                    Weight = (int)orderDetail.weight,
                    Width = (int)orderDetail.width,
                    Length = (int)orderDetail.length,
                    TotalProductAmount = GetDecimal(orderDetail.total_amount),
                });
            }
            return aliOrderDetails;
        }
    }
}
