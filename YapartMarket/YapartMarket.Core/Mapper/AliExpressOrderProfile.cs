using System;
using System.Linq;
using AutoMapper;
using YapartMarket.Core.DTO;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Core.Mapper
{
    public class AliExpressOrderProfile : Profile
    {
        public AliExpressOrderProfile()
        {
            CreateMap<AliExpressOrderProductDTO, AliExpressOrderDetail>()
                .ForMember(aliExpressOrderDetail => aliExpressOrderDetail.LogisticsServiceName,
                    aliExpressOrderProductDto => aliExpressOrderProductDto.MapFrom(x => x.LogisticsServiceName))
                .ForMember(aliExpressOrderDetail => aliExpressOrderDetail.AliOrderId,
                    aliExpressOrderProductDto => aliExpressOrderProductDto.MapFrom(x => x.OrderId))
                .ForMember(aliExpressOrderDetail => aliExpressOrderDetail.OrderId,
                    aliExpressOrderProductDto => aliExpressOrderProductDto.Ignore())
                .ForMember(aliExpressOrderDetail => aliExpressOrderDetail.ProductCount,
                    aliExpressOrderProductDto => aliExpressOrderProductDto.MapFrom(x => x.ProductCount))
                .ForMember(aliExpressOrderDetail => aliExpressOrderDetail.ProductId,
                    aliExpressOrderProductDto => aliExpressOrderProductDto.MapFrom(x => x.ProductId))
                .ForMember(aliExpressOrderDetail => aliExpressOrderDetail.ProductName,
                    aliExpressOrderProductDto => aliExpressOrderProductDto.MapFrom(x => x.ProductName))
                .ForMember(aliExpressOrderDetail => aliExpressOrderDetail.ProductUnitPrice,
                    aliExpressOrderProductDto => aliExpressOrderProductDto.MapFrom(x => x.ProductUnitPrice))
                .ForMember(aliExpressOrderDetail => aliExpressOrderDetail.SendGoodsOperator,
                    aliExpressOrderProductDto => aliExpressOrderProductDto.MapFrom(x => x.SendGoodsOperator))
                .ForMember(aliExpressOrderDetail => aliExpressOrderDetail.ShowStatus,
                    aliExpressOrderProductDto => aliExpressOrderProductDto.MapFrom(x => x.ShowStatus))
                .ForMember(aliExpressOrderDetail => aliExpressOrderDetail.GoodsPrepareTime,
                    aliExpressOrderProductDto => aliExpressOrderProductDto.MapFrom(x => x.GoodsPrepareTime))
                .ForMember(aliExpressOrderDetail => aliExpressOrderDetail.TotalProductAmount,
                    aliExpressOrderProductDto => aliExpressOrderProductDto.MapFrom(x => x.TotalProductAmount));

            CreateMap<AliExpressOrderDTO, AliExpressOrder>()
                .ForMember(aliExpressOrder => aliExpressOrder.SellerSignerFullName,
                    aliExpressOrderDto => aliExpressOrderDto.MapFrom(x=>x.SellerSignerFullName))
                .ForMember(aliExpressOrder => aliExpressOrder.SellerLoginId,
                    aliExpressOrderDto => aliExpressOrderDto.MapFrom(x => x.SellerLoginId))
                .ForMember(aliExpressOrder => aliExpressOrder.OrderId,
                    aliExpressOrderDto => aliExpressOrderDto.MapFrom(x => x.OrderId))
                .ForMember(aliExpressOrder => aliExpressOrder.LogisticsStatus,
                    aliExpressOrderDto => aliExpressOrderDto.MapFrom(x => x.LogisticsStatus))
                .ForMember(aliExpressOrder => aliExpressOrder.BizType,
                    aliExpressOrderDto => aliExpressOrderDto.MapFrom(x => x.BizType))
                .ForMember(aliExpressOrder => aliExpressOrder.GmtPayTime,
                    aliExpressOrderDto => aliExpressOrderDto.MapFrom(x => x.GmtPayTime))
                .ForMember(aliExpressOrder => aliExpressOrder.EndReason,
                    aliExpressOrderDto => aliExpressOrderDto.MapFrom(x => x.EndReason))
                .ForMember(aliExpressOrder => aliExpressOrder.TotalProductCount,
                    aliExpressOrderDto => aliExpressOrderDto.MapFrom(x => x.AliExpressOrderProducts.Select(t => t.ProductCount).Aggregate((a, b) => a + b)))
                .ForMember(aliExpressOrder => aliExpressOrder.TotalPayAmount,
                    aliExpressOrderDto => aliExpressOrderDto.MapFrom(x => x.AliExpressOrderProducts.Select(t => t.TotalProductAmount).Aggregate((a, b) => a + b)))
                .ForMember(aliExpressOrder => aliExpressOrder.OrderStatus,
                    aliExpressOrderDto => aliExpressOrderDto.MapFrom(x => x.OrderStatus))
                .ForMember(aliExpressOrder => aliExpressOrder.GmtCreate,
                    aliExpressOrderDto => aliExpressOrderDto.MapFrom(x => x.GmtCreate))
                .ForMember(aliExpressOrder => aliExpressOrder.GmtUpdate,
                    aliExpressOrderDto => aliExpressOrderDto.MapFrom(x => x.GmtUpdate))
                .ForMember(aliExpressOrder => aliExpressOrder.FundStatus,
                    aliExpressOrderDto => aliExpressOrderDto.MapFrom(x => x.FundStatus))
                .ForMember(aliExpressOrder => aliExpressOrder.FrozenStatus,
                    aliExpressOrderDto => aliExpressOrderDto.MapFrom(x => x.FrozenStatus))

                .ForMember(aliExpressOrder => aliExpressOrder.AliExpressOrderDetails,
                    aliExpressOrderDto => aliExpressOrderDto.MapFrom(x => x.AliExpressOrderProducts));



        }
    }
}
