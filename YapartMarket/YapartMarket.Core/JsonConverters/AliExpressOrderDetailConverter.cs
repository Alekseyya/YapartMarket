using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YapartMarket.Core.DTO;
using YapartMarket.Core.Extensions;

namespace YapartMarket.Core.JsonConverters
{
    public class AliExpressOrderDetailConverter : JsonConverter<AliExpressOrderDTO>
    {
        public override void WriteJson(JsonWriter writer, AliExpressOrderDTO value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override AliExpressOrderDTO ReadJson(JsonReader reader, Type objectType, AliExpressOrderDTO existingValue,
            bool hasExistingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            existingValue = new AliExpressOrderDTO();
            existingValue.FillProperties(jObject);
            existingValue.AliExpressOrderProducts = jObject.SelectToken("product_list.order_product_dto").ToObject<List<AliExpressOrderProductDTO>>();
            return existingValue;

        }
    }
}
