using System.Text.Json;
using System;
using YapartMarket.Core.DTO.AliExpress.OrderGetResponse;
using Microsoft.Extensions.Logging;
using YapartMarket.Core.Extensions;
using Result = YapartMarket.Core.DTO.AliExpress.OrderGetResponse.Result;
using System.Collections.Generic;

namespace YapartMarket.Core
{
    abstract class OrderDeserializer<T> : Deserializer<T>
    {
        protected OrderDeserializer() { }

        protected override T DeserializeCore(string data)
        {
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };
            var orderRootMessage = JsonSerializer.Deserialize<OrderRootDto>(data, jsonSerializerOptions);
            if (orderRootMessage == null)
                throw new FormatException("OrderRoot can't be deserialized to null.");
            var result = orderRootMessage.aliexpress_solution_order_get_response.result;
            if (result.success)
                throw new FormatException("Order can't be success.");
            ValidateJson(result);
            return CreateInstanceFromMessage(result);
        }
        void ValidateJson(Result data, OrderRootDto orderRootMessage)
        {
            var orderList = new List<OrderDto>();
            try
            {
                if (data.target_list.Orders.IsAny())
                    orderList.AddRange(orderRootMessage!.aliexpress_solution_order_get_response.result.target_list.Orders);
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, correlationId, "sourceMessage:" + textInputMessage, LogLevel.Warning);
            }
        }
    }
}
