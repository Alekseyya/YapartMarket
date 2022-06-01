﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
        private readonly IAzureAliExpressOrderDetailRepository _orderDetailRepository;
        private readonly IProductPropertyRepository _productPropertyRepository;
        private readonly IOrderSizeCargoPlaceService _orderSizeCargoPlaceService;
        private readonly IFullOrderInfoService _fullOrderInfoService;
        private readonly IMapper _mapper;
        private ITopClient _client;
        private readonly HttpClient _httpClient;

        public AliExpressCreateLogisticWarehouseOrderService(IMapper mapper, IOptions<AliExpressOptions> options, 
            IAzureAliExpressOrderReceiptInfoRepository aliExpressOrderReceiptInfoRepository,
            ICategoryRepository categoryRepository,
            IAzureAliExpressProductRepository productRepository,
            IAzureAliExpressOrderDetailRepository orderDetailRepository,
            IProductPropertyRepository productPropertyRepository,
            IOrderSizeCargoPlaceService orderSizeCargoPlaceService,
            IFullOrderInfoService fullOrderInfoService, IHttpClientFactory factory)
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
            _client = new DefaultTopClient(options.Value.HttpsEndPoint, options.Value.AppKey, options.Value.AppSecret, "Json");
            _httpClient = factory.CreateClient("aliClient");
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
            if(resultContent.TryParseJson(out ErrorResponse errorResponse))
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
            var chinaTimeZoneString =chinaTimeZone.ToString("yyyy-MM-dd HH:mm:ss");
            var body = JsonConvert.SerializeObject(createWarehouseDto);
            var url = $"?method=cainiao.global.logistic.order.create&v=2.0&app_key={_options.Value.AppKey}&session={_options.Value.AccessToken}&format=json&timestamp={chinaTimeZoneString}&sign_method=hmac&sign={sign}";
            var response = await SendRequest(url, body);
            return response;
        }

        public async Task CreateWarehouseOrderAsync(long orderId)
        {
            var orderDetails = await _orderDetailRepository.GetAsync("select * from order_details where order_id = @order_id", new { order_id = orderId });
            if (orderDetails.IsAny())
            {
                var orderInfo = (await _aliExpressOrderReceiptInfoRepository.GetAsync("select * from order_receipt_infos where order_id = @order_id", new { order_id = orderId })).FirstOrDefault();
                var warehouseService = _fullOrderInfoService.GetRequest(orderId).aliexpress_trade_new_redefining_findorderbyid_response.
                    target.child_order_list.aeop_tp_child_order_dto
                    .FirstOrDefault().logistics_type;
                AliexpressLogisticsRedefiningGetlogisticsselleraddressesRequest reqSender = new AliexpressLogisticsRedefiningGetlogisticsselleraddressesRequest();
                reqSender.SellerAddressQuery = "sender,pickup,refund";
                var rspSender = _client.Execute(reqSender, _options.Value.AccessToken);
                var sender = JsonConvert.DeserializeObject<SenderRoot>(rspSender.Body).Sender.SenderSellerAddressList.SenderSellerAddress.First();
                var refund = JsonConvert.DeserializeObject<SenderRoot>(rspSender.Body).Sender.RefundSellerAddressList.RefundSellerAddresses.First();
                var pickup = JsonConvert.DeserializeObject<SenderRoot>(rspSender.Body).Sender.PickupSellerAddressList.PickupSellerAddresses.First();

                var itemsInOrder = new List<ItemParam>();
                foreach (var orderDetail in orderDetails)
                {
                    itemsInOrder.Add(new()
                    {
                        item_id = orderDetail.ProductId,
                        //sku = orderDetail. //Todo Дописать!!!
                    });
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
                        //seller_info_param = new()
                        //{
                        //    top_user_key = 
                        //}
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
                            telephone = orderInfo.PhoneNumber,
                            mobile_phone = orderInfo.Mobile,
                            address_param = new()
                            {
                                country_code = orderInfo.Country,
                                country_name = "Russia",
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
                            item_params = new List<ItemParam>()
                            {
                                new()
                            }
                        }
                    }
                    }
                };
            }
        }

        public async Task CreateOrderAsync(long orderId)
        {
            var orderDetails = await _orderDetailRepository.GetAsync("select * from order_details where order_id = @order_id", new {order_id = orderId});
            var orderInfo = (await _aliExpressOrderReceiptInfoRepository.GetAsync("select * from order_receipt_infos where order_id = @order_id", new { order_id = orderId})).FirstOrDefault();
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

                
                var warehouseService = _fullOrderInfoService.GetRequest(orderId).aliexpress_trade_new_redefining_findorderbyid_response.
                    target.child_order_list.aeop_tp_child_order_dto
                    .FirstOrDefault().logistics_type;

                req.WarehouseCarrierService = warehouseService;
                AliexpressLogisticsCreatewarehouseorderResponse rsp = _client.Execute(req, _options.Value.AccessToken);
                Console.WriteLine(rsp.Body);
            }
        }
    }
}