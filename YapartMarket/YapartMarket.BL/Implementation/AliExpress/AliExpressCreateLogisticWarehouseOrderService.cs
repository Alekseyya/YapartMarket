using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;
using Top.Api.Util;
using YapartMarket.Core.BL;
using YapartMarket.Core.BL.AliExpress;
using YapartMarket.Core.Config;
using YapartMarket.Core.Data.Interfaces.Azure;
using YapartMarket.Core.DTO.AliExpress;
using YapartMarket.Core.DTO.AliExpress.CreateWarehouse;
using YapartMarket.Core.Extensions;
using YapartMarket.Core.Models.Azure;
using YapartMarket.React.ViewModels.AliExpress;

namespace YapartMarket.BL.Implementation.AliExpress
{
    public class AliExpressCreateLogisticWarehouseOrderService : ILogisticWarehouseOrderService
    {
        private readonly IOptions<AliExpressOptions> _options;
        private readonly IAzureAliExpressOrderReceiptInfoRepository _aliExpressOrderReceiptInfoRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAzureAliExpressProductRepository _productRepository;
        private readonly IAliExpressOrderDetailRepository _orderDetailRepository;
        private readonly IProductPropertyRepository _productPropertyRepository;
        private readonly IOrderSizeCargoPlaceService _orderSizeCargoPlaceService;
        private readonly IFullOrderInfoService _fullOrderInfoService;
        private readonly IAliExpressProductService _productProductService;
        private readonly IMapper _mapper;
        private ITopClient _client;
        private readonly HttpClient _httpClient;

        public AliExpressCreateLogisticWarehouseOrderService(IMapper mapper, IOptions<AliExpressOptions> options,
            IAzureAliExpressOrderReceiptInfoRepository aliExpressOrderReceiptInfoRepository,
            ICategoryRepository categoryRepository,
            IAzureAliExpressProductRepository productRepository,
            IAliExpressOrderDetailRepository orderDetailRepository,
            IProductPropertyRepository productPropertyRepository,
            IOrderSizeCargoPlaceService orderSizeCargoPlaceService,
            IFullOrderInfoService fullOrderInfoService,
            IAliExpressProductService productProductService,
            IHttpClientFactory factory)
        {
            _mapper = mapper;
            _options = options;
            _aliExpressOrderReceiptInfoRepository = aliExpressOrderReceiptInfoRepository;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _orderDetailRepository = orderDetailRepository;
            _productPropertyRepository = productPropertyRepository;
            _orderSizeCargoPlaceService = orderSizeCargoPlaceService;
            _fullOrderInfoService = fullOrderInfoService;
            _productProductService = productProductService;
            _client = new DefaultTopClient(options.Value.HttpsEndPoint, options.Value.AppKey, options.Value.AppSecret, "Json");
            //_httpClient = factory.CreateClient("aliClient");
            _httpClient = factory.CreateClient();
        }

        private void Refund(AliexpressLogisticsCreatewarehouseorderRequest.AddressdtosDomain addressesDomain, Address address)
        {
            var refundObject = new AliexpressLogisticsCreatewarehouseorderRequest.AeopWlDeclareAddressDtoDomain();
            //refundObject.Phone = address.Phone;
            //refundObject.Fax = address.Fax;
            //refundObject.MemberType = address.MemberType;
            //refundObject.TrademanageId = address.TradeManageId;
            //refundObject.Street = address.Street;
            //refundObject.Country = address.Country;
            //refundObject.City = address.City;
            //refundObject.County = address.County;
            //refundObject.Email = address.Email;
            refundObject.AddressId = address.AddressId;
            //refundObject.Name = address.Name;
            //refundObject.Province = address.Province;
            //refundObject.StreetAddress = address.StreetAddress;
            //refundObject.Mobile = address.Mobile;
            //refundObject.PostCode = address.Postcode;
            addressesDomain.Refund = refundObject;
        }

