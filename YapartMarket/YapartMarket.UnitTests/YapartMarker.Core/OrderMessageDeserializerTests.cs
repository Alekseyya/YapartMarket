using Xunit;
using YapartMarket.Core;
using System.Text.Json;
using YapartMarket.Core.Models.Raw;

namespace YapartMarket.UnitTests.YapartMarker.Core
{
    public class OrderMessageDeserializerTests : OrderDeserializer
    {
        [Fact]
        public void Test_Attribute()
        {
            var body = @"{
    ""data"": {
        ""total_count"": 1,
        ""orders"": [
            {
                ""id"": 2212094469888866,
                ""created_at"": ""2022-12-09T20:57:26.606005+00:00"",
                ""paid_at"": ""2022-12-09T20:57:37.450422+00:00"",
                ""updated_at"": ""2022-12-09T20:57:38.502102+00:00"",
                ""status"": ""Created"",
                ""payment_status"": ""Hold"",
                ""delivery_status"": ""Init"",
                ""delivery_address"": ""Россия, Нижегородская обл, Арзамас г, ПМС-73,3,24, 607220"",
                ""antifraud_status"": ""Passed"",
                ""buyer_country_code"": ""RU"",
                ""buyer_name"": ""Кукушкин Сергей Викторович "",
                ""order_display_status"": ""WaitSendGoods"",
                ""buyer_phone"": ""+79200358382"",
                ""order_lines"": [
                    {
                        ""id"": 2212095463331348,
                        ""item_id"": ""1005002891582458"",
                        ""sku_id"": ""12000029610583328"",
                        ""sku_code"": ""CHERY.S.6314003"",
                        ""name"": ""Подкрылок с шумоизоляцией CHERY Tiggo 5, 2014-2016,  (задний левый), CHERY.S.6314003"",
                        ""img_url"": ""https://ae04.alicdn.com/kf/S6ac980cb81ee417c9c20e9b312b4809e8.jpg_50x50.jpg"",
                        ""item_price"": 34000,
                        ""quantity"": 1.0,
                        ""total_amount"": 32980,
                        ""properties"": [
                            ""Autofamily"",
                            ""100000015"",
                            ""sell_by_piece"",
                            ""1"",
                            ""0.650"",
                            ""30"",
                            ""10"",
                            ""20""
                        ],
                        ""buyer_comment"": null,
                        ""height"": 10.0,
                        ""weight"": 650.0,
                        ""width"": 20.0,
                        ""length"": 30.0,
                        ""issue_status"": ""NoDispute"",
                        ""promotions"": [
                            {
                                ""ae_promotion_id"": 5000000071015191,
                                ""ae_activity_id"": 5000000071015191,
                                ""code"": null,
                                ""promotion_type"": ""FlexiCoin"",
                                ""discount"": 1020,
                                ""discount_currency"": ""RUB"",
                                ""original_discount"": 1020,
                                ""original_discount_currency"": ""RUB"",
                                ""promotion_target"": ""Sale"",
                                ""budget_sponsor"": ""Seller""
                            }
                        ],
                        ""order_line_fees"": null
                    }
                ],
                ""total_amount"": 64559,
                ""seller_comment"": null,
                ""fully_prepared"": false,
                ""finish_reason"": null,
                ""cut_off_date"": null,
                ""cut_off_date_histories"": null,
                ""shipping_deadline"": null,
                ""next_cut_off_date"": null,
                ""pre_split_postings"": null,
                ""logistic_orders"": null,
                ""commission"": null
            }
        ]
    },
    ""error"": null
}";
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };
            var orderRootMessage = JsonSerializer.Deserialize<OrderRoot>(body, jsonSerializerOptions);
            var result = CreateInstanceFromMessage(orderRootMessage?.data.orders);
            Assert.NotNull(result);
        }
    }
}
