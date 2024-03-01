using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Contrib.Extensions;

namespace YapartMarket.Core.Models.Azure
{
    public class AliExpressOrderLogisticRedefining
    {
        [Key]
        public int Id { get; set; }
        [Column("recommend_order")]
        public long RecommendOrder { get; set; }
        [Column("tracking_no_regex")]
        public string? TrackingNoRegex { get; set; }
        [Column("min_process_day")]
        public int MinProcessDay { get; set; }
        [Column("logistic_company")]
        public string? LogisticCompany { get; set; }
        [Column("max_process_day")]
        public int MaxProcessDay { get; set; }
        [Column("display_name")]
        public string? DisplayName { get; set; }
        [Column("service_name")]
        public string? ServiceName { get; set; }
    }
}
