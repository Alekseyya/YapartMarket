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
        [Column("updatedAt")]
        [Display(Name = "Время обновления записи")]
        public string UpdatedAt { get; set; }

        public AliExpressProduct AliExpressProduct { get; set; }
    }
}
