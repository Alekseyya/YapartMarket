using System.Collections.Generic;
using Newtonsoft.Json;

namespace YapartMarket.Core.DTO.AliExpress.RedefininDomesticLogicalCompany
{
    public class RedefiningDomesticLogicalCompanyRoot
    {
        [JsonProperty("aliexpress_logistics_redefining_qureywlbdomesticlogisticscompany_response")]
        public RedefiningQueryLogisticCompanyResponse Result { get; set; }
    }

    public class RedefiningQueryLogisticCompanyResponse
    {
        [JsonProperty("result_list")]
        public DomesticLogicalCompanyList DomesticLogicalCompanyList { get; set; }
    }

    public class DomesticLogicalCompanyList
    {
        [JsonProperty("result")]
        public List<DomesticLogicalCompanyInfo> DomesticLogicalCompanyInfo { get; set; }
    }

    public class DomesticLogicalCompanyInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("company_id")]
        public int CompanyId { get; set; }
        [JsonProperty("company_code")]
        public string CompanyCode { get; set; }
    }
}
