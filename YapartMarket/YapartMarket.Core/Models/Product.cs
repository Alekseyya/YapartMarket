namespace YapartMarket.Core.Models
{
    public sealed record Product
    {
        public int Id { get; set; }
        public string? Article { get; set; }
        public string? ShortArticle { get; set; }
        public string? Descriptions { get; set; }
        public decimal Price { get; set; }
        public int DaysDelivery { get; set; }
        //cross price
        public decimal OldPrice { get; set; }
        //show to popular or not
        public bool Popular { get; set; }
        // additional characteristic
        public string? Characteristic { get; set; }
        //for seo
        public string? Brief { get; set; }
        //see on layout
        public bool Show { get; set; }
        //see in discount tab
        public bool Discount { get; set; }
        public string? Keywords { get; set; }
        public bool RemoveMarketplace { get; set; }

        public int? BrandId { get; set; }

        public int? CategoryId { get; set; }
    }
}
