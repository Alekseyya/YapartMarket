using System.Collections.Generic;

namespace YapartMarket.React.ViewModels
{
    public sealed class ProductExpressInfoViewModel
    {
        public List<ProductExpressViewModel> UpdateProducts { get; set; }
        public List<ProductExpressViewModel> MissingProducts { get; set; }
    }

    public sealed class ProductExpressViewModel
    {
        public string Sku { get; set; }
        public int Count { get; set; }
    }
}
