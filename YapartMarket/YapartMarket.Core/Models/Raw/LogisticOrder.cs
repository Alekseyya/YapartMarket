using System.Collections.Generic;

namespace YapartMarket.Core.Models.Raw
{
    public sealed class LogisticOrder
    {
        public List<LogisticOrderItem>? orders { get; set; }
    }

    public class LogisticOrderItemInfo
    {
        public int quantity { get; set; }
        public long sku_id { get; set; }
    }

    public class LogisticOrderItem
    {
        public long trade_order_id { get; set; }
        public int total_length { get; set; }
        public int total_width { get; set; }
        public int total_height { get; set; }
        public double total_weight { get; set; }
        public List<LogisticOrderItemInfo>? items { get; set; }
    }
}
