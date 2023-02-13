using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YapartMarket.WebApi.ViewModel
{
    public class OrderDto
    {
        [JsonPropertyName("order")]
        public OrderInfoDto OrderInfoDto { get; set; }
    }

    public class OrderInfoDto
    {
        [JsonPropertyName("creationDate")]
        public string CreationDate { get; set; }
        [JsonPropertyName("currency")]
        public string Currency { get; set; }
        [JsonPropertyName("fake")]
        public bool Fake { get; set; }
        [JsonPropertyName("id")]
        public Int64 Id { get; set; }
        [JsonPropertyName("itemsTotal")]
        public double ItemsTotal { get; set; }
        [JsonPropertyName("paymentType")]
        public string PaymentType { get; set; }
        [JsonPropertyName("paymentMethod")]
        public string PaymentMethod { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("substatus")]
        public string SubStatus { get; set; }
        [JsonPropertyName("taxSystem")]
        public string TaxSystem { get; set; }
        [JsonPropertyName("total")]
        public double Total { get; set; }
        [JsonPropertyName("subsidyTotal")]
        public double SubsidyTotal { get; set; }
        [JsonPropertyName("delivery")]
        public DeliveryOrderDto DeliveryOrderDto { get; set; }
        [JsonPropertyName("items")]
        public List<OrderItemDto> OrderItemsDto { get; set; }
    }

    public class DeliveryOrderDto
    {
        [JsonPropertyName("deliveryPartnerType")]
        public string DeliveryPartnerType { get; set; }
        [JsonPropertyName("deliveryServiceId")]
        public Int64 DeliveryServiceId { get; set; }
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("shipments")]
        public List<ShipmentOrderDto> ShipmentsOrderDto { get; set; }
        [JsonPropertyName("serviceName")]
        public string ServiceName { get; set; }
        [JsonPropertyName("type")]
        [Display(Name = "Способ доставки заказа")]
        public string Type { get; set; }
        [JsonPropertyName("region")]
        public RegionOrderDto RegionOrderDto { get; set; }
        [JsonPropertyName("items")]
        public List<OrderItemDto> OrderItemsDto { get; set; }
        [JsonPropertyName("notes")]
        public string Notes { get; set; }
    }

    public class OrderItemDto
    {
        [JsonPropertyName("id")]
        public Int64 Id { get; set; }
        [JsonPropertyName("feedId")]
        public Int64 FeedId { get; set; }
        [JsonPropertyName("offerId")]
        public string OfferId { get; set; }
        [JsonPropertyName("price")]
        public double Price { get; set; }
        [JsonPropertyName("buyer-price")]
        public double BuyerPrice { get; set; }
        [JsonPropertyName("subsidy")]
        public double Subsidy { get; set; }
        [JsonPropertyName("count")]
        public int Count { get; set; }
        [JsonPropertyName("delivery")]
        public bool Delivery { get; set; }
        [JsonPropertyName("params")]
        public string Params { get; set; }
        [JsonPropertyName("vat")]
        public string Vat { get; set; }
        [JsonPropertyName("fulfilmentShopId")]
        public Int64 FulfilmentShopId { get; set; }
        [JsonPropertyName("sku")]
        public string Sku { get; set; }
        [JsonPropertyName("shopSku")]
        public string ShopSku { get; set; }
        [JsonPropertyName("promos")]
        public List<PromosOrderDto> PromosOrderDto { get; set; }
        [JsonPropertyName("instances")]
        public List<InstanceDto> Instances { get; set; }
    }

    public class InstanceDto
    {
        [JsonPropertyName("cis")]
        public string Cis { get; set; }
    }

    public class PromosOrderDto
    {
        [JsonPropertyName("marketPromoId")]
        public string MarketPromoId { get; set; }
        [JsonPropertyName("subsidy")]
        public float Subsidy { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class RegionOrderDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("parent")]
        public ParentRegionDto ParentRegionDto { get; set; }
    }

    public class ParentRegionDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("parent")]
        public ParentRegionDto RegionDto { get; set; }
    }

    public class ShipmentOrderDto
    {
        [JsonPropertyName("id")]
        public Int64 Id { get; set; }
        [JsonPropertyName("shipmentDate")]
        public string ShipmentDate { get; set; }

        [JsonPropertyName("boxes")]
        public List<OrderBox> OrderBoxes { get; set; }
    }

    public class OrderBox
    {
        [JsonPropertyName("id")]
        public Int64 Id { get; set; }

        [JsonPropertyName("weight")]
        public Int64 Weight { get; set; }

        [JsonPropertyName("width")]
        public Int64 Width { get; set; }

        [JsonPropertyName("height")]
        public Int64 Height { get; set; }

        [JsonPropertyName("depth")]
        public Int64 Depth { get; set; }

        [JsonPropertyName("items")]
        public List<OrderBoxItem> OrderBoxItems { get; set; }
    }

    public class OrderBoxItem
    {
        [JsonPropertyName("id")]
        public Int64 Id { get; set; }

        [JsonPropertyName("count")]
        public int Count { get; set; }
    }
}
