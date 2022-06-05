using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using Xunit;
using YapartMarket.BL.Implementation;
using YapartMarket.Core.BL;
using YapartMarket.Core.Config;
using YapartMarket.Core.Data.Interfaces.Azure;
using YapartMarket.Core.DTO.AliExpress;
using YapartMarket.Core.DTO.AliExpress.OrderGetResponse;
using YapartMarket.Core.Extensions;
using YapartMarket.Core.Mapper;
using YapartMarket.Core.Models.Azure;
using Category = YapartMarket.Core.DTO.AliExpress.Category;

namespace YapartMarket.UnitTests.YapartMarker.Core
{
    public class MapperProfileTests
    {
        private IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IOptions<AliExpressOptions> _aliExpressOption;
        private readonly Mock<IAliExpressOrderService> _mockOrderService;
        private readonly Mock<ILogger<AliExpressOrderService>> _mockOrderServiceLogger;
        private readonly Mock<IAliExpressOrderRepository> _mockOrderRepository;
        private readonly Mock<IAliExpressOrderDetailRepository> _mockOrderDetailRepository;

        public MapperProfileTests()
        {
            _configuration = (IConfiguration)new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true).Build();
            _aliExpressOption = Options.Create(new AliExpressOptions()
            {
                AppKey = _configuration["AliExpress:AppKey"],
                AppSecret = _configuration["AliExpress:AppSecret"],
                AccessToken = _configuration["AliExpress:AccessToken"],
                HttpsEndPoint = _configuration["AliExpress:HttpsEndPoint"],
            });
            _mockOrderService = new Mock<IAliExpressOrderService>();
            _mockOrderServiceLogger = new Mock<ILogger<AliExpressOrderService>>();
            _mockOrderRepository = new Mock<IAliExpressOrderRepository>();
            _mockOrderDetailRepository = new Mock<IAliExpressOrderDetailRepository>();
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AliExpressOrderProfile());
                cfg.AddProfile(new AliExpressOrderLogisticProfile());
                cfg.AddProfile(new AliCategoryProfile());
            });
            _mapper = mockMapper.CreateMapper();
        }
        [Fact]
        public void AliCategoryProfile_Call_ReturnSuccess()
        {
            //Arrange
            var json =
                "{\r\n    \"aliexpress_category_redefining_getpostcategorybyid_response\": {\r\n        \"result\": {\r\n            \"aeop_post_category_list\": {\r\n                \"aeop_post_category_dto\": [\r\n                    {\r\n                        \"features\": \"{}\",\r\n                        \"id\": 200095145,\r\n                        \"isleaf\": true,\r\n                        \"level\": 4,\r\n                        \"names\": \"{\\\"de\\\":\\\"Block & Teile\\\",\\\"hi\\\":\\\"ब्लॉक और भागों\\\",\\\"ru\\\":\\\"Блоки и детали\\\",\\\"ko\\\":\\\"블록 및 부품\\\",\\\"pt\\\":\\\"Bloco e peças\\\",\\\"in\\\":\\\"Blok & Bagian\\\",\\\"en\\\":\\\"Block & Parts\\\",\\\"it\\\":\\\"Blocco & parti\\\",\\\"fr\\\":\\\"Blocs et pièces\\\",\\\"es\\\":\\\"Bloque y piezas\\\",\\\"iw\\\":\\\"בלוק & חלקים\\\",\\\"zh\\\":\\\"缸体及零件\\\",\\\"ar\\\":\\\"كتلة و أجزاء\\\",\\\"vi\\\":\\\"khối & Phụ Tùng\\\",\\\"th\\\":\\\"บล็อกและชิ้นส่วน\\\",\\\"ja\\\":\\\"ブロック&パーツ\\\",\\\"nl\\\":\\\"blok & Onderdelen\\\",\\\"tr\\\":\\\"Blok ve Parçaları\\\"}\"\r\n                    }\r\n                ]\r\n            },\r\n            \"success\": true\r\n        },\r\n        \"request_id\": \"15raj3e9db2u0\"\r\n    }\r\n}";
            var categoryInfo = JsonConvert.DeserializeObject<CategoryRoot>(json);
            var category = categoryInfo.Result.CategoryList.PostCategoryList.CategoryInfo.First();
            var languages = JsonConvert.DeserializeObject<LanguageNames>(category.MultilanguageName);
            //Act
            var result = _mapper.Map<Category, global::YapartMarket.Core.Models.Azure.Category>(category);
            //Assert
            Assert.Equal(result.CategoryId, category.Id);
            Assert.Equal(result.IsLeaf, category.IsLeaf);
            Assert.Equal(result.Level, category.Level);
            Assert.Equal(result.RuName, languages.Ru);
            Assert.Equal(result.EnName, languages.En);
        }

        [Fact]
        public void OrderProfile_GetData_ReturnSuccess()
        {
            //Arrange
            var json = @"{
    ""aliexpress_solution_order_get_response"": {
        ""result"": {
            ""current_page"": 1,
            ""error_code"": ""0"",
            ""error_message"": ""操作成功"",
            ""page_size"": 20,
            ""success"": true,
            ""target_list"": {
                ""order_dto"": [
                    {
                        ""biz_type"": ""AE_COMMON"",
                        ""buyer_login_id"": ""ru1293982566lkak"",
                        ""buyer_signer_fullname"": ""Aleksei Kostin"",
                        ""end_reason"": """",
                        ""frozen_status"": ""NO_FROZEN"",
                        ""fund_status"": ""PAY_SUCCESS"",
                        ""gmt_create"": ""2022-05-31 09:31:06"",
                        ""gmt_pay_time"": ""2022-05-31 09:31:12"",
                        ""gmt_update"": ""2022-06-01 04:57:43"",
                        ""has_request_loan"": false,
                        ""issue_status"": ""NO_ISSUE"",
                        ""logisitcs_escrow_fee_rate"": """",
                        ""order_id"": 5029511320737142,
                        ""order_status"": ""WAIT_BUYER_ACCEPT_GOODS"",
                        ""payment_type"": ""MIXEDCARD"",
                        ""phone"": false,
                        ""product_list"": {
                            ""order_product_dto"": [
                                {
                                    ""can_submit_issue"": false,
                                    ""child_id"": 5029511320747142,
                                    ""delivery_time"": ""3-3"",
                                    ""freight_commit_day"": ""50"",
                                    ""goods_prepare_time"": 1,
                                    ""issue_status"": ""NO_ISSUE"",
                                    ""logistics_amount"": {
                                        ""amount"": ""0.00"",
                                        ""currency_code"": ""RUB""
                                    },
                                    ""logistics_service_name"": ""AliExpress в пункт выдачи"",
                                    ""logistics_type"": ""AE_RU_MP_PUDO_PH3"",
                                    ""money_back3x"": false,
                                    ""order_id"": 5029511320747142,
                                    ""product_count"": 2,
                                    ""product_id"": 3256803852403583,
                                    ""product_img_url"": ""http:\/\/ae01.alicdn.com\/kf\/Ue22ba11bf4cc4a79bc7f233cb4c5241cd.jpg"",
                                    ""product_name"": ""Mud flaps splash guards front for EXEED VX 2021 - 2 PCs (optimum) in package, NLF. an0020.f1 NLF an0020 F1"",
                                    ""product_snap_url"": ""\/\/www.aliexpress.com\/snapshot\/null.html?orderId=5029511320747142"",
                                    ""product_unit"": ""piece"",
                                    ""product_unit_price"": {
                                        ""amount"": ""1230.00"",
                                        ""currency_code"": ""RUB""
                                    },
                                    ""send_goods_operator"": ""SELLER_SEND_GOODS"",
                                    ""show_status"": ""WAIT_BUYER_ACCEPT_GOODS"",
                                    ""sku_code"": ""NLF.AN0020.F1"",
                                    ""son_order_status"": ""WAIT_BUYER_ACCEPT_GOODS"",
                                    ""total_product_amount"": {
                                        ""amount"": ""1230.00"",
                                        ""currency_code"": ""RUB""
                                    }
                                }
                            ]
                        },
                        ""seller_login_id"": ""ru1404462327cets"",
                        ""seller_operator_login_id"": ""ru1404462327cets"",
                        ""seller_signer_fullname"": ""Rogonskiy Store"",
                        ""timeout_left_time"": 1348494003
                    },
                    {
                        ""biz_type"": ""AE_COMMON"",
                        ""buyer_login_id"": ""ru1106779685hred"",
                        ""buyer_signer_fullname"": ""Dmitriy Shafikov"",
                        ""end_reason"": """",
                        ""frozen_status"": ""NO_FROZEN"",
                        ""fund_status"": ""PAY_SUCCESS"",
                        ""gmt_create"": ""2022-05-31 08:01:21"",
                        ""gmt_pay_time"": ""2022-05-31 08:01:26"",
                        ""gmt_update"": ""2022-06-01 04:57:43"",
                        ""has_request_loan"": false,
                        ""issue_status"": ""NO_ISSUE"",
                        ""logisitcs_escrow_fee_rate"": """",
                        ""order_id"": 5029401129572430,
                        ""order_status"": ""WAIT_BUYER_ACCEPT_GOODS"",
                        ""payment_type"": ""MIXEDCARD"",
                        ""phone"": false,
                        ""product_list"": {
                            ""order_product_dto"": [
                                {
                                    ""can_submit_issue"": false,
                                    ""child_id"": 5029401129582430,
                                    ""delivery_time"": ""19-19"",
                                    ""freight_commit_day"": ""50"",
                                    ""goods_prepare_time"": 2,
                                    ""issue_status"": ""NO_ISSUE"",
                                    ""logistics_amount"": {
                                        ""amount"": ""349.00"",
                                        ""currency_code"": ""RUB""
                                    },
                                    ""logistics_service_name"": ""AliExpress Курьер в города"",
                                    ""logistics_type"": ""AE_RU_MP_COURIER_PH3_CITY"",
                                    ""money_back3x"": false,
                                    ""order_id"": 5029401129582430,
                                    ""product_count"": 1,
                                    ""product_id"": 1005002891824070,
                                    ""product_img_url"": ""http:\/\/ae01.alicdn.com\/kf\/A4f07259e00284a2da2a03d3d1bc23bb5m.jpg"",
                                    ""product_name"": ""Брызговики задние SSANG YONG Kyron, 2005-, 2 шт. (standart), NLFD.61.09.E13"",
                                    ""product_snap_url"": ""\/\/www.aliexpress.com\/snapshot\/null.html?orderId=5029401129582430"",
                                    ""product_unit"": ""piece"",
                                    ""product_unit_price"": {
                                        ""amount"": ""963.00"",
                                        ""currency_code"": ""RUB""
                                    },
                                    ""send_goods_operator"": ""SELLER_SEND_GOODS"",
                                    ""show_status"": ""WAIT_BUYER_ACCEPT_GOODS"",
                                    ""sku_code"": ""NLFD.61.09.E13"",
                                    ""son_order_status"": ""WAIT_BUYER_ACCEPT_GOODS"",
                                    ""total_product_amount"": {
                                        ""amount"": ""963.00"",
                                        ""currency_code"": ""RUB""
                                    }
                                }
                            ]
                        },
                        ""seller_login_id"": ""ru1404462327cets"",
                        ""seller_operator_login_id"": ""ru1404462327cets"",
                        ""seller_signer_fullname"": ""Rogonskiy Store"",
                        ""timeout_left_time"": 1348493412
                    },
                    {
                        ""biz_type"": ""AE_COMMON"",
                        ""buyer_login_id"": ""ru2272933691qpzae"",
                        ""buyer_signer_fullname"": ""ARTEM KOLIN"",
                        ""end_reason"": """",
                        ""frozen_status"": ""NO_FROZEN"",
                        ""fund_status"": ""PAY_SUCCESS"",
                        ""gmt_create"": ""2022-05-31 07:59:39"",
                        ""gmt_pay_time"": ""2022-05-31 07:59:46"",
                        ""gmt_update"": ""2022-06-01 04:57:45"",
                        ""has_request_loan"": false,
                        ""issue_status"": ""NO_ISSUE"",
                        ""logisitcs_escrow_fee_rate"": """",
                        ""order_id"": 5029547897177691,
                        ""order_status"": ""WAIT_BUYER_ACCEPT_GOODS"",
                        ""payment_type"": ""MIXEDCARD"",
                        ""phone"": false,
                        ""product_list"": {
                            ""order_product_dto"": [
                                {
                                    ""can_submit_issue"": false,
                                    ""child_id"": 5029547897187691,
                                    ""delivery_time"": ""5-5"",
                                    ""freight_commit_day"": ""100"",
                                    ""goods_prepare_time"": 2,
                                    ""issue_status"": ""NO_ISSUE"",
                                    ""logistics_amount"": {
                                        ""amount"": ""0.00"",
                                        ""currency_code"": ""RUB""
                                    },
                                    ""logistics_service_name"": ""AliExpress: в отделение Почты России («Посылка Онлайн»)"",
                                    ""logistics_type"": ""AE_RU_MP_RUPOST_PH3_FR"",
                                    ""money_back3x"": false,
                                    ""order_id"": 5029547897187691,
                                    ""product_count"": 1,
                                    ""product_id"": 1005002891615028,
                                    ""product_img_url"": ""http:\/\/ae01.alicdn.com\/kf\/A8f669362a5e44963ae5d11939c2c5d3cL.jpg"",
                                    ""product_name"": ""Lockers Ford Fusion 09\/2002-2012 1 set (rear), 1750930 1750930"",
                                    ""product_snap_url"": ""\/\/www.aliexpress.com\/snapshot\/null.html?orderId=5029547897187691"",
                                    ""product_unit"": ""piece"",
                                    ""product_unit_price"": {
                                        ""amount"": ""3375.00"",
                                        ""currency_code"": ""RUB""
                                    },
                                    ""send_goods_operator"": ""SELLER_SEND_GOODS"",
                                    ""show_status"": ""WAIT_BUYER_ACCEPT_GOODS"",
                                    ""sku_code"": ""1750930"",
                                    ""son_order_status"": ""WAIT_BUYER_ACCEPT_GOODS"",
                                    ""total_product_amount"": {
                                        ""amount"": ""3375.00"",
                                        ""currency_code"": ""RUB""
                                    }
                                }
                            ]
                        },
                        ""seller_login_id"": ""ru1404462327cets"",
                        ""seller_operator_login_id"": ""ru1404462327cets"",
                        ""seller_signer_fullname"": ""Rogonskiy Store"",
                        ""timeout_left_time"": 1348495801
                    },
                    {
                        ""biz_type"": ""AE_COMMON"",
                        ""buyer_login_id"": ""ru396055188ovnae"",
                        ""buyer_signer_fullname"": ""Evgeny shopper"",
                        ""end_reason"": """",
                        ""frozen_status"": ""NO_FROZEN"",
                        ""fund_status"": ""PAY_SUCCESS"",
                        ""gmt_create"": ""2022-05-31 02:53:27"",
                        ""gmt_pay_time"": ""2022-05-31 02:53:34"",
                        ""gmt_update"": ""2022-06-01 04:57:44"",
                        ""has_request_loan"": false,
                        ""issue_status"": ""NO_ISSUE"",
                        ""logisitcs_escrow_fee_rate"": """",
                        ""order_id"": 5029396889252188,
                        ""order_status"": ""WAIT_BUYER_ACCEPT_GOODS"",
                        ""payment_type"": ""MIXEDCARD"",
                        ""phone"": false,
                        ""product_list"": {
                            ""order_product_dto"": [
                                {
                                    ""can_submit_issue"": false,
                                    ""child_id"": 5029396889262188,
                                    ""delivery_time"": ""19-19"",
                                    ""freight_commit_day"": ""50"",
                                    ""goods_prepare_time"": 2,
                                    ""issue_status"": ""NO_ISSUE"",
                                    ""logistics_amount"": {
                                        ""amount"": ""0.00"",
                                        ""currency_code"": ""RUB""
                                    },
                                    ""logistics_service_name"": ""AliExpress в пункт выдачи"",
                                    ""logistics_type"": ""AE_RU_MP_PUDO_PH3"",
                                    ""money_back3x"": false,
                                    ""order_id"": 5029396889262188,
                                    ""product_count"": 1,
                                    ""product_id"": 1005002891687203,
                                    ""product_img_url"": ""http:\/\/ae01.alicdn.com\/kf\/U3e6ab61aec704ec6a29095b8b4627fd7i.jpg"",
                                    ""product_name"": ""Mat trunk Skoda Octavia Tour 1996-, хетчбек (polyurethane), nlc.45.09.b11 NLC 45 09 B11"",
                                    ""product_snap_url"": ""\/\/www.aliexpress.com\/snapshot\/null.html?orderId=5029396889262188"",
                                    ""product_unit"": ""piece"",
                                    ""product_unit_price"": {
                                        ""amount"": ""2070.00"",
                                        ""currency_code"": ""RUB""
                                    },
                                    ""send_goods_operator"": ""SELLER_SEND_GOODS"",
                                    ""show_status"": ""WAIT_BUYER_ACCEPT_GOODS"",
                                    ""sku_code"": ""NLC.45.09.B11"",
                                    ""son_order_status"": ""WAIT_BUYER_ACCEPT_GOODS"",
                                    ""total_product_amount"": {
                                        ""amount"": ""2070.00"",
                                        ""currency_code"": ""RUB""
                                    }
                                }
                            ]
                        },
                        ""seller_login_id"": ""ru1404462327cets"",
                        ""seller_operator_login_id"": ""ru1404462327cets"",
                        ""seller_signer_fullname"": ""Rogonskiy Store"",
                        ""timeout_left_time"": 1348494938
                    },
                    {
                        ""biz_type"": ""AE_COMMON"",
                        ""buyer_login_id"": ""ru1259587027dyzq"",
                        ""buyer_signer_fullname"": ""Andrei Linikov"",
                        ""end_reason"": """",
                        ""frozen_status"": ""NO_FROZEN"",
                        ""fund_status"": ""PAY_SUCCESS"",
                        ""gmt_create"": ""2022-05-31 00:35:07"",
                        ""gmt_pay_time"": ""2022-05-31 00:35:13"",
                        ""gmt_update"": ""2022-06-01 04:57:42"",
                        ""has_request_loan"": false,
                        ""issue_status"": ""NO_ISSUE"",
                        ""logisitcs_escrow_fee_rate"": """",
                        ""order_id"": 5029503885210251,
                        ""order_status"": ""WAIT_BUYER_ACCEPT_GOODS"",
                        ""payment_type"": ""MIXEDCARD"",
                        ""phone"": false,
                        ""product_list"": {
                            ""order_product_dto"": [
                                {
                                    ""can_submit_issue"": false,
                                    ""child_id"": 5029503885220251,
                                    ""delivery_time"": ""9-9"",
                                    ""freight_commit_day"": ""100"",
                                    ""goods_prepare_time"": 2,
                                    ""issue_status"": ""NO_ISSUE"",
                                    ""logistics_amount"": {
                                        ""amount"": ""0.00"",
                                        ""currency_code"": ""RUB""
                                    },
                                    ""logistics_service_name"": ""AliExpress: в отделение Почты России («Посылка Онлайн»)"",
                                    ""logistics_type"": ""AE_RU_MP_RUPOST_PH3_FR"",
                                    ""money_back3x"": false,
                                    ""order_id"": 5029503885220251,
                                    ""product_count"": 1,
                                    ""product_id"": 1005002891641018,
                                    ""product_img_url"": ""http:\/\/ae01.alicdn.com\/kf\/U6fc4dcf2bf98426486a9076f384b4c0eC.jpg"",
                                    ""product_name"": ""Mat trunk Lifan Solano, 2010-2016 sedan (polyurethane), carlif00004 carlif00004"",
                                    ""product_snap_url"": ""\/\/www.aliexpress.com\/snapshot\/null.html?orderId=5029503885220251"",
                                    ""product_unit"": ""piece"",
                                    ""product_unit_price"": {
                                        ""amount"": ""1980.00"",
                                        ""currency_code"": ""RUB""
                                    },
                                    ""send_goods_operator"": ""SELLER_SEND_GOODS"",
                                    ""show_status"": ""WAIT_BUYER_ACCEPT_GOODS"",
                                    ""sku_code"": ""CARLIF00004"",
                                    ""son_order_status"": ""WAIT_BUYER_ACCEPT_GOODS"",
                                    ""total_product_amount"": {
                                        ""amount"": ""1980.00"",
                                        ""currency_code"": ""RUB""
                                    }
                                }
                            ]
                        },
                        ""seller_login_id"": ""ru1404462327cets"",
                        ""seller_operator_login_id"": ""ru1404462327cets"",
                        ""seller_signer_fullname"": ""Yapart Store"",
                        ""timeout_left_time"": 1348492151
                    }
                ]
            },
            ""total_count"": 5,
            ""total_page"": 1
        },
        ""request_id"": ""15rxrzq6cno4d""
    }
}";
            var deserializeJson = JsonConvert.DeserializeObject<OrderRootDto>(json);
            var orders = deserializeJson.aliexpress_solution_order_get_response.result.target_list.Orders;
            //Act
            var aliExpressOrders = _mapper.Map<List<OrderDto>, List<AliExpressOrder>>(orders);
            //Assert
            Assert.True(aliExpressOrders.IsAny());
            Assert.True(aliExpressOrders.IsAny());
        }
    }
}
