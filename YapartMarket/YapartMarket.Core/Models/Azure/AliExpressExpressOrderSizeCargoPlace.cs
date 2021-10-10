using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Contrib.Extensions;

namespace YapartMarket.Core.Models.Azure
{
    public class AliExpressExpressOrderSizeCargoPlace : AliExpressBaseEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("order_id")]
        public long OrderId { get; set; }
        [Column("warehouse_name")]
        public string WarehouseName { get; set; }
        [Column("logistics_service_name")]
        public string LogisticServiceName { get; set; }
        [Column("logistics_timeliness")]
        public string LogisticTimeLines { get; set; }
        [Column("logistics_service_id")]
        public string LogisticServiceId { get; set; }
        [Column("delivery_address")]
        public string DeliveryAddress { get; set; }
        [Column("express_logistics_service")]
        public bool ExpressLogisticsService { get; set; }
        [Column("trial_result")]
        public string TrialResult { get; set; }
    }
}
