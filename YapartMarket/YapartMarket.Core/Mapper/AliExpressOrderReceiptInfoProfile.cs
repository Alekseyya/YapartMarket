using AutoMapper;
using YapartMarket.Core.DTO;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Core.Mapper
{
    public class AliExpressOrderReceiptInfoProfile : Profile
    {
        public AliExpressOrderReceiptInfoProfile()
        {
            CreateMap<AliExpressOrderReceiptInfoDTO, AliExpressOrderReceiptInfo>()
                .ForMember(orderRec => orderRec.CountryName,
                    root => root.MapFrom(x => x.CountryName))
                .ForMember(orderRec => orderRec.Mobile,
                    root => root.MapFrom(x => x.Mobile))
                .ForMember(orderRec => orderRec.ContractPerson,
                    root => root.MapFrom(
                        x => x.ContractPerson))
                .ForMember(orderRec => orderRec.PhoneCountry,
                    root => root.MapFrom(x => x.PhoneCountry))
                .ForMember(orderRec => orderRec.PhoneArea,
                    root => root.MapFrom(x => x.PhoneArea))
                .ForMember(orderRec => orderRec.Province,
                    root => root.MapFrom(x => x.Province))
                .ForMember(orderRec => orderRec.Address,
                    root => root.MapFrom(x => x.Address))
                .ForMember(orderRec => orderRec.PhoneNumber,
                    root => root.MapFrom(x => x.PhoneNumber))
                .ForMember(orderRec => orderRec.FaxNumber,
                    root => root.MapFrom(x => x.FaxNumber))
                .ForMember(orderRec => orderRec.StreetDetailedAddress,
                    root => root.MapFrom(x =>
                        x.DetailAddress))
                .ForMember(orderRec => orderRec.City,
                    root => root.MapFrom(x => x.City))
                .ForMember(orderRec => orderRec.Country,
                    root => root.MapFrom(x => x.Country))
                .ForMember(orderRec => orderRec.Address2,
                    root => root.MapFrom(x => x.Address2))
                .ForMember(orderRec => orderRec.FaxCountry,
                    root => root.MapFrom(x => x.FaxCountry))
                .ForMember(orderRec => orderRec.PostCode,
                    root => root.MapFrom(x => x.Zip))
                .ForMember(orderRec => orderRec.FaxArea,
                    root => root.MapFrom(x => x.FaxArea))
                .ForMember(orderRec => orderRec.LocalizedAddress,
                    root => root.MapFrom(x =>
                        x.LocalizedAddress));
        }
    }
}