        private void Sender(AliexpressLogisticsCreatewarehouseorderRequest.AddressdtosDomain addressesDomain, Address address)
        {
            AliexpressLogisticsCreatewarehouseorderRequest.AeopWlDeclareAddressDtoDomain senderObject = new AliexpressLogisticsCreatewarehouseorderRequest.AeopWlDeclareAddressDtoDomain();
            //senderObject.Phone = address.Phone;
            //senderObject.PostCode = address.Postcode;
            //senderObject.Country = address.Country;
            ////obj2.Fax = "234234234";
            ////obj2.MemberType = "类型";
            //senderObject.TrademanageId = address.TradeManageId;
            //senderObject.Street = address.Street;
            //senderObject.City = address.City;
            //senderObject.County = address.County;
            //senderObject.Email = address.Email;
            senderObject.AddressId = address.AddressId;
            //senderObject.Name = address.Name;
            //senderObject.Province = address.Province;
            //senderObject.StreetAddress = address.StreetAddress;
            //senderObject.Mobile = address.Mobile;
            addressesDomain.Sender = senderObject;
        }

        private void Pickup(AliexpressLogisticsCreatewarehouseorderRequest.AddressdtosDomain addressesDomain, Address address)
        {
            var pickupObject = new AliexpressLogisticsCreatewarehouseorderRequest.AeopWlDeclareAddressDtoDomain();
            //pickupObject.Phone = address.Phone;
            //pickupObject.Fax = "234234234";
            ////pickupObject.MemberType = "类型";
            ////pickupObject.TrademanageId = address.TradeManageId;
            //pickupObject.Street = address.Street;
            //pickupObject.Country = address.Country;
            //pickupObject.City = address.City;
            //pickupObject.County = address.County;
            //pickupObject.Email = address.Email;
            pickupObject.AddressId = address.AddressId;
            //pickupObject.Name = address.Name;
            //pickupObject.Province = address.Province;
            //pickupObject.StreetAddress = address.StreetAddress;
            //pickupObject.Mobile = address.Mobile;
            //pickupObject.PostCode = address.Postcode;
            ////pickupObject.FromWarehouseCode = "AML001";
            addressesDomain.Pickup = pickupObject;
        }

        private void Receiver(AliexpressLogisticsCreatewarehouseorderRequest.AddressdtosDomain addressesDomain, AliExpressOrderReceiptInfo orderInfo)
        {
            var receiverObject = new AliexpressLogisticsCreatewarehouseorderRequest.AeopWlDeclareAddressDtoDomain();
            receiverObject.Phone = orderInfo.PhoneNumber;
            receiverObject.Fax = orderInfo.FaxNumber;
            receiverObject.PostCode = orderInfo.PostCode;
            receiverObject.Country = orderInfo.Country;
            receiverObject.City = orderInfo.City;
            receiverObject.Name = orderInfo.ContractPerson;
            receiverObject.Province = orderInfo.Province;
            //receiverObject.TrademanageId = orderInfo.
            //receiverObject.AddressId = orderInfo.Address.AddressId; //todo ЭТО ВАЖНО!!
            receiverObject.Street = orderInfo.StreetDetailedAddress;
            receiverObject.StreetAddress = orderInfo.LocalizedAddress;
            receiverObject.Mobile = orderInfo.Mobile;
            addressesDomain.Receiver = receiverObject;
        }
        private async Task<bool> SendRequest(string url, string body)
        {
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            var result = await _httpClient.PostAsync(url, content);
            var resultContent = await result.Content.ReadAsStringAsync();
            if (resultContent.TryParseJson(out SuccessfulResponse successfulResponse))
                return true;
            if (resultContent.TryParseJson(out ErrorResponse errorResponse))
                Log.Instance.Error(errorResponse.cainiao_global_logistic_order_create_response.error_info.error_msg);
            return false;
        }
        private async Task<bool> SendCreateWarehouseRequest(CreateWarehouseDTO createWarehouseDto)
        {
            TopDictionary topDictionary = new TopDictionary();
            topDictionary.Add("method", "cainiao.global.logistic.order.create");
            topDictionary.Add("v", "2.0");
            topDictionary.Add("sign_method", "hmac");
            topDictionary.Add("app_key", _options.Value.AppKey);
            topDictionary.Add("format", "json");
            //topDictionary.Add("target_app_key", request.GetTargetAppKey());
            topDictionary.Add("session", _options.Value.AccessToken);
            //topDictionary.AddAll(this.systemParameters);
            //if (this.useSimplifyJson)
            //    topDictionary.Add("simplify", "true");
            var sign = TopUtils.SignTopRequest(topDictionary, _options.Value.AppSecret, "hmac");
            var dateTime = DateTime.UtcNow;
            var singapore = TimeZoneInfo.FindSystemTimeZoneById("China Standard Time");
            var chinaTimeZone = TimeZoneInfo.ConvertTimeFromUtc(dateTime, singapore); //гггг-мм-дд ЧЧ:ММ:СС
            var chinaTimeZoneString = chinaTimeZone.ToString("yyyy-MM-dd HH:mm:ss");
            var body = JsonConvert.SerializeObject(createWarehouseDto);
            var url = $"?method=cainiao.global.logistic.order.create&v=2.0&app_key={_options.Value.AppKey}&session={_options.Value.AccessToken}&format=json&timestamp={chinaTimeZoneString}&sign_method=hmac&sign={sign}";
            var response = await SendRequest(url, body);
            return response;
        }

