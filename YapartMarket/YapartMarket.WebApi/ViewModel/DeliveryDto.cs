using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YapartMarket.WebApi.ViewModel
{
    public class DeliveryDto
    {
        [JsonPropertyName("region")]
        public Region? Region { get; set; }
    }

    public class Region
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("type")]
        [Display(Name = "Тип региона")]
        public string? TypeRegion { get; set; }
        [JsonPropertyName("parent")]
        public ParenRegion? ParentRegion { get; set; }
    }

    public class ParenRegion
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("type")]
        public string? ParentTypeRegion { get; set; }
        [JsonPropertyName("parent")]
        public ParenRegion? ParentRegion { get; set; }
    }


    public enum TypeRegion
    {
        CITY,
        CITY_DISTRICT,
        CONTINENT,
        COUNTRY_DISTRICT,
        METRO_STATION,
        MONORAIL_STATION,
        OTHERS_UNIVERSAL,
        OVERSEAS_TERRITORY,
        REGION,
        SECONDARY_DISTRICT,
        SETTLEMENT,
        SUBJECT_FEDERATION,
        SUBJECT_FEDERATION_DISTRICT,
        SUBURB,
        VILLAGE
    }
}
