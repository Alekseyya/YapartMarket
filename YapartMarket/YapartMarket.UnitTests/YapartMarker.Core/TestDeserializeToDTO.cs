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
    ""biz_type"": ""AE_COMMON"",
    ""buyer_login_id"": ""ru2943540738mooae"",
    ""buyer_signer_fullname"": ""4144 user"",
    ""end_reason"": """",
    ""frozen_status"": ""NO_FROZEN"",
    ""fund_status"": ""NOT_PAY"",
    ""gmt_create"": ""2021-09-12 11:03:37"",
    ""gmt_update"": ""2021-09-12 11:03:37"",
    ""has_request_loan"": false,
    ""issue_status"": ""NO_ISSUE"",
    ""logisitcs_escrow_fee_rate"": """",
    ""order_id"": 5013393113256737,
    ""order_status"": ""PLACE_ORDER_SUCCESS"",
    ""phone"": false,
    ""product_list"": {
        ""order_product_dto"": [
            {
                ""can_submit_issue"": false,
                ""child_id"": 5013393113266737,
                ""delivery_time"": ""5-5"",
                ""freight_commit_day"": ""50"",
                ""goods_prepare_time"": 3,
                ""issue_status"": ""NO_ISSUE"",
                ""logistics_amount"": {
                    ""amount"": ""349.00"",
                    ""currency_code"": ""RUB""
                },
                ""logistics_service_name"": ""AliExpress Курьер в города"",
                ""logistics_type"": ""AE_RU_MP_COURIER_PH3_CITY"",
                ""money_back3x"": false,
                ""order_id"": 5013393113266737,
                ""product_count"": 1,
                ""product_id"": 1005002891691638,
                ""product_img_url"": ""http:\/\/ae01.alicdn.com\/kf\/U2c924593d64a48b395f5269c56acc046I.jpg"",
                ""product_name"": ""Mud flaps splash guard Fender rear for Chery Tiggo 4, 2018-, 2 PCs (standard) rein 63 23 E13"",
                ""product_snap_url"": ""\/\/www.aliexpress.com\/snapshot\/null.html?orderId=5013393113266737"",
                ""product_unit"": ""piece"",
                ""product_unit_price"": {
                    ""amount"": ""820.00"",
                    ""currency_code"": ""RUB""
                },
                ""send_goods_operator"": ""SELLER_SEND_GOODS"",
                ""show_status"": ""PLACE_ORDER_SUCCESS"",
                ""sku_code"": ""REIN.63.23.E13"",
                ""son_order_status"": ""PLACE_ORDER_SUCCESS"",
                ""total_product_amount"": {
                    ""amount"": ""820.00"",
                    ""currency_code"": ""RUB""
                }
            }
        ]
    },
    ""seller_login_id"": ""ru1404462327cets"",
    ""seller_operator_login_id"": ""ru1404462327cets"",
    ""seller_signer_fullname"": ""Yapart Store""
},
                ]
            },
            ""page_size"":1,
            ""error_code"":""1"",
            ""current_page"":1,
            ""total_page"":1,
            ""success"":true,
            ""time_stamp"":""1""
        },
        ""request_id"": ""2y0vy8yvq6s2"" 
    }
   
}";
        }

        [Fact]
        public void TestDeserializeDTO_AliExpressOrder_Deserialize_AliExpressSolutionOrderGetResponseDTO_RequestId()
        {
            //arrange
            var json = _jsonOrderExpample;
            //act
            var aliExpressOrder = JsonConvert.DeserializeObject<AliExpressGetOrderRoot>(json);
            Assert.NotNull(aliExpressOrder);
            Assert.Equal("2y0vy8yvq6s2", aliExpressOrder.AliExpressSolutionOrderGetResponseDTO.RequestId);
        }

        [Fact]
        public void TestDeserializeDTO_AliExpressOrder_Deserialize_AliExpressSolutionOrderGetResponseResultDto_NotNull()
        {
            //arrange
            var json = _jsonOrderExpample;
            //act
            var aliExpressOrder = JsonConvert.DeserializeObject<AliExpressGetOrderRoot>(json);
            Assert.NotNull(aliExpressOrder);
            Assert.NotNull(aliExpressOrder.AliExpressSolutionOrderGetResponseDTO.AliExpressSolutionOrderGetResponseResultDto);
        }

        [Fact]
        public void TestDeserializeDTO_AliExpressOrder_Deserialize_AliExpressOrderListDTOs_NotNull()
        {
            //arrange
            var json = _jsonOrderExpample;
            //act
            var aliExpressOrder = JsonConvert.DeserializeObject<AliExpressGetOrderRoot>(json);
            Assert.NotNull(aliExpressOrder);
            Assert.NotNull(aliExpressOrder.AliExpressSolutionOrderGetResponseDTO.AliExpressSolutionOrderGetResponseResultDto.AliExpressOrderListDTOs);
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
        public void TestDeserializeDTO_AliExpressOrder_Deserialize_AliExpressGetOrderRoot_OrderBizType()
        {
            //arrange
            var json = _jsonOrderExpample;
            //act
            var aliExpressResponseResult = JsonConvert.DeserializeObject<AliExpressGetOrderRoot>(json)?.AliExpressSolutionOrderGetResponseDTO.AliExpressSolutionOrderGetResponseResultDto;
            //assert
            Assert.Equal("AE_COMMON", aliExpressResponseResult.AliExpressOrderListDTOs.First().BizType);
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
        public void TestDeserializeDTO_AliExpressOrder_Deserialize_AliExpressOrderListDTO_BizTypeString()
        {
            //arrange
            var json = _jsonOrderExpample;
            //act
            var jsonObject = JObject.Parse(json);
            var orderJson = jsonObject.SelectToken("aliexpress_solution_order_get_response.result.target_list.order_dto")?[0]?.ToString();
            var aliExpressOrder = JsonConvert.DeserializeObject<AliExpressOrderListDTO>(orderJson, new AliExpressOrderDetailConverter());
            Assert.NotNull(aliExpressOrder);
            Assert.Equal("AE_COMMON", aliExpressOrder.BizType);
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
            Assert.Equal(820.00, aliExpressOrder.AliExpressOrderProducts.First().ProductUnitPrice);
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
            Assert.Equal(820.00, aliExpressOrder.AliExpressOrderProducts.First().TotalProductAmount);
        }
    }
}
