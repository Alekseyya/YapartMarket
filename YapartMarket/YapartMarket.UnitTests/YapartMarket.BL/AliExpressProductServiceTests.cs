using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using YapartMarket.BL.Implementation;
using YapartMarket.Core;
using YapartMarket.Core.Config;
using YapartMarket.Core.Data.Interfaces.Azure;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.UnitTests.YapartMarket.BL
{
    public class AliExpressProductServiceTests
    {
        private readonly IOptions<AliExpressOptions> _aliExpressOption;
        private readonly IConfiguration _configuration;
        private Mock<ILogger<AliExpressProductService>> _mockLogger;
        private Mock<IAzureProductRepository> _mockAzureProductService;
        private Mock<IAzureAliExpressProductRepository> _mockAzureAliExpressRepository;
        private Mock<IProductPropertyRepository> _mockProductPropertyRepository;
        private Mock<IServiceScopeFactory> _mockServiceScopeFactory;
        private Mock<IHttpClientFactory> _mockHttpClientFactory;

        public AliExpressProductServiceTests()
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
                GetOrderList = _configuration["AliExpress:GetOrderList"],
                Url = _configuration["AliExpress:Url"],
            });

            _mockLogger = new Mock<ILogger<AliExpressProductService>>();
            _mockAzureProductService = new Mock<IAzureProductRepository>();
            _mockAzureAliExpressRepository = new Mock<IAzureAliExpressProductRepository>();
            _mockProductPropertyRepository = new Mock<IProductPropertyRepository>();
            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
                .Setup(x => x.GetService(typeof(Deserializer<IReadOnlyList<AliExpressOrder>>)))
                .Returns(new OrderDeserializer());

            var serviceScope = new Mock<IServiceScope>();
            serviceScope.Setup(x => x.ServiceProvider).Returns(serviceProvider.Object);

            var serviceScopeFactory = new Mock<IServiceScopeFactory>();
            serviceScopeFactory
                .Setup(x => x.CreateScope())
                .Returns(serviceScope.Object);

            serviceProvider
                .Setup(x => x.GetService(typeof(IServiceScopeFactory)))
                .Returns(serviceScopeFactory.Object);

            _mockServiceScopeFactory = serviceScopeFactory;
            var mockFactory = new Mock<IHttpClientFactory>();

            var client = new HttpClient();
            client.BaseAddress = new Uri(_aliExpressOption.Value.Url);
            client.DefaultRequestHeaders.Add("x-auth-token", _aliExpressOption.Value.AuthToken);
            mockFactory.Setup(_ => _.CreateClient("aliExpress")).Returns(client);

            _mockHttpClientFactory = mockFactory;
        }


        [Fact]
        public async Task TestAliExpressProductService_UpdateProducts_NotNUll()
        {
            //Act
            var aliExpressProductService = new AliExpressProductService(_mockAzureAliExpressRepository.Object,
                _mockAzureProductService.Object, _mockProductPropertyRepository.Object, _aliExpressOption,
                _configuration, _mockLogger.Object, _mockServiceScopeFactory.Object, _mockHttpClientFactory.Object);
            //Arrange
            await aliExpressProductService.UpdateProductFromSql();

        }
//        [Fact]
//        public void TestAliExpressProductService_ProcessAliExpressProductId_UpdateOneRow()
//        {
//            //act
//            var aliExpressProductService = new AliExpressProductService(_mockAzureAliExpressRepository.Object, _mockAzureProductService.Object, _mockProductPropertyRepository.Object, _aliExpressOption, _configuration, _mockLogger.Object);
//            var aliExpressProduct = new List<AliExpressProductDTO>
//            {
//                new()
//                {
//                    SkuCode = "REINWV1181",
//                    ProductId = 1005003033814656
//                }
//            };
//            //arrange
//            aliExpressProductService.ProcessUpdateDatabaseAliExpressProductId();
//            Product productsInDb = null;
//            using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
//            {
//                connection.Open();
//                productsInDb = connection.QueryFirstOrDefault<Product>("select * from products where sku = @sku", new { sku = "REINWV1181" });
//            }
//            //assert
//            Assert.NotNull(productsInDb);
//            Assert.NotNull(productsInDb.AliExpressProductId);
//            Assert.Equal(1005003033814656, productsInDb.AliExpressProductId.Value);
//        }

