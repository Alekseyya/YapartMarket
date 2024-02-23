using System;
using System.Globalization;
using System.Linq;
using AutoMapper;
using YapartMarket.Core.DTO.AliExpress.OrderGetResponse;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Core.Mapper
{
    public class AliExpressOrderProfile : Profile
    {
        public AliExpressOrderProfile()
        {
            CreateMap<OrderProductDto, AliExpressOrderDetail>()
                .ForMember(aliExpressOrderDetail => aliExpressOrderDetail.LogisticsServiceName,
                    aliExpressOrderProductDto => aliExpressOrderProductDto.MapFrom(x => x.logistics_service_name))
                .ForMember(aliExpressOrderDetail => aliExpressOrderDetail.OrderId,
                    aliExpressOrderProductDto => aliExpressOrderProductDto.MapFrom(x => x.order_id))
                .ForMember(aliExpressOrderDetail => aliExpressOrderDetail.OrderId,
                    aliExpressOrderProductDto => aliExpressOrderProductDto.Ignore())
                .ForMember(aliExpressOrderDetail => aliExpressOrderDetail.ProductCount,
                    aliExpressOrderProductDto => aliExpressOrderProductDto.MapFrom(x => x.product_count))
                .ForMember(aliExpressOrderDetail => aliExpressOrderDetail.ProductId,
                    aliExpressOrderProductDto => aliExpressOrderProductDto.MapFrom(x => x.product_id))
                .ForMember(aliExpressOrderDetail => aliExpressOrderDetail.ProductName,
                    aliExpressOrderProductDto => aliExpressOrderProductDto.MapFrom(x => x.product_name))
                .ForMember(aliExpressOrderDetail => aliExpressOrderDetail.ItemPrice,
                    aliExpressOrderProductDto => aliExpressOrderProductDto.MapFrom(x => Convert.ToDecimal(x.product_unit_price!.amount, new CultureInfo("en-US"))))
                .ForMember(aliExpressOrderDetail => aliExpressOrderDetail.ShowStatus,
                    aliExpressOrderProductDto => aliExpressOrderProductDto.MapFrom(x => x.show_status))
                .ForMember(aliExpressOrderDetail => aliExpressOrderDetail.GoodsPrepareDays,
                    aliExpressOrderProductDto => aliExpressOrderProductDto.MapFrom(x => x.goods_prepare_time))
                .ForMember(aliExpressOrderDetail => aliExpressOrderDetail.TotalProductAmount,
                    aliExpressOrderProductDto => aliExpressOrderProductDto.MapFrom(x => Convert.ToDecimal(x.total_product_amount!.amount, new CultureInfo("en-US"))));

            CreateMap<OrderDto, AliExpressOrder>()
                .ForMember(aliExpressOrder => aliExpressOrder.BuyerName,
                    aliExpressOrderDto => aliExpressOrderDto.MapFrom(x => x.SellerSignerFullname))
                .ForMember(aliExpressOrder => aliExpressOrder.OrderId,
                    aliExpressOrderDto => aliExpressOrderDto.MapFrom(x => x.order_id))
                .ForMember(aliExpressOrder => aliExpressOrder.PaidAt,
                    aliExpressOrderDto => aliExpressOrderDto.MapFrom(x => x.gmt_pay_time))
                .ForMember(aliExpressOrder => aliExpressOrder.TotalProductCount,
                    aliExpressOrderDto => aliExpressOrderDto.MapFrom(x =>
                        x.product_list!.order_product_dto.Select(t => t.product_count).Aggregate((a, b) => a + b)))
                .ForMember(aliExpressOrder => aliExpressOrder.TotalPayAmount,
                    aliExpressOrderDto => aliExpressOrderDto.MapFrom(x =>
                        x.product_list!.order_product_dto.Select(t => Convert.ToDecimal(t.total_product_amount!.amount, new CultureInfo("en-US")))
                            .Aggregate((a, b) => a + b)))
                .ForMember(aliExpressOrder => aliExpressOrder.OrderStatus,
                    aliExpressOrderDto => aliExpressOrderDto.MapFrom(x => x.order_status))
                .ForMember(aliExpressOrder => aliExpressOrder.CreateAt,
                    aliExpressOrderDto => aliExpressOrderDto.MapFrom(x => x.gmt_create))
                .ForMember(aliExpressOrder => aliExpressOrder.UpdateAt,
                    aliExpressOrderDto => aliExpressOrderDto.MapFrom(x => x.gmt_update))
                .ForMember(aliExpressOrder => aliExpressOrder.PaymentStatus,
                    aliExpressOrderDto => aliExpressOrderDto.MapFrom(x => x.fund_status))

            .ForMember(aliExpressOrder => aliExpressOrder.AliExpressOrderDetails,
                aliExpressOrderDto => aliExpressOrderDto.MapFrom(x => x.product_list!.order_product_dto));



        }
    }
}