        public class SolutionServiceRoot
        {
            [JsonProperty("service_param")]
            public SolutionService SolutionService { get; set; }
            [JsonProperty("solution_code")]
            public string SolutionCode { get; set; }
        }
        public class SolutionService
        {
            [JsonProperty("code")]
            public string Code { get; set; }
        }

        public class Seller
        {
            [JsonProperty("seller_address_id")]
            public long SellerAddressId { get; set; }
        }

        public async Task CreateWarehouseAsync(long orderId)
        {

            
            //return JsonConvert.DeserializeObject<SuccessfulResponse>(resultContent);
            //_testOutputHelper.WriteLine(resultContent);

            var orderDetails = await _orderDetailRepository.GetAsync("select * from order_details where order_id = @order_id", new { order_id = orderId });
            if (orderDetails.IsAny())
            {
                var orderInfo = (await _aliExpressOrderReceiptInfoRepository.GetAsync("select * from order_receipt_infos where order_id = @order_id", new { order_id = orderId })).FirstOrDefault();
                var warehouseService = (await _fullOrderInfoService.GetRequest(orderId))?.aliexpress_trade_new_redefining_findorderbyid_response?.
                    target.child_order_list?.aeop_tp_child_order_dto?.FirstOrDefault()!.logistics_type;
                AliexpressLogisticsRedefiningGetlogisticsselleraddressesRequest reqSender = new AliexpressLogisticsRedefiningGetlogisticsselleraddressesRequest();
                reqSender.SellerAddressQuery = "sender,pickup,refund";
                var rspSender = _client.Execute(reqSender, _options.Value.AccessToken);
                var senderAddresses = JsonConvert.DeserializeObject<SenderRoot>(rspSender.Body)?.Sender.SenderSellerAddressList.SenderSellerAddress;
                var refundAddresses = JsonConvert.DeserializeObject<SenderRoot>(rspSender.Body)?.Sender.RefundSellerAddressList.RefundSellerAddresses;
                var pickupAddresses = JsonConvert.DeserializeObject<SenderRoot>(rspSender.Body)?.Sender.PickupSellerAddressList.PickupSellerAddresses;
                var sender = senderAddresses.First();
                var refund = refundAddresses.First();
                var pickup = pickupAddresses.First();
                var orderProductsId = orderDetails.Select(x => x.ProductId).ToList();
                var products = await _productProductService.GetProductFromAli(orderProductsId);
                var items = new List<CainiaoGlobalLogisticOrderCreateRequest.OpenItemParamDomain>();

                var dateTime = DateTime.UtcNow;
                var singapore = TimeZoneInfo.FindSystemTimeZoneById("China Standard Time");
                var chinaTimeZone = TimeZoneInfo.ConvertTimeFromUtc(dateTime, singapore); //гггг-мм-дд ЧЧ:ММ:СС
                var chinaTimeZoneString = chinaTimeZone.ToString("yyyy-MM-dd HH:mm:ss");

                var dic = new Dictionary<string, string>();
                dic.Add("method", "cainiao.global.solution.service.resource.query");
                dic.Add("v", "2.0");
                dic.Add("sign_method", "hmac");
                dic.Add("app_key", _options.Value.AppKey);
                dic.Add("format", "json");
                dic.Add("timestamp", chinaTimeZoneString);
                dic.Add("session", _options.Value.AccessToken);
                dic.Add("sign", TopUtils.SignTopRequest(dic, _options.Value.AppSecret, "hmac"));
                var senderParam = new Seller()
                {
                    SellerAddressId = sender.AddressId
                };
                var solutionService = new SolutionServiceRoot()
                {
                    SolutionService = new()
                    {
                        Code = "DOOR_PICKUP",
                    },
                    SolutionCode = warehouseService
                };
                var senderParamJson = JsonConvert.SerializeObject(senderParam);
                var solutionServiceJson = JsonConvert.SerializeObject(solutionService);
                dic.Add("seller_param", "{}");
                dic.Add("sender_param", senderParamJson);
                dic.Add("solution_service_res_param", solutionServiceJson);
                var keyValuePairs = dic.OrderBy(x => x.Key);
                var url = $"https://eco.taobao.com/router/rest?{string.Join("&", keyValuePairs.Select(kvp => $"{kvp.Key}={kvp.Value}"))}";
                //var url = $"https://eco.taobao.com/router/rest?{HttpUtility.UrlEncode(string.Join("&", keyValuePairs.Select(kvp => $"{kvp.Key}={kvp.Value}")))}";

                var content = new StringContent("", Encoding.UTF8, "application/json");
                var result = await _httpClient.PostAsync(url, content);
                string resultContent = await result.Content.ReadAsStringAsync();

                foreach (var orderDetail in orderDetails)
                {
                    var product = products.FirstOrDefault(x => x.ProductId == orderDetail.ProductId);
                    var sku = product.ProductInfoSku.GlobalProductSkus?.FirstOrDefault()?.SkuCode;
                    var englishName = product.MultiLanguageList.GlobalSubjects.FirstOrDefault(x => x.Locale == "en_US")?.Subject;
                    var localeName = product.MultiLanguageList.GlobalSubjects.FirstOrDefault(x => x.Locale == "ru_RU")?.Subject;
                    var unitPrice = Convert.ToInt64(Math.Round(decimal.Parse(product.ProductInfoSku.GlobalProductSkus?.FirstOrDefault()?.DiscountPrice!, new CultureInfo("en-US"))));
                    var totalPrice = Convert.ToInt64(orderDetail.TotalProductAmount);
                    var gramms = Convert.ToInt32(product.GrossWeight.Split(".")[1]);
                    var kilo = Convert.ToInt32(product.GrossWeight.Split(".")[0]);
                    var summaryGramms = (kilo * 1000) + gramms;
                    if (product != null)
                    {
                        items.Add(new()
                        {
                            ItemId = product.ProductId,
                            Sku = sku,
                            EnglishName = englishName,
                            LocalName = localeName,
                            Height = product.PackageHeight,
                            Length = product.PackageLength,
                            Weight = summaryGramms,
                            Width = product.PackageWidth,
                            UnitPrice = unitPrice,
                            TotalPrice = totalPrice,
                            Quantity = orderDetail.ProductCount,
                            Currency = "RUB"
                        });
                    }
                }

                var request = new CainiaoGlobalLogisticOrderCreateRequest();
                var orderParam = new CainiaoGlobalLogisticOrderCreateRequest.OpenOrderParamDomain();

                orderParam.TradeOrderParam = new()
                {
                    TradeOrderId = orderId
                };
                orderParam.SolutionParam = new()
                {
                    SolutionCode = warehouseService,
                    ServiceParams = new List<CainiaoGlobalLogisticOrderCreateRequest.OpenServiceParamDomain>()
                    {
                        new()
                        {
                            Code = "SELF_SEND",
                            Features = new()
                            {
                                WarehouseCode = warehouseService
                            }
                        }
                    }
                };
                orderParam.SenderParam = new()
                {
                    SellerAddressId = sender.AddressId
                };
                orderParam.SellerInfoParam = new() //todo неизвестно обязательный ли он
                {
                    TopUserKey = ""
                };
                orderParam.ReturnerParam = new()
                {
                    SellerAddressId = refund.AddressId
                };
                orderParam.ReceiverParam = new()
                {
                    Name = orderInfo.ContractPerson,
                    Telephone = !string.IsNullOrEmpty(orderInfo.PhoneNumber) ? orderInfo.PhoneNumber : "",
                    MobilePhone = !string.IsNullOrEmpty(orderInfo.Mobile) ? orderInfo.Mobile : "",
                    AddressParam = new()
                    {
                        CountryCode = orderInfo.Country,
                        CountryName = orderInfo.CountryName,
                        Province = orderInfo.Province,
                        City = orderInfo.City,
                        DetailAddress = orderInfo.LocalizedAddress,
                        ZipCode = orderInfo.PostCode
                    }
                };
                orderParam.PickupInfoParam = new()
                {
                    SellerAddressId = pickup.AddressId
                };
                orderParam.PackageParams = new List<CainiaoGlobalLogisticOrderCreateRequest.OpenPackageParamDomain>()
                {
                    new CainiaoGlobalLogisticOrderCreateRequest.OpenPackageParamDomain()
                    {
                        ItemParams = items
                    }
                };
                request.Locale = "ru_RU";
                request.OrderParam_ = orderParam;
                var response = _client.Execute(request, _options.Value.AccessToken);
                Console.WriteLine(response.Body);
            }
        }

