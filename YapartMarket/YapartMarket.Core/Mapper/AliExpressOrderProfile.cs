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
                .ForMember(aliExpressOrderDetail => aliExpressOrderDetail.ProductUnitPrice,
                    aliExpressOrderProductDto => aliExpressOrderProductDto.MapFrom(x => Convert.ToDecimal(x.product_unit_price.amount, new CultureInfo("en-US"))))
                .ForMember(aliExpressOrderDetail => aliExpressOrderDetail.SendGoodsOperator,
                    aliExpressOrderProductDto => aliExpressOrderProductDto.MapFrom(x => x.send_goods_operator))
                .ForMember(aliExpressOrderDetail => aliExpressOrderDetail.ShowStatus,
                    aliExpressOrderProductDto => aliExpressOrderProductDto.MapFrom(x => x.show_status))
                .ForMember(aliExpressOrderDetail => aliExpressOrderDetail.GoodsPrepareTime,
                    aliExpressOrderProductDto => aliExpressOrderProductDto.MapFrom(x => x.goods_prepare_time))
                .ForMember(aliExpressOrderDetail => aliExpressOrderDetail.TotalProductAmount,
                    aliExpressOrderProductDto => aliExpressOrderProductDto.MapFrom(x => Convert.ToDecimal(x.total_product_amount.amount, new CultureInfo("en-US"))));

            CreateMap<OrderDto, AliExpressOrder>()
                .ForMember(aliExpressOrder => aliExpressOrder.SellerSignerFullName,
                    aliExpressOrderDto => aliExpressOrderDto.MapFrom(x => x.SellerSignerFullname))
                .ForMember(aliExpressOrder => aliExpressOrder.SellerLoginId,
                    aliExpressOrderDto => aliExpressOrderDto.MapFrom(x => x.seller_login_id))
                .ForMember(aliExpressOrder => aliExpressOrder.OrderId,
                    aliExpressOrderDto => aliExpressOrderDto.MapFrom(x => x.order_id))
                .ForMember(aliExpressOrder => aliExpressOrder.LogisticsStatus,
                    aliExpressOrderDto => aliExpressOrderDto.MapFrom(x => x.logistics_status))
                .ForMember(aliExpressOrder => aliExpressOrder.BizType,
                    aliExpressOrderDto => aliExpressOrderDto.MapFrom(x => x.biz_type))
                .ForMember(aliExpressOrder => aliExpressOrder.GmtPayTime,
                    aliExpressOrderDto => aliExpressOrderDto.MapFrom(x => x.gmt_pay_time))
                .ForMember(aliExpressOrder => aliExpressOrder.EndReason,
                    aliExpressOrderDto => aliExpressOrderDto.MapFrom(x => x.end_reason))
                .ForMember(aliExpressOrder => aliExpressOrder.TotalProductCount,
                    aliExpressOrderDto => aliExpressOrderDto.MapFrom(x =>
                        x.product_list.order_product_dto.Select(t => t.product_count).Aggregate((a, b) => a + b)))
                .ForMember(aliExpressOrder => aliExpressOrder.TotalPayAmount,
                    aliExpressOrderDto => aliExpressOrderDto.MapFrom(x =>
                        x.product_list.order_product_dto.Select(t => Convert.ToDecimal(t.total_product_amount.amount, new CultureInfo("en-US")))
                            .Aggregate((a, b) => a + b)))
                .ForMember(aliExpressOrder => aliExpressOrder.OrderStatus,
                    aliExpressOrderDto => aliExpressOrderDto.MapFrom(x => x.order_status))
                .ForMember(aliExpressOrder => aliExpressOrder.GmtCreate,
                    aliExpressOrderDto => aliExpressOrderDto.MapFrom(x => x.gmt_create))
                .ForMember(aliExpressOrder => aliExpressOrder.GmtUpdate,
                    aliExpressOrderDto => aliExpressOrderDto.MapFrom(x => x.gmt_update))
                .ForMember(aliExpressOrder => aliExpressOrder.FundStatus,
                    aliExpressOrderDto => aliExpressOrderDto.MapFrom(x => x.fund_status))
                .ForMember(aliExpressOrder => aliExpressOrder.FrozenStatus,
                    aliExpressOrderDto => aliExpressOrderDto.MapFrom(x => x.frozen_status))

            .ForMember(aliExpressOrder => aliExpressOrder.AliExpressOrderDetails,
                aliExpressOrderDto => aliExpressOrderDto.MapFrom(x => x.product_list.order_product_dto));



        }
    }
}
