using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YapartMarket.Core.DTO;
using YapartMarket.Core.Extensions;

namespace YapartMarket.Core.JsonConverters
{
    public class AliExpressSolutionOrderGetResponseResultConverter : JsonConverter<AliExpressSolutionOrderGetResponseResultDTO>
    {
        public override void WriteJson(JsonWriter writer, AliExpressSolutionOrderGetResponseResultDTO? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override AliExpressSolutionOrderGetResponseResultDTO? ReadJson(JsonReader reader, Type objectType, AliExpressSolutionOrderGetResponseResultDTO? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            existingValue = new AliExpressSolutionOrderGetResponseResultDTO();
            existingValue.FillProperties(jObject);
            existingValue.AliExpressOrderListDTOs = jObject.SelectToken("target_list.order_dto")?.ToObject<List<AliExpressOrderDTO>>();
            return existingValue;
        }
    }
}
