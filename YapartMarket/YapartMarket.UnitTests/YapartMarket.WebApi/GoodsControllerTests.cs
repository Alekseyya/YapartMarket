using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace YapartMarket.UnitTests.YapartMarket.WebApi
{
    public sealed class GoodsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public GoodsControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }
        [Fact]
        public async Task New_SendModel_ReturnSuccess()
        {
            //Arrange
            var json = @"{
    ""data"": {
        ""merchantId"": 2020,
        ""shipments"": [
            {
                ""shipmentId"": ""631026625"",
                ""confirmationDate"": ""2023-01-26T17:00:00+03:00"",
                ""packingDate"": ""2023-01-28T18:30:00+03:00"",
                ""shipmentDate"": ""2023-01-26T13:09:37+03:00"",
                ""customerOrderId"": ""664037650"",
                ""items"": [
                    {
                        ""itemIndex"": ""1"",
                        ""goodsId"": ""100022889385"",
                        ""offerId"": ""85030-0"",
                        ""itemId"": ""46465851903"",
                        ""itemName"": ""Концентрат для стеклоочистителей 1:10 // 1000 мл - лимон"",
                        ""price"": 1098,
                        ""finalPrice"": 1098,
                        ""discounts"": [],
                        ""priceAdjustments"": [],
                        ""quantity"": 1,
                        ""taxRate"": ""NOT"",
                        ""reservationPerformed"": true,
                        ""isDigitalMarkRequired"": false
                    }
                ],
                ""label"": {
                    ""deliveryId"": ""697532132"",
                    ""region"": ""Москва"",
                    ""city"": ""Москва"",
                    ""address"": ""Москва, улица Большая Бронная, 6а ст1, кв. 22"",
                    ""fullName"": ""Тест Тест"",
                    ""merchantName"": ""Индивидуальный предприниматель Инихова Лидия Сергеевна"",
                    ""merchantId"": 2020,
                    ""shipmentId"": ""631026625"",
                    ""shippingDate"": ""2023-01-27T18:00:00+03:00"",
                    ""deliveryType"": ""Доставка курьером"",
                    ""labelText"": ""<!DOCTYPE html>\n<html>\n<head>\n    <meta charset=\""UTF-8\""/>\t\n    <title>Маркировочный лист</title>    ...""
                },
                ""shipping"": {
                    ""shippingPoint"": 4003978,
                    ""shippingDate"": ""2023-01-27T18:00:00+03:00"",
                    ""shippingInterval"": {
                        ""dateFrom"": ""2023-01-27T09:00:00+03:00"",
                        ""dateTo"": ""2023-01-27T18:00:00+03:00""
                    }
                },
                ""delivery"": {
                    ""deliveryDate"": ""2023-01-29T00:00:00+03:00"",
                    ""deliveryInterval"": {
                        ""dateFrom"": ""2023-01-29T10:00:00+03:00"",
                        ""dateTo"": ""2023-01-29T18:00:00+03:00""
                    }
                },
                ""fulfillmentMethod"": ""FULFILLMENT_BY_MERCHANT""
            }
        ]
    },
    ""meta"": {
        ""source"": ""OMS""
    }
}";
            var url = "api/Goods/order/new";
            //Act
            
            var client = _factory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            var uTF8Encoding = new System.Text.UTF8Encoding(false, true);
            var bytes = uTF8Encoding.GetBytes(json);
            HttpContent content = new ByteArrayContent(bytes, 0, bytes.Length);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            httpRequestMessage.Content = content;

            var responseMessage = await client.SendAsync(httpRequestMessage).ConfigureAwait(true);
            //Assert
            var message = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            Assert.Equal(responseMessage.StatusCode, HttpStatusCode.OK);
            Assert.True(message.Contains("success"));
        }
    }
}
