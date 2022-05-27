using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;
using YapartMarket.Core.BL;
using YapartMarket.Core.BL.AliExpress;
using YapartMarket.Core.Config;
using YapartMarket.Core.Data.Interfaces.Azure;
using YapartMarket.Core.DTO.AliExpress;
using YapartMarket.Core.Models.Azure;

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
        private readonly IMapper _mapper;
        private ITopClient _client;

        public AliExpressCreateLogisticWarehouseOrderService(IMapper mapper, IOptions<AliExpressOptions> options, 
            IAzureAliExpressOrderReceiptInfoRepository aliExpressOrderReceiptInfoRepository,
            ICategoryRepository categoryRepository,
            IAzureAliExpressProductRepository productRepository,
            IAzureAliExpressOrderDetailRepository orderDetailRepository,
            IProductPropertyRepository productPropertyRepository,
            IOrderSizeCargoPlaceService orderSizeCargoPlaceService)
        {
            _mapper = mapper;
            _options = options;
            _aliExpressOrderReceiptInfoRepository = aliExpressOrderReceiptInfoRepository;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _orderDetailRepository = orderDetailRepository;
            _productPropertyRepository = productPropertyRepository;
            _orderSizeCargoPlaceService = orderSizeCargoPlaceService;
            _client = new DefaultTopClient(options.Value.HttpsEndPoint, options.Value.AppKey, options.Value.AppSecret, "Json");
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
                    productDto.CategoryCnDesc = category.CnName;
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
                req.DomesticLogisticsCompany = "SF Express";
                req.DomesticLogisticsCompanyId = 133; // 505L; //todo - найти какой ставить
                req.DomesticTrackingNo = "none";// "none"; //todo возможно с этим ошибка
                req.TradeOrderFrom = "ESCROW";
                req.TradeOrderId = orderId;

                //var orderWarehouseServicesResponse = _orderSizeCargoPlaceService.GetRequest(orderId);
                //var warehouseService = _orderSizeCargoPlaceService.CreateLogisticsServicesId(orderWarehouseServicesResponse);

                req.WarehouseCarrierService = "AE_RU_MP_COURIER_PH3_REGION;"; //warehouseService; // todo поствить один! 
                AliexpressLogisticsCreatewarehouseorderResponse rsp = _client.Execute(req, _options.Value.AccessToken);
                Console.WriteLine(rsp.Body);
            }
        }
    }
}
