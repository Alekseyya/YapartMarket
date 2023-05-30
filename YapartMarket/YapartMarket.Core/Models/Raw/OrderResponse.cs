using System.Collections.Generic;

namespace YapartMarket.Core.Models.Raw
{
    public sealed class OrderResponse
    {
        public OrderData data { get; set; }
        public object error { get; set; }
    }

    public sealed class OrderData
    {
        public List<ResponseOrder> orders { get; set; }
        public OrderErrors errors { get; set; }
    }

    public sealed class OrderErrors
    {
        public object common_errors { get; set; }
        public object item_errors { get; set; }
    }

    public sealed class Line
    {
        public long sku_id { get; set; }
        public int product_source_id { get; set; }
        public int quantity { get; set; }
    }

    public sealed class ResponseLogisticOrder
    {
        public int id { get; set; }
        public List<Line> lines { get; set; }
    }

    public sealed class ResponseOrder
    {
        public long trade_order_id { get; set; }
        public List<LogisticOrder> logistic_orders { get; set; }
    }
}
