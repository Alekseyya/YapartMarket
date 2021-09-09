﻿using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YapartMarket.Core.DTO;
using YapartMarket.Core.Extensions;

namespace YapartMarket.Core.JsonConverters
{
    public class AliExpressOrderProductConverter : JsonConverter<AliExpressOrderProductDTO>
    {
        public override void WriteJson(JsonWriter writer, AliExpressOrderProductDTO? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override AliExpressOrderProductDTO? ReadJson(JsonReader reader, Type objectType, AliExpressOrderProductDTO? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            existingValue = new AliExpressOrderProductDTO();
            existingValue.FillProperties(jObject);
            existingValue.ProductUnitPrice = (double)jObject.SelectToken("product_unit_price.amount");
            existingValue.TotalProductAmount = (double)jObject.SelectToken("product_unit_price.total_product_amount");
            return existingValue;
        }
    }
}