        public async Task CreateWarehouseOrderAsync(long orderId)
        {

            

            var orderDetails = await _orderDetailRepository.GetAsync("select * from order_details where order_id = @order_id", new { order_id = orderId });
            if (orderDetails.IsAny())
            {
                var orderInfo = (await _aliExpressOrderReceiptInfoRepository.GetAsync("select * from order_receipt_infos where order_id = @order_id", new { order_id = orderId })).FirstOrDefault();
                var warehouseService = (await  _fullOrderInfoService.GetRequest(orderId)).aliexpress_trade_new_redefining_findorderbyid_response.
                    target.child_order_list.aeop_tp_child_order_dto
                    .FirstOrDefault().logistics_type;
                AliexpressLogisticsRedefiningGetlogisticsselleraddressesRequest reqSender = new AliexpressLogisticsRedefiningGetlogisticsselleraddressesRequest();
                reqSender.SellerAddressQuery = "sender,pickup,refund";
                var rspSender = _client.Execute(reqSender, _options.Value.AccessToken);
                var sender = JsonConvert.DeserializeObject<SenderRoot>(rspSender.Body).Sender.SenderSellerAddressList.SenderSellerAddress.First();
                var refund = JsonConvert.DeserializeObject<SenderRoot>(rspSender.Body).Sender.RefundSellerAddressList.RefundSellerAddresses.First();
                var pickup = JsonConvert.DeserializeObject<SenderRoot>(rspSender.Body).Sender.PickupSellerAddressList.PickupSellerAddresses.First();
                var orderProductsId = orderDetails.Select(x => x.ProductId).ToList();
                var products = await _productProductService.GetProductFromAli(orderProductsId);
                var itemsInOrder = new List<ItemParam>();
                foreach (var orderDetail in orderDetails)
                {
                    var product = products.FirstOrDefault(x => x.ProductId == orderDetail.ProductId);
                    var sku = product.ProductInfoSku.GlobalProductSkus.FirstOrDefault()?.SkuCode;
                    var englishName = product.MultiLanguageList.GlobalSubjects.FirstOrDefault(x => x.Locale == "en_US")?.Subject;
                    var localeName = product.MultiLanguageList.GlobalSubjects.FirstOrDefault(x => x.Locale == "ru_RU")?.Subject;
                    var unitPrice = Convert.ToInt64(Math.Round(decimal.Parse(product.ProductInfoSku.GlobalProductSkus.FirstOrDefault()?.DiscountPrice)));
                    var totalPrice = Convert.ToInt64(orderDetail.TotalProductAmount);
                    if (product != null)
                    {
                        itemsInOrder.Add(new()
                        {
                            item_id = product.ProductId,
                            sku = sku,
                            english_name = englishName,
                            local_name = localeName,
                            length = product.PackageLength,
                            width = product.PackageWidth,
                            height = product.PackageHeight,
                            quantity = orderDetail.ProductCount,
                            unit_price = unitPrice,
                            total_price = totalPrice,
                            currency = "RUB",
                            item_features = new []{ "cf_normal" }
                        });
                    }
                }

                var result = new CreateWarehouseDTO()
                {
                    locale = "ru_RU",
                    order_param = new()
                    {
                        trade_order_param = new()
                        {
                            trade_order_id = orderId
                        },
                        solution_param = new()
                        {
                            solution_code = warehouseService,
                            service_params = new List<ServiceParam>
                        {
                            new()
                            {
                                code = "SELF_SEND",
                                features = new()
                                {
                                    warehouse_code = warehouseService
                                }
                            }
                        }
                        },
                        seller_info_param = new()
                        {

                        },
                        sender_param = new()
                        {
                            seller_address_id = sender.AddressId
                        },
                        returner_param = new()
                        {
                            seller_address_id = refund.AddressId
                        },
                        pickup_info_param = new()
                        {
                            seller_address_id = pickup.AddressId
                        },
                        receiver_param = new()
                        {
                            name = orderInfo.ContractPerson,
                            telephone =  !string.IsNullOrEmpty(orderInfo.PhoneNumber) ? orderInfo.PhoneNumber : "",
                            mobile_phone = !string.IsNullOrEmpty(orderInfo.Mobile) ? orderInfo.Mobile : "",
                            address_param = new()
                            {
                                country_code = orderInfo.Country,
                                country_name = orderInfo.CountryName,
                                province = orderInfo.Province,
                                city = orderInfo.City,
                                detail_address = orderInfo.LocalizedAddress,
                                zip_code = orderInfo.PostCode
                            }
                        },
                        package_params = new List<PackageParam>
                            {
                                new PackageParam
                                {
                                    item_params = itemsInOrder
                                }
                            }
                    }
                };
                await SendCreateWarehouseRequest(result);
            }
        }