//        [Fact]
//        public void TestAliExpressService_GetProductsFromJson()
//        {
//            //act

//            #region json
//            var json = @"{
//    ""aliexpress_solution_product_list_get_response"": {
//        ""result"": {
//            ""aeop_a_e_product_display_d_t_o_list"": {
//                ""item_display_dto"": [
//                    {
//                        ""currency_code"": ""RUB"",
//                        ""freight_template_id"": -1719201133,
//                        ""gmt_create"": ""2021-07-22 20:17:24"",
//                        ""gmt_modified"": ""2021-08-14 02:25:00"",
//                        ""image_u_r_ls"": ""https://ae04.alicdn.com/kf/H514b598e8f1d48e09ab4c2729d84b9965.jpg"",
//                        ""owner_member_id"": ""ru1404462327cets"",
//                        ""owner_member_seq"": 1021372981,
//                        ""product_id"": 1005003033814656,
//                        ""product_max_price"": ""2113.0"",
//                        ""product_min_price"": ""2113.0"",
//                        ""src"": ""isv"",
//                        ""subject"": ""Deflector window (patch tape m), 4 PCs Hyundai palisade I, 2018-, crossover""
//                    },
//                    {
//                        ""currency_code"": ""RUB"",
//                        ""freight_template_id"": -1719201133,
//                        ""gmt_create"": ""2021-07-22 20:17:21"",
//                        ""gmt_modified"": ""2021-08-14 02:24:58"",
//                        ""image_u_r_ls"": ""https://ae04.alicdn.com/kf/H3c4df270152e4778b5322a50b6ef1ef5c.jpg"",
//                        ""owner_member_id"": ""ru1404462327cets"",
//                        ""owner_member_seq"": 1021372981,
//                        ""product_id"": 1005003033802755,
//                        ""product_max_price"": ""1869.0"",
//                        ""product_min_price"": ""1869.0"",
//                        ""src"": ""isv"",
//                        ""subject"": ""Deflector window (patch tape m), 4 PCs Hyundai Elantra VII (CN7), 2020-, sedan""
//                    },
//                    {
//                        ""currency_code"": ""RUB"",
//                        ""freight_template_id"": -1719201133,
//                        ""gmt_create"": ""2021-07-22 20:17:21"",
//                        ""gmt_modified"": ""2021-08-14 02:25:08"",
//                        ""image_u_r_ls"": ""https://ae04.alicdn.com/kf/Sf940545e239545d584e637d30fd3bd5d3.jpg"",
//                        ""owner_member_id"": ""ru1404462327cets"",
//                        ""owner_member_seq"": 1021372981,
//                        ""product_id"": 1005003033854420,
//                        ""product_max_price"": ""2194.0"",
//                        ""product_min_price"": ""2194.0"",
//                        ""src"": ""isv"",
//                        ""subject"": ""Deflector window (insertable under the elastic band), 4 PCs Hyundai Elantra VII (CN7), 2020-, sedan""
//                    },
//                    {
//                        ""currency_code"": ""RUB"",
//                        ""freight_template_id"": -1719201133,
//                        ""gmt_create"": ""2021-07-22 20:17:20"",
//                        ""gmt_modified"": ""2021-08-14 02:24:56"",
//                        ""image_u_r_ls"": ""https://ae04.alicdn.com/kf/Had5efa7b8c084db2a7b5b144c30d44dd3.jpg"",
//                        ""owner_member_id"": ""ru1404462327cets"",
//                        ""owner_member_seq"": 1021372981,
//                        ""product_id"": 1005003033795871,
//                        ""product_max_price"": ""2290.0"",
//                        ""product_min_price"": ""2290.0"",
//                        ""src"": ""isv"",
//                        ""subject"": ""Deflector window (insertable under the elastic band), 4 PCs Skoda Octavia IV (A8), 2019-, лифтбек""
//                    },
//                    {
//                        ""currency_code"": ""RUB"",
//                        ""freight_template_id"": -1719201133,
//                        ""gmt_create"": ""2021-07-22 20:17:20"",
//                        ""gmt_modified"": ""2021-08-14 02:25:18"",
//                        ""image_u_r_ls"": ""https://ae04.alicdn.com/kf/Sf940545e239545d584e637d30fd3bd5d3.jpg"",
//                        ""owner_member_id"": ""ru1404462327cets"",
//                        ""owner_member_seq"": 1021372981,
//                        ""product_id"": 1005003033890188,
//                        ""product_max_price"": ""3875.0"",
//                        ""product_min_price"": ""3875.0"",
//                        ""src"": ""isv"",
//                        ""subject"": ""Deflectors windows SIM Kia K5, 2020 -, dark""
//                    },
//                    {
//                        ""currency_code"": ""RUB"",
//                        ""freight_template_id"": -1719201133,
//                        ""gmt_create"": ""2021-07-22 20:17:20"",
//                        ""gmt_modified"": ""2021-08-14 02:25:15"",
//                        ""image_u_r_ls"": ""https://ae04.alicdn.com/kf/H31a419363b2c42d0980eecafa239ed1dW.jpg"",
//                        ""owner_member_id"": ""ru1404462327cets"",
//                        ""owner_member_seq"": 1021372981,
//                        ""product_id"": 1005003033872274,
//                        ""product_max_price"": ""3584.0"",
//                        ""product_min_price"": ""3584.0"",
//                        ""src"": ""isv"",
//                        ""subject"": ""Дефлектор капота SKODA Rapid, 2020-, темный""
//                    },
//                    {
//                        ""currency_code"": ""RUB"",
//                        ""freight_template_id"": -1719201133,
//                        ""gmt_create"": ""2021-07-22 20:17:08"",
//                        ""gmt_modified"": ""2021-08-14 02:25:04"",
//                        ""image_u_r_ls"": ""https://ae04.alicdn.com/kf/Sf940545e239545d584e637d30fd3bd5d3.jpg"",
//                        ""owner_member_id"": ""ru1404462327cets"",
//                        ""owner_member_seq"": 1021372981,
//                        ""product_id"": 1005003033821628,
//                        ""product_max_price"": ""3875.0"",
//                        ""product_min_price"": ""3875.0"",
//                        ""src"": ""isv"",
//                        ""subject"": ""Deflectors windows SIM Kia Sorento, 2020-, dark""
//                    },
//                    {
//                        ""currency_code"": ""RUB"",
//                        ""freight_template_id"": -1719201133,
//                        ""gmt_create"": ""2021-07-22 20:17:01"",
//                        ""gmt_modified"": ""2021-08-14 02:25:20"",
//                        ""image_u_r_ls"": ""https://ae04.alicdn.com/kf/S6ccb15ce618345409cea8b04ab3839d1k.jpg"",
//                        ""owner_member_id"": ""ru1404462327cets"",
//                        ""owner_member_seq"": 1021372981,
//                        ""product_id"": 1005003033900098,
//                        ""product_max_price"": ""4018.0"",
//                        ""product_min_price"": ""4018.0"",
//                        ""src"": ""isv"",
//                        ""subject"": ""Window deflectors vinguru Toyota Camry 2019, false, tape, K-M 4 PCs injection polycarbonate""
//                    },
//                    {
//                        ""currency_code"": ""RUB"",
//                        ""freight_template_id"": -1719201133,
//                        ""gmt_create"": ""2021-07-22 20:16:59"",
//                        ""gmt_modified"": ""2021-08-14 02:25:17"",
//                        ""image_u_r_ls"": ""https://ae04.alicdn.com/kf/S6ccb15ce618345409cea8b04ab3839d1k.jpg"",
//                        ""owner_member_id"": ""ru1404462327cets"",
//                        ""owner_member_seq"": 1021372981,
//                        ""product_id"": 1005003033882210,
//                        ""product_max_price"": ""4018.0"",
//                        ""product_min_price"": ""4018.0"",
//                        ""src"": ""isv"",
//                        ""subject"": ""Дефлекторы окон Vinguru HUYNDAI Tucson 2016-2021 ,накладные,скотч ,к-т 4 шт., литьевой поликарбонат""
//                    },
//                    {
//                        ""currency_code"": ""RUB"",
//                        ""freight_template_id"": -1719201133,
//                        ""gmt_create"": ""2021-07-22 20:16:58"",
//                        ""gmt_modified"": ""2021-08-14 02:25:10"",
//                        ""image_u_r_ls"": ""https://ae04.alicdn.com/kf/Sf940545e239545d584e637d30fd3bd5d3.jpg"",
//                        ""owner_member_id"": ""ru1404462327cets"",
//                        ""owner_member_seq"": 1021372981,
//                        ""product_id"": 1005003033858378,
//                        ""product_max_price"": ""4557.0"",
//                        ""product_min_price"": ""4557.0"",
//                        ""src"": ""isv"",
//                        ""subject"": ""Window deflectors for Hyundai Santa Fe 2018 -, 6 PCs (4 PCs The door + deflectors rear глухих Windows)""
//                    },
//                    {
//                        ""currency_code"": ""RUB"",
//                        ""freight_template_id"": -1719201133,
//                        ""gmt_create"": ""2021-07-22 20:16:58"",
//                        ""gmt_modified"": ""2021-08-14 02:25:23"",
//                        ""image_u_r_ls"": ""https://ae04.alicdn.com/kf/H5a645dd9a8c34a8d983419dda6427a9ex.jpg"",
//                        ""owner_member_id"": ""ru1404462327cets"",
//                        ""owner_member_seq"": 1021372981,
//                        ""product_id"": 1005003033911007,
//                        ""product_max_price"": ""2194.0"",
//                        ""product_min_price"": ""2194.0"",
//                        ""src"": ""isv"",
//                        ""subject"": ""Deflector window (insertable under the elastic band), 4 PCs Kia Sorento IV, 2020-, SUV""
//                    },
//                    {
//                        ""currency_code"": ""RUB"",
//                        ""freight_template_id"": -1719201133,
//                        ""gmt_create"": ""2021-07-22 20:16:57"",
//                        ""gmt_modified"": ""2021-08-14 02:25:21"",
//                        ""image_u_r_ls"": ""https://ae04.alicdn.com/kf/Hcbfa069b4b244c05bd293713929a7ea8G.jpg"",
//                        ""owner_member_id"": ""ru1404462327cets"",
//                        ""owner_member_seq"": 1021372981,
//                        ""product_id"": 1005003033904051,
//                        ""product_max_price"": ""4421.0"",
//                        ""product_min_price"": ""4421.0"",
//                        ""src"": ""isv"",
//                        ""subject"": ""Window deflectors chromex Chrome молдингом Skoda Octavia IV (A8), 2019-, patch""
//                    },
//                    {
//                        ""currency_code"": ""RUB"",
//                        ""freight_template_id"": -1719201133,
//                        ""gmt_create"": ""2021-07-22 20:16:57"",
//                        ""gmt_modified"": ""2021-08-14 02:25:02"",
//                        ""image_u_r_ls"": ""https://ae04.alicdn.com/kf/H9da7a83968474a8ba2e27305c5a6fff4u.jpg"",
//                        ""owner_member_id"": ""ru1404462327cets"",
//                        ""owner_member_seq"": 1021372981,
//                        ""product_id"": 1005003033818662,
//                        ""product_max_price"": ""4421.0"",
//                        ""product_min_price"": ""4421.0"",
//                        ""src"": ""isv"",
//                        ""subject"": ""Window deflectors chromex Chrome молдингом Kia Sorento IV, 2020-, 4 PCs, patch""
//                    },
//                    {
//                        ""currency_code"": ""RUB"",
//                        ""freight_template_id"": -1719201133,
//                        ""gmt_create"": ""2021-07-22 20:16:57"",
//                        ""gmt_modified"": ""2021-08-14 02:25:02"",
//                        ""image_u_r_ls"": ""https://ae04.alicdn.com/kf/S6ccb15ce618345409cea8b04ab3839d1k.jpg"",
//                        ""owner_member_id"": ""ru1404462327cets"",
//                        ""owner_member_seq"": 1021372981,
//                        ""product_id"": 1005003033817647,
//                        ""product_max_price"": ""4018.0"",
//                        ""product_min_price"": ""4018.0"",
//                        ""src"": ""isv"",
//                        ""subject"": ""Window deflectors vinguru Honda Pilot 2016-, false, tape, K-M 4 PCs injection polycarbonate""
//                    },
//                    {
//                        ""currency_code"": ""RUB"",
//                        ""freight_template_id"": -1719201133,
//                        ""gmt_create"": ""2021-07-22 20:16:57"",
//                        ""gmt_modified"": ""2021-08-14 02:25:01"",
//                        ""image_u_r_ls"": ""https://ae04.alicdn.com/kf/S6ccb15ce618345409cea8b04ab3839d1k.jpg"",
//                        ""owner_member_id"": ""ru1404462327cets"",
//                        ""owner_member_seq"": 1021372981,
//                        ""product_id"": 1005003033816650,
//                        ""product_max_price"": ""4018.0"",
//                        ""product_min_price"": ""4018.0"",
//                        ""src"": ""isv"",
//                        ""subject"": ""Window deflectors vinguru Jeep Cherokee 2014-, false, tape, K-M 4 PCs injection polycarbonate""
//                    },
//                    {
//                        ""currency_code"": ""RUB"",
//                        ""freight_template_id"": -1719201133,
//                        ""gmt_create"": ""2021-07-22 20:16:57"",
//                        ""gmt_modified"": ""2021-08-14 02:25:12"",
//                        ""image_u_r_ls"": ""https://ae04.alicdn.com/kf/S6ccb15ce618345409cea8b04ab3839d1k.jpg"",
//                        ""owner_member_id"": ""ru1404462327cets"",
//                        ""owner_member_seq"": 1021372981,
//                        ""product_id"": 1005003033862306,
//                        ""product_max_price"": ""4018.0"",
//                        ""product_min_price"": ""4018.0"",
//                        ""src"": ""isv"",
//                        ""subject"": ""Window deflectors vinguru Subaru Forester 2019-, false, tape, K-M 4 PCs injection polycarbonate""
//                    },
//                    {
//                        ""currency_code"": ""RUB"",
//                        ""freight_template_id"": -1719201133,
//                        ""gmt_create"": ""2021-07-22 20:16:57"",
//                        ""gmt_modified"": ""2021-08-14 02:26:07"",
//                        ""image_u_r_ls"": ""https://ae04.alicdn.com/kf/Sf940545e239545d584e637d30fd3bd5d3.jpg"",
//                        ""owner_member_id"": ""ru1404462327cets"",
//                        ""owner_member_seq"": 1021372981,
//                        ""product_id"": 1005003033861318,
//                        ""product_max_price"": ""1980.0"",
//                        ""product_min_price"": ""1980.0"",
//                        ""src"": ""isv"",
//                        ""subject"": ""Deflector window (patch tape m), 4 PCs Haval jolion, 2020-""
//                    },
//                    {
//                        ""currency_code"": ""RUB"",
//                        ""freight_template_id"": -1719201133,
//                        ""gmt_create"": ""2021-07-22 20:16:57"",
//                        ""gmt_modified"": ""2021-08-14 02:25:22"",
//                        ""image_u_r_ls"": ""https://ae04.alicdn.com/kf/S6ccb15ce618345409cea8b04ab3839d1k.jpg"",
//                        ""owner_member_id"": ""ru1404462327cets"",
//                        ""owner_member_seq"": 1021372981,
//                        ""product_id"": 1005003033908038,
//                        ""product_max_price"": ""4018.0"",
//                        ""product_min_price"": ""4018.0"",
//                        ""src"": ""isv"",
//                        ""subject"": ""Window deflectors vinguru Subaru Outback 2015-2019, false, tape, K-M 4 PCs injection polycarbonate""
//                    },
//                    {
//                        ""currency_code"": ""RUB"",
//                        ""freight_template_id"": -1719201133,
//                        ""gmt_create"": ""2021-07-22 20:16:56"",
//                        ""gmt_modified"": ""2021-08-14 02:25:10"",
//                        ""image_u_r_ls"": ""https://ae04.alicdn.com/kf/S6ccb15ce618345409cea8b04ab3839d1k.jpg"",
//                        ""owner_member_id"": ""ru1404462327cets"",
//                        ""owner_member_seq"": 1021372981,
//                        ""product_id"": 1005003033857376,
//                        ""product_max_price"": ""4018.0"",
//                        ""product_min_price"": ""4018.0"",
//                        ""src"": ""isv"",
//                        ""subject"": ""Window deflectors vinguru Jeep Grand Cherokee 2011-2021, false, tape, K-M 4 PCs injection polycarbonate""
//                    },
//                    {
//                        ""currency_code"": ""RUB"",
//                        ""freight_template_id"": -1719201133,
//                        ""gmt_create"": ""2021-07-22 20:16:56"",
//                        ""gmt_modified"": ""2021-08-14 02:25:19"",
//                        ""image_u_r_ls"": ""https://ae04.alicdn.com/kf/He71fbe6fb1274d08bd907bfa3eb2b0ed2.jpg"",
//                        ""owner_member_id"": ""ru1404462327cets"",
//                        ""owner_member_seq"": 1021372981,
//                        ""product_id"": 1005003033899082,
//                        ""product_max_price"": ""4421.0"",
//                        ""product_min_price"": ""4421.0"",
//                        ""src"": ""isv"",
//                        ""subject"": ""Window deflectors chromex with Chrome. Молдингом Hyundai palisade I, 2018-, 4 PCs, crossover, patch""
//                    }
//                ]
//            },
//            ""current_page"": 1,
//            ""product_count"": 7657,
//            ""success"": true,
//            ""total_page"": 383
//        },
//        ""request_id"": ""ezt239rfn7m8""
//    }
//}";
//            #endregion
//            //arrange
//            var jsonObject = JObject.Parse(json);
//            var productsJson = jsonObject.SelectToken("aliexpress_solution_product_list_get_response.result.aeop_a_e_product_display_d_t_o_list.item_display_dto").ToObject<IEnumerable<AliExpressProductDTO>>();
//            //assert
//            Assert.Equal(1005003033814656, productsJson.First().ProductId);
//        }

