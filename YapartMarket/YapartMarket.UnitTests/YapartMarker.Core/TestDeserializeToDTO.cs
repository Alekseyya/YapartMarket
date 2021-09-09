using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using YapartMarket.Core.DateStructures;
using YapartMarket.Core.DTO;
using YapartMarket.Core.JsonConverters;

namespace YapartMarket.UnitTests.YapartMarker.Core
{
    public class TestDeserializeToDTO
    {
        private readonly string _jsonOrderExpample;

        public TestDeserializeToDTO()
        {
            _jsonOrderExpample = @"{
    ""aliexpress_solution_order_get_response"":{
        ""result"":{
            ""error_message"":""1"",
            ""total_count"":1,
            ""target_list"":{
                ""order_dto"":[
                    {
                        ""timeout_left_time"":120340569,
                        ""seller_signer_fullname"":""cn1234"",
                        ""seller_operator_login_id"":""cn1234"",
                        ""seller_login_id"":""cn1234"", 
                        ""product_list"":{
                            ""order_product_dto"":[
                                {
                                    ""total_product_amount"":{
                                        ""currency_code"":""USD"",
                                        ""amount"":""1.01""
                                    },
                                    ""son_order_status"":""PLACE_ORDER_SUCCESS"",
                                    ""sku_code"":""12"",
                                    ""show_status"":""PLACE_ORDER_SUCCESS"",
                                    ""send_goods_time"":""2017-10-12 12:12:12"",
                                    ""send_goods_operator"":""WAREHOUSE_SEND_GOODS"",
                                    ""product_unit_price"":{
                                        ""currency_code"":""USD"",
                                        ""amount"":""1.01""
                                    },
                                    ""product_unit"":""piece"",
                                    ""product_standard"":"""",
                                    ""product_snap_url"":""http:\/\/www.aliexpress.com:1080\/snapshot\/null.html?orderId\\u003d1160045860056286"",
                                    ""product_name"":""mobile"",
                                    ""product_img_url"":""http:\/\/g03.a.alicdn.com\/kf\/images\/eng\/no_photo.gif"",
                                    ""product_id"":2356980,
                                    ""product_count"":1,
                                    ""order_id"":5013302654414510,
                                    ""money_back3x"":false,
                                    ""memo"":""1"",
                                    ""logistics_type"":""EMS"",
                                    ""logistics_service_name"":""EMS"",
                                    ""logistics_amount"":{
                                        ""currency_code"":""USD"",
                                        ""amount"":""1.01""
                                    },
                                    ""issue_status"":""END_ISSUE"",
                                    ""issue_mode"":""w"",
                                    ""goods_prepare_time"":3,
                                    ""fund_status"":""WAIT_SELLER_CHECK"",
                                    ""freight_commit_day"":""27"",
                                    ""escrow_fee_rate"":""0.01"",
                                    ""delivery_time"":""5-10"",
                                    ""child_id"":23457890,
                                    ""can_submit_issue"":false,
                                    ""buyer_signer_last_name"":""1"",
                                    ""buyer_signer_first_name"":""1"",
                                    ""afflicate_fee_rate"":""0.03""
                                }
                            ]
                        },
                        ""phone"":false,
                        ""payment_type"":""ebanx101"",
                        ""pay_amount"":{
                            ""currency_code"":""USD"",
                            ""amount"":""1.01""
                        },
                        ""order_status"":""PLACE_ORDER_SUCCESS"",
                        ""order_id"":1160045860056286,
                        ""order_detail_url"":""http"",
                        ""logistics_status"":""NO_LOGISTICS"",
                        ""logisitcs_escrow_fee_rate"":""1"",
                        ""loan_amount"":{
                            ""currency_code"":""USD"",
                            ""amount"":""1.01""
                        },
                        ""left_send_good_min"":""1"",
                        ""left_send_good_hour"":""1"",
                        ""left_send_good_day"":""1"",
                        ""issue_status"":""END_ISSUE"",
                        ""has_request_loan"":false,
                        ""gmt_update"":""2017-10-12 12:12:12"",
                        ""gmt_send_goods_time"":""2017-10-12 12:12:12"",
                        ""gmt_pay_time"":""2017-10-12 12:12:12"",
                        ""gmt_create"":""2017-10-12 12:12:12"",
                        ""fund_status"":""WAIT_SELLER_CHECK"",
                        ""frozen_status"":""IN_FROZEN"",
                        ""escrow_fee_rate"":1,
                        ""escrow_fee"":{
                            ""currency_code"":""USD"",
                            ""amount"":""1.01""
                        },
                        ""end_reason"":""buyer_confirm_goods"",
                        ""buyer_signer_fullname"":""test"",
                        ""buyer_login_id"":""test"",
                        ""biz_type"":""AE_RECHARGE"",
                        ""offline_pickup_type"":""RU_OFFLINE_SELF_PICK_UP_EXPRESSION""
                    }
                ]
            },
            ""page_size"":1,
            ""error_code"":""1"",
            ""current_page"":1,
            ""total_page"":1,
            ""success"":true,
            ""time_stamp"":""1""
        }
    }
}";
        }
        [Fact]
        public void TestDeserializeDTO_AliExpressBatchProductInventoryUpdateResponseDTO()
        {
            //act
            var json = @"{
    ""aliexpress_solution_batch_product_inventory_update_response"":{
        ""update_error_code"":"""",
        ""update_error_message"":"""",
        ""update_success"": true,
        ""update_failed_list"":{
            ""synchronize_product_response_dto"":[
                {
                    ""error_code"":"""",
                    ""error_message"":"""",
                    ""product_id"": 1
                }
            ]
        },
        ""update_successful_list"":{
            ""synchronize_product_response_dto"":[
                {
                    ""product_id"": 1
                }
            ]
        }
    }
}";
            //arrange
            var aliExpressBatchProductInventoryUpdateResponseDto = JsonConvert.DeserializeObject<AliExpressBatchProductInventoryUpdateResponseDTO>(json);
            //assert
            Assert.NotNull(aliExpressBatchProductInventoryUpdateResponseDto);
        }

        [Fact]
        public void TestDeserializeDTO_AliExpressOrder_NotNullBasicFields()
        {
            //arrange
            var json = _jsonOrderExpample;
            //act
            var jsonObject = JObject.Parse(json);
            var orderJson = jsonObject.SelectToken("aliexpress_solution_order_get_response.result.target_list.order_dto")?[0]?.ToString();
            var order = JsonConvert.DeserializeObject<AliExpressOrderListDTO>(orderJson);
            //assert
            Assert.NotNull(orderJson);
            Assert.NotEmpty(orderJson);
            Assert.NotNull(order);
        }

        [Fact]
        public void TestDeserializeDTO_AliExpressOrder_NotNullProductListProperty()
        {
            //arrange
            var json = _jsonOrderExpample;
            //act
            var jsonObject = JObject.Parse(json);
            var orderJson = jsonObject.SelectToken("aliexpress_solution_order_get_response.result.target_list.order_dto")?[0]?.ToString();
            var order = JsonConvert.DeserializeObject<AliExpressOrderListDTO>(orderJson, new AliExpressOrderDetailConverter());
            //assert
            Assert.NotNull(order);
            Assert.NotNull(order.AliExpressOrderProducts);
            Assert.True(order.AliExpressOrderProducts.Any());
        }

        [Fact]
        public void TestDeserializeDTO_AliExpressOrder_DeserializeProductOrderIdToLong()
        {
            //arrange
            var json = _jsonOrderExpample;
            //act
            var jsonObject = JObject.Parse(json);
            var orderJson = jsonObject.SelectToken("aliexpress_solution_order_get_response.result.target_list.order_dto")?[0]?.ToString();
            Func<AliExpressOrderListDTO> func = () => JsonConvert.DeserializeObject<AliExpressOrderListDTO>(orderJson, new AliExpressOrderDetailConverter());
            //assert
            var jsonReaderException = Assert.Throws<JsonReaderException>(func);
            Assert.DoesNotContain("Could not convert to integer", jsonReaderException.Message);
        }

        [Fact]
        public void TestDeserializeDTO_AliExpressOrder_DeserializeDateType()
        {
            //arrange
            var json = _jsonOrderExpample;
            //act
            var jsonObject = JObject.Parse(json);
            var orderJson = jsonObject.SelectToken("aliexpress_solution_order_get_response.result.target_list.order_dto")?[0]?.ToString();
            var aliExpressOrder = JsonConvert.DeserializeObject<AliExpressOrderListDTO>(orderJson, new AliExpressOrderDetailConverter());
            Assert.NotNull(aliExpressOrder);
            Assert.True(aliExpressOrder.GmtUpdate is DateTime);
        }

        [Fact]
        public void TestDeserializeDTO_AliExpressOrder_DeserializeEnumType()
        {
            //arrange
            var json = _jsonOrderExpample;
            //act
            var jsonObject = JObject.Parse(json);
            var orderJson = jsonObject.SelectToken("aliexpress_solution_order_get_response.result.target_list.order_dto")?[0]?.ToString();
            var aliExpressOrder = JsonConvert.DeserializeObject<AliExpressOrderListDTO>(orderJson, new AliExpressOrderDetailConverter());
            Assert.NotNull(aliExpressOrder);
            Assert.True(aliExpressOrder.OrderStatus is OrderStatus.PLACE_ORDER_SUCCESS);
            Assert.True(aliExpressOrder.AliExpressOrderProducts.First().ShowStatus is OrderStatus.PLACE_ORDER_SUCCESS);
        }

        [Fact]
        public void TestDeserializeDTO_AliExpressOrder_DeserializeProductUnitPrice_Amount()
        {
            //arrange
            var json = _jsonOrderExpample;
            //act
            var jsonObject = JObject.Parse(json);
            var orderJson = jsonObject.SelectToken("aliexpress_solution_order_get_response.result.target_list.order_dto")?[0]?.ToString();
            var aliExpressOrder = JsonConvert.DeserializeObject<AliExpressOrderListDTO>(orderJson, new AliExpressOrderDetailConverter());
            Assert.NotNull(aliExpressOrder);
            Assert.Equal(1.01, aliExpressOrder.AliExpressOrderProducts.First().ProductUnitPrice);
        }
        [Fact]
        public void TestDeserializeDTO_AliExpressOrder_Deserialize_Product_Total_Amount()
        {
            //arrange
            var json = _jsonOrderExpample;
            //act
            var jsonObject = JObject.Parse(json);
            var orderJson = jsonObject.SelectToken("aliexpress_solution_order_get_response.result.target_list.order_dto")?[0]?.ToString();
            var aliExpressOrder = JsonConvert.DeserializeObject<AliExpressOrderListDTO>(orderJson, new AliExpressOrderDetailConverter());
            Assert.NotNull(aliExpressOrder);
            Assert.Equal(1.01, aliExpressOrder.AliExpressOrderProducts.First().TotalProductAmount);
        }
    }
}
