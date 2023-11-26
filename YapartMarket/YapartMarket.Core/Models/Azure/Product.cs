using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YapartMarket.Core.Models.Azure
{
    public class Product
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("sku")]
        [Display(Name = "Артикул")]
        public string Sku { get; set; }
        [Column("type")]
        [Display(Name = "Тип товара")]
        public string Type { get; set; }
        [Column("count")]
        [Display(Name = "Количество")]
        public int Count { get; set; }
        [Column("aliExpressProductId")]
        public long? AliExpressProductId { get; set; }
        [Column("goodsId")]
        public long? GoodsId { get; set; }
        [Column("offerId")]
        public long? OfferId { get; set; }
        [Column("updatedAt")]
        [Display(Name = "Время обновления записи")]
        public string UpdatedAt { get; set; }
        [Column("takeTime")]
        public string TakeTime { get; set; }
        [Column("countExpress")]
        public int CountExpress { get; set; }
        [Column("updateExpress")]
        public string UpdateExpress { get; set; }
        [Column("takeTimeExpress")]
        public string TakeTimeExpress { get; set; }
        public AliExpressProduct AliExpressProduct { get; set; }
    }
}