//        [Fact]
//        public void TestAliExpressService_SetInventoryFromDatabase_SetInventoryValue()
//        {
//            //act
//            var aliExpressProductService = new AliExpressProductService(_mockAzureAliExpressRepository.Object, _mockAzureProductService.Object, _mockProductPropertyRepository.Object, _aliExpressOption, _configuration, _mockLogger.Object);
//            var aliExpressProductTest = new List<AliExpressProductDTO>()
//            {
//                new()
//                {
//                    SkuCode = "REINWV1181"
//                }
//            };
//            //arrange
//            var aliExpressProducts = aliExpressProductService.SetInventoryFromDatabase(aliExpressProductTest);
//            //assert
//            Assert.NotNull(aliExpressProducts);
//            Assert.True(aliExpressProducts.Count > 0);
//            Assert.Equal(1, aliExpressProducts.First().Inventory);
//        }

//        [Fact]
//        public void GetProductFromAli_SetInventoryValue()
//        {
//            //act
//            var aliExpressProductService = new AliExpressProductService(_mockAzureAliExpressRepository.Object, _mockAzureProductService.Object, _mockProductPropertyRepository.Object, _aliExpressOption, _configuration, _mockLogger.Object);
//            var aliExpressProductTest = new List<AliExpressProductDTO>()
//            {
//                new()
//                {
//                    SkuCode = "REINWV1181"
//                }
//            };
//            //arrange
//            var aliExpressProducts = aliExpressProductService.SetInventoryFromDatabase(aliExpressProductTest);
//            //assert
//            Assert.NotNull(aliExpressProducts);
//            Assert.True(aliExpressProducts.Count > 0);
//            Assert.Equal(1, aliExpressProducts.First().Inventory);
//        }


