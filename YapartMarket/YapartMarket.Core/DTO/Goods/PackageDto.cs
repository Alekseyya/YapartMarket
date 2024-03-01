using System.Collections.Generic;

namespace YapartMarket.Core.DTO.Goods
{
    public class PackageDto
    {
        public string? ShipmentId { get; set; }
        public int OrderId { get; set; }
        public List<ItemDto>? Items { get; set; }
    }

    public class ItemDto
    {
        public int ItemIndex { get; set; }
        public int BoxIndex { get; set; }
        public string? BoxCode { get; set; }
        public string? DigitalMarks { get; set; }
    }
}
