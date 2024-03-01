using Newtonsoft.Json;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using YapartMarket.Core.Models.AliProduct;
using System.Collections.Generic;
using YapartMarket.Core.Models.Azure;
using System.Linq;

namespace YapartMarket.BL
{
    public sealed class AliProductRequest : SendRequest<ProductFilter>
    {
        public HttpClient httpClient { get; }
        public AliProductRequest(HttpClient httpClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }
        public async Task<List<ProductResponse>> SendAsync(IReadOnlyList<Product> products, string url)
        {
            var contentValues = products.Select(x => x.Sku).ToList();
            var skip = 0;
            var count = products.Count;
            var response = new List<ProductResponse>();
            while (skip < count)
            {
                try
                {
                    var productFilter = new ProductFilter()
                    {
                        filter = new()
                        {
                            search_content = new()
                            {
                                content_values = contentValues.Skip(skip).Take(50).ToList()!,
                                content_type = "SKU_SELLER_SKU"
                            }
                        },
                        limit = 50
                    };
                    var result = await RequestAsync(productFilter, url, httpClient);
                    response.Add(JsonConvert.DeserializeObject<ProductResponse>(result)!);

                }
                catch (Exception e)
                {
                    return new List<ProductResponse>() { new ProductResponse() { error = e.Message } };
                }
                skip += 50;
            }
            return response;
        }
    }
}