//        [Fact]
//        public async Task TestAliExpressService_GetProductWhereAliExpressProductIdIsNull_CheckNullAliProductId()
//        {
//            //arrange
//            var aliExpressProductService = new AliExpressProductService(_mockAzureAliExpressRepository.Object, _mockAzureProductService.Object, _mockProductPropertyRepository.Object, _aliExpressOption, _configuration, _mockLogger.Object);
//            //act
//            var aliExpressProducts = await aliExpressProductService.GetProductWhereAliExpressProductIdIsNull();
//            //assert
//            Assert.NotNull(aliExpressProducts);
//            Assert.True(aliExpressProducts.Any());
//            Assert.NotNull(aliExpressProducts.First().AliExpressProductId);
//        }

//        [Fact]
//        public async Task TestAliExpressService_GetProductWhereAliExpressProductIdIsNull_NotDuplicateElements()
//        {
//            //arrange
//            var aliExpressProductService = new AliExpressProductService(_mockAzureAliExpressRepository.Object, _mockAzureProductService.Object, _mockProductPropertyRepository.Object, _aliExpressOption, _configuration, _mockLogger.Object);
//            //act
//            var aliExpressProducts = await aliExpressProductService.GetProductWhereAliExpressProductIdIsNull();
//            //assert
//            Assert.NotNull(aliExpressProducts);
//            Assert.True(aliExpressProducts.Any());
//            Assert.True(!aliExpressProducts.GroupBy(x => x.Sku).Where(x => x.Count() > 1).Any());
//        }

