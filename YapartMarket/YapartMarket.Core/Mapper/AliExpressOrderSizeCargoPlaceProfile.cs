﻿using AutoMapper;
using YapartMarket.Core.DTO;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Core.Mapper
{
    public class AliExpressOrderSizeCargoPlaceProfile : Profile
    {
        public AliExpressOrderSizeCargoPlaceProfile()
        {
            CreateMap<AliExpressOrderSizeCargoPlaceDTO, AliExpressExpressOrderSizeCargoPlace>();
        }
    }
}
