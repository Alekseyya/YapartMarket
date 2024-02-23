using System.Collections.Generic;

namespace YapartMarket.Core.DTO.Yandex
{
    public sealed class StockDto
    {
        public long WarehouseId { get; set; }
        public List<string>? Skus { get; set; }
    }
}