//        [Fact]
//        public async Task TestAliExpressService_ListProductsForUpdateInventory_NotDuplicateElements()
//        {
//            //arrange
//            var aliExpressProductService = new AliExpressProductService(_mockAzureAliExpressRepository.Object, _mockAzureProductService.Object, _mockProductPropertyRepository.Object, _aliExpressOption, _configuration, _mockLogger.Object);
//            //act
//            var aliExpressProducts = await aliExpressProductService.ListProductsForUpdateInventory();
//            //assert
//            Assert.NotNull(aliExpressProducts);
//            Assert.True(aliExpressProducts.Any());
//            Assert.True(!aliExpressProducts.GroupBy(x => x.Sku).Where(x => x.Count() > 1).Any());
//        }


//        [Fact]
//        public async Task TestAliExpressService_ListProductsForUpdateInventory_CountElements()
//        {
//            //arrange
//            var aliExpressProductService = new AliExpressProductService(_mockAzureAliExpressRepository.Object, _mockAzureProductService.Object, _mockProductPropertyRepository.Object, _aliExpressOption, _configuration, _mockLogger.Object);
//            //act
//            var aliExpressProducts = await aliExpressProductService.ListProductsForUpdateInventory();
//            //assert
//            Assert.NotNull(aliExpressProducts);
//            Assert.True(aliExpressProducts.Any());
//            Assert.Equal(6822, aliExpressProducts.Count());
//        }

