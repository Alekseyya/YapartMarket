using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Contrib.Extensions;

namespace YapartMarket.Core.Models.Azure
{ 
    public class AliExpressOrderReceiptInfo
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("order_id")]
        public long OrderId { get; set; }
        [Column("country_name")]
        public string CountryName { get; set; }
        [Column("mobile_no")]
        public string Mobile { get; set; }
        [Column("contact_person")]
        public string ContractPerson { get; set; }
        [Column("phone_country")]
        public string PhoneCountry { get; set; }
        [Column("phone_area")]
        public string PhoneArea { get; set; }
        [Column("province")]
        public string Province { get; set; }
        [Column("address")]
        public string Address { get; set; }
        [Column("phone_number")]
        public string PhoneNumber { get; set; }
        [Column("fax_number")]
        public string FaxNumber { get; set; }
        [Column("detail_address")]
        public string DetailAddress { get; set; }
        [Column("city")]
        public string City { get; set; }
        [Column("country")]
        public string Country { get; set; }
        [Column("address2")]
        public string Address2 { get; set; }
        [Column("fax_country")]
        public string FaxCountry { get; set; }
        [Column("zip")]
        public string Zip { get; set; }
        [Column("fax_area")]
        public string FaxArea { get; set; }
        [Column("localized_address")]
        public string LocalizedAddress { get; set; }
    }
}
