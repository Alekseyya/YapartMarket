using System.Text.Json;
using System;
using YapartMarket.Core.DTO.AliExpress.OrderGetResponse;
using System.Collections.Generic;
using YapartMarket.Core.Extensions;
using System.Globalization;

namespace YapartMarket.Core
{
    public abstract class OrderMessageDeserializer<T> : Deserializer<T>
    {
        protected OrderMessageDeserializer() { }
        protected static DateTime? GetDateTime(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            if (DateTime.TryParse(value, out var result))
                return result;
            if (DateTime.TryParseExact(value, "yyyyMMddTHHmmss.FFFFFF", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out result))
                return result;
            if (DateTime.TryParseExact(value, "yyyyMMddTHHmmssZ", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out result))
                return result;
            if (DateTime.TryParseExact(value, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
                return result;
            return null;
        }

        protected int GetInt(string value)
        {
            if (int.TryParse(value, out var result))
                return result;
            return 0;
        }

        protected decimal GetDecimal(string value)
        {
            if (decimal.TryParse(value, NumberStyles.None, new CultureInfo("en-US"), out var result))
                return result;
            return 0;
        }
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
            if (!result.success)
                throw new FormatException("Order can't be success.");
            var orderList = new List<OrderDto>();
            if (result.target_list.order_dto.IsAny())
                orderList.AddRange(orderRootMessage!.aliexpress_solution_order_get_response.result.target_list.order_dto);
                //ValidateJson(result, orderRootMessage);
            return CreateInstanceFromMessage(orderList);
        }
        //Todo Позже дописать
        //void ValidateJson(Result data, OrderRootDto orderRootMessage)
        //{
        //    var orderList = new List<OrderDto>();
        //    try
        //    {
        //        if (data.target_list.Orders.IsAny())
        //            orderList.AddRange(orderRootMessage!.aliexpress_solution_order_get_response.result.target_list.Orders);
        //    }
        //    catch (Exception ex)
        //    {
        //        //Log.WriteException(ex, correlationId, "sourceMessage:" + textInputMessage, LogLevel.Warning);
        //    }
        //}
        protected abstract T CreateInstanceFromMessage(IReadOnlyList<OrderDto> orders);
    }
}