//        [Fact]
//        public async Task TestAliExpressService_ListProductsForUpdateInventory_AliExpressProductsIdNotNull()
//        {
//            //arrange
//            var aliExpressProductService = new AliExpressProductService(_mockAzureAliExpressRepository.Object, _mockAzureProductService.Object, _mockProductPropertyRepository.Object, _aliExpressOption, _configuration, _mockLogger.Object);
//            //act
//            var aliExpressProducts = await aliExpressProductService.ListProductsForUpdateInventory();
//            //assert
//            Assert.NotNull(aliExpressProducts);
//            Assert.True(aliExpressProducts.Any());
//            Assert.False(aliExpressProducts.Any(x => x.AliExpressProductId == null));
//        }


        //[Fact]
        //public void TestAliExpressService_UpdateInventoryProducts_Test()
        //{
        //    //arrange
        //    var aliExpressProductService = new AliExpressProductService(_mockAzureAliExpressRepository.Object, _mockAzureProductService.Object, _aliExpressOption, _configuration, _mockLogger.Object);
        //    var aliExpressProductId = 1005003028580861;
        //    var products = new List<Product>
        //    {
        //        new()
        //        {
        //            Sku = "KVR01.041.052.01200k",
        //            Count = 0,
        //            AliExpressProductId = aliExpressProductId
        //        }
        //    };
        //    //act
        //    aliExpressProductService.UpdateInventoryProducts(products);
        //    var resultAliExpressResponseInventory = aliExpressProductService.GetProductInfo(aliExpressProductId);
        //    //assert
        //    Assert.NotNull(resultAliExpressResponseInventory);
        //    Assert.Equal(0, resultAliExpressResponseInventory.SkuStock);
        //}
    }
}