        public async Task CreateOrderAsync(long orderId)
        {
            var orderDetails = await _orderDetailRepository.GetAsync("select * from order_details where order_id = @order_id", new { order_id = orderId });
            var orderInfo = (await _aliExpressOrderReceiptInfoRepository.GetAsync("select * from order_receipt_infos where order_id = @order_id", new { order_id = orderId })).FirstOrDefault();
            if (orderDetails.Any())
            {
                AliexpressLogisticsCreatewarehouseorderRequest req = new AliexpressLogisticsCreatewarehouseorderRequest();
                AliexpressLogisticsCreatewarehouseorderRequest.AddressdtosDomain addressesDomain = new AliexpressLogisticsCreatewarehouseorderRequest.AddressdtosDomain();

                AliexpressLogisticsRedefiningGetlogisticsselleraddressesRequest reqSender = new AliexpressLogisticsRedefiningGetlogisticsselleraddressesRequest();
                reqSender.SellerAddressQuery = "sender,pickup,refund";
                var rspSender = _client.Execute(reqSender, _options.Value.AccessToken);
                var sender = JsonConvert.DeserializeObject<SenderRoot>(rspSender.Body).Sender.SenderSellerAddressList.SenderSellerAddress.First();
                var refund = JsonConvert.DeserializeObject<SenderRoot>(rspSender.Body).Sender.RefundSellerAddressList.RefundSellerAddresses.First();
                var pickup = JsonConvert.DeserializeObject<SenderRoot>(rspSender.Body).Sender.PickupSellerAddressList.PickupSellerAddresses.First();

                Sender(addressesDomain, sender);
                Pickup(addressesDomain, pickup);
                Refund(addressesDomain, refund);
                Receiver(addressesDomain, orderInfo);

                req.AddressDTOs_ = addressesDomain;
                List<AliexpressLogisticsCreatewarehouseorderRequest.AeopWlDeclareProductForTopDtoDomain> list7 = new List<AliexpressLogisticsCreatewarehouseorderRequest.AeopWlDeclareProductForTopDtoDomain>();
                foreach (var orderDetail in orderDetails)
                {
                    var product = (await _productRepository.GetAsync("select * from aliExpressProducts where productId = @productId", new { productId = orderDetail.ProductId })).FirstOrDefault();
                    var category = (await _categoryRepository.GetAsync("select * from ali_category where category_id = @category_id", new { category_id = product.CategoryId })).FirstOrDefault();
                    //var productProperties = (await _productPropertyRepository.GetAsync("select * from ali_product_properties where product_id = @product_id", new {product_id = product.ProductId}));
                    var productDto = new AliexpressLogisticsCreatewarehouseorderRequest.AeopWlDeclareProductForTopDtoDomain();
                    productDto.CategoryCnDesc = category.CnName; // todo вот с этим вопрос!
                    productDto.CategoryEnDesc = category.EnName;
                    productDto.ProductWeight = product.GrossWeight;
                    productDto.ProductNum = orderDetail.ProductCount;
                    productDto.ProductDeclareAmount = "1.3"; //todo взять откуда-то
                    productDto.SkuValue = "black";
                    productDto.ProductId = product.ProductId;
                    productDto.Breakable = false;
                    productDto.SkuCode = product.Sku;
                    productDto.AneroidMarkup = false;
                    productDto.ContainsBattery = false;
                    productDto.ChildOrderId = orderId;
                    list7.Add(productDto);
                }
                req.DeclareProductDTOs_ = list7;
                //todo добавить полную инф о продукте
                //req.DomesticLogisticsCompany = "SF Express";//todo необязательное поле, тоже самое! WarehouseCarrierService -  AE_RU_MP_COURIER_PH3_REGION
                req.DomesticLogisticsCompanyId = 505; // 505L; //todo - найти какой ставить
                req.DomesticTrackingNo = "none";// "none"; //todo возможно с этим ошибка
                req.TradeOrderFrom = "none"; // "ESCROW";
                req.TradeOrderId = orderId;


                var warehouseService = (await _fullOrderInfoService.GetRequest(orderId)).aliexpress_trade_new_redefining_findorderbyid_response.
                    target.child_order_list.aeop_tp_child_order_dto
                    .FirstOrDefault().logistics_type;

                req.WarehouseCarrierService = warehouseService;
                AliexpressLogisticsCreatewarehouseorderResponse rsp = _client.Execute(req, _options.Value.AccessToken);
                Console.WriteLine(rsp.Body);
            }
        }
    }
}
