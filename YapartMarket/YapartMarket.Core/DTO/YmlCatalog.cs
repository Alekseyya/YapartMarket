using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace YapartMarket.Core.DTO
{
    [XmlRoot(ElementName = "currency")]
    public class Currency
    {

        [XmlAttribute(AttributeName = "id")]
        public string? Id { get; init; }

        [XmlAttribute(AttributeName = "rate")]
        public int Rate { get; init; }
    }

    [XmlRoot(ElementName = "currencies")]
    public class Currencies
    {

        [XmlElement(ElementName = "currency")]
        public List<Currency>? Currency { get; init; }
    }

    [XmlRoot(ElementName = "category")]
    public class Category
    {

        [XmlAttribute(AttributeName = "id")]
        public int Id { get; init; }

        [XmlText]
        public string? Text { get; init; }

        [XmlAttribute(AttributeName = "parentId")]
        public int ParentId { get; init; }
    }

    [XmlRoot(ElementName = "categories")]
    public class Categories
    {

        [XmlElement(ElementName = "category")]
        public List<Category>? Category { get; init; }
    }

    [XmlRoot(ElementName = "option")]
    public class Option
    {

        [XmlAttribute(AttributeName = "days")]
        public int Days { get; init; }

        [XmlAttribute(AttributeName = "order-before")]
        public int OrderBefore { get; init; }
    }

    [XmlRoot(ElementName = "shipment-options")]
    public class Shipmentoptions
    {

        [XmlElement(ElementName = "option")]
        public Option? Option { get; init; }
    }

    [XmlRoot(ElementName = "outlet")]
    public class Outlet
    {

        [XmlAttribute(AttributeName = "id")]
        public int Id { get; set; }

        [XmlAttribute(AttributeName = "instock")]
        public int Instock { get; set; }
    }

    [XmlRoot(ElementName = "outlets")]
    public class Outlets
    {

        [XmlElement(ElementName = "outlet")]
        public Outlet? Outlet { get; init; }
    }

    [XmlRoot(ElementName = "param")]
    public class Param
    {
        [XmlAttribute(AttributeName = "name")]
        public string? Name { get; init; }

        [XmlText]
        public string? Text { get; init; }
    }

    [XmlRoot(ElementName = "offer")]
    public class Offer
    {

        [XmlElement(ElementName = "url")]
        public string? Url { get; init; }

        [XmlElement(ElementName = "name")]
        public string? Name { get; init; }

        [XmlElement(ElementName = "price")]
        public int Price { get; init; }

        [XmlElement(ElementName = "oldprice")]
        public int Oldprice { get; init; }

        [XmlElement(ElementName = "categoryId")]
        public int CategoryId { get; init; }

        [XmlElement(ElementName = "picture")]
        public List<string>? Picture { get; init; }

        [XmlElement(ElementName = "vat")]
        public string? Vat { get; init; }

        [XmlElement(ElementName = "shipment-options")]
        public Shipmentoptions? Shipmentoptions { get; init; }

        [XmlElement(ElementName = "outlets")]
        public Outlets? Outlets { get; init; }

        [XmlElement(ElementName = "vendor")]
        public string? Vendor { get; init; }

        [XmlElement(ElementName = "vendorCode")]
        public string? VendorCode { get; init; }

        [XmlElement(ElementName = "model")]
        public string? Model { get; init; }

        [XmlElement(ElementName = "description")]
        public string? Description { get; init; }

        [XmlElement(ElementName = "param")]
        public List<Param>? Param { get; init; }

        [XmlElement(ElementName = "barcode")]
        public string? Barcode { get; init; }

        [XmlAttribute(AttributeName = "id")]
        public int Id { get; init; }

        [XmlAttribute(AttributeName = "available")]
        public bool Available { get; init; }

        [XmlText]
        public string? Text { get; init; }
    }

    [XmlRoot(ElementName = "offers")]
    public class Offers
    {

        [XmlElement(ElementName = "offer")]
        public List<Offer>? Offer { get; init; }
    }

    [XmlRoot(ElementName = "shop")]
    public class Shop
    {

        [XmlElement(ElementName = "name")]
        public string? Name { get; init; }

        [XmlElement(ElementName = "company")]
        public string? Company { get; init; }

        [XmlElement(ElementName = "url")]
        public string? Url { get; init; }

        [XmlElement(ElementName = "currencies")]
        public Currencies? Currencies { get; init; }

        [XmlElement(ElementName = "categories")]
        public Categories? Categories { get; init; }

        [XmlElement(ElementName = "shipment-options")]
        public Shipmentoptions? Shipmentoptions { get; init; }

        [XmlElement(ElementName = "offers")]
        public Offers? Offers { get; init; }
    }

    [XmlRoot(ElementName = "yml_catalog")]
    public class YmlCatalog
    {

        [XmlElement(ElementName = "shop")]
        public Shop? Shop { get; init; }

        [XmlIgnore]
        public DateTime Date { get; init; }

        [XmlElement("date")]
        public string DataString
        {
            get { return this.Date.ToString("YYYY-MM-DD hh:mm"); }
            init { this.Date = DateTime.Parse(value); }
        }

        [XmlText]
        public string? Text { get; init; }
    }
}
