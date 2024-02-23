using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Contrib.Extensions;

namespace YapartMarket.Core.Models.Azure
{
    public sealed class LogisticServiceOrder
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("order_id")]
        public long OrderId { get; set; }
        [Column("warehouse_name")]
        public string? WarehouseName { get; set; }
        [Column("logistics_service_name")]
        public string? LogisticsServiceName { get; set; }
        [Column("logistics_service_id")]
        public string? LogisticsServiceId { get; set; }
        [Column("delivery_address")]
        public string? DeliveryAddress { get; set; }
    }
}
