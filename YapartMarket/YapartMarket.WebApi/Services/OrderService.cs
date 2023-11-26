using System.Collections.Generic;
using System.Linq;
using YapartMarket.WebApi.ViewModel;

namespace YapartMarket.WebApi.Services
{
    internal sealed class OrderService
    {
        internal IReadOnlyList<AliExpressOrder> Convert(IReadOnlyList<Core.Models.Azure.AliExpressOrder> orders)
        {
            var aliOrders = new List<AliExpressOrder>();
            foreach (var order in orders)
            {
                aliOrders.Add(new AliExpressOrder()
                {
                    BuyerName = order.BuyerName,
                    OrderId = order.OrderId,
                    OrderStatus = order.OrderStatus.ToString(),
                    CreateAt = order.CreateAt,
                    UpdateAt = order.UpdateAt,
                    PaidAt = order.PaidAt,
                    PaymentStatus = order.PaymentStatus.ToString(),
                    TotalProductCount = order.TotalProductCount,
                    TotalPayAmount = order.TotalPayAmount,
                    OrderDetails = GetOrderDetails((IReadOnlyList<Core.Models.Azure.AliExpressOrderDetail>)order.AliExpressOrderDetails).ToList()
                });
            }
            return aliOrders;
        }
        private IReadOnlyList<AliExpressOrderDetail> GetOrderDetails(IReadOnlyList<Core.Models.Azure.AliExpressOrderDetail> orderDetails)
        {
            var aliOrderDetails = new List<AliExpressOrderDetail>();
            foreach (var orderDetail in orderDetails)
            {
                aliOrderDetails.Add(new AliExpressOrderDetail()
                {
                    ProductId = orderDetail.ProductId,
                    ProductName = orderDetail.ProductName,
                    ProductCount = orderDetail.ProductCount,
                    ItemPrice = orderDetail.ItemPrice,
                    TotalProductAmount = orderDetail.TotalProductAmount
                });
            }
            return aliOrderDetails;
        }

    }
}
