using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;
using YapartMarket.Core.BL.AliExpress;
using YapartMarket.Core.Config;
using YapartMarket.Core.Data.Interfaces.Azure;

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
        private readonly IMapper _mapper;
        private ITopClient _client;

        public AliExpressCreateLogisticWarehouseOrderService(IMapper mapper, IOptions<AliExpressOptions> options, 
            IAzureAliExpressOrderReceiptInfoRepository aliExpressOrderReceiptInfoRepository,
            ICategoryRepository categoryRepository,
            IAzureAliExpressProductRepository productRepository,
            IAzureAliExpressOrderDetailRepository orderDetailRepository,
            IProductPropertyRepository productPropertyRepository)
        {
            _mapper = mapper;
            _options = options;
            _aliExpressOrderReceiptInfoRepository = aliExpressOrderReceiptInfoRepository;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _orderDetailRepository = orderDetailRepository;
            _productPropertyRepository = productPropertyRepository;
            _client = new DefaultTopClient(options.Value.HttpsEndPoint, options.Value.AppKey, options.Value.AppSecret, "Json");
        }
        public async Task CreateOrderAsync(long orderId)
        {
            var orderDetails = await _orderDetailRepository.GetAsync("select * from order_details where order_id = @order_id", new {order_id = orderId});
            var orderInfo = (await _aliExpressOrderReceiptInfoRepository.GetAsync("select * from order_receipt_infos where order_id = @order_id", new { order_id = orderId})).FirstOrDefault();

            
            AliexpressLogisticsCreatewarehouseorderRequest req = new AliexpressLogisticsCreatewarehouseorderRequest();
            AliexpressLogisticsCreatewarehouseorderRequest.AddressdtosDomain obj1 = new AliexpressLogisticsCreatewarehouseorderRequest.AddressdtosDomain();
            AliexpressLogisticsCreatewarehouseorderRequest.AeopWlDeclareAddressDtoDomain receiverObject = new AliexpressLogisticsCreatewarehouseorderRequest.AeopWlDeclareAddressDtoDomain();
            receiverObject.Phone = orderInfo.PhoneNumber;
            receiverObject.PostCode = orderInfo.PostCode;
            receiverObject.Country = orderInfo.Country;
            receiverObject.City = orderInfo.City;
            receiverObject.Name = orderInfo.ContractPerson;
            receiverObject.Province = orderInfo.Province;
            receiverObject.StreetAddress = orderInfo.LocalizedAddress;
            receiverObject.Mobile = orderInfo.Mobile;
            obj1.Receiver = receiverObject;
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
                productDto.ProductDeclareAmount = "1.3";
                //productDto.SkuValue = "sku value";
                productDto.ProductId = product.ProductId;
                productDto.Breakable = false;
                productDto.SkuCode = product.Sku;
                productDto.ContainsBattery = false;
                list7.Add(productDto);
            }
            
            req.DomesticLogisticsCompanyId = 505L;
            req.DomesticTrackingNo = "none";
            req.DeclareProductDTOs_ = list7;
            req.TradeOrderFrom = "ESCROW";
            req.TradeOrderId = orderId;
            req.WarehouseCarrierService = "HRB_WLB_ZTOGZ";
            AliexpressLogisticsCreatewarehouseorderResponse rsp = _client.Execute(req, _options.Value.AccessToken);
            Console.WriteLine(rsp.Body);
        }
    }
}
