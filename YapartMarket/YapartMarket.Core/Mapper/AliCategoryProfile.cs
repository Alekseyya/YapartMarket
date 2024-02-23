using AutoMapper;
using Newtonsoft.Json;
using YapartMarket.Core.DTO.AliExpress;
using Category = YapartMarket.Core.DTO.AliExpress.Category;

namespace YapartMarket.Core.Mapper
{
    public sealed class AliCategoryProfile : Profile
    {
        public AliCategoryProfile()
        {
            CreateMap<Category, Models.Azure.Category>()
                .ForMember(x=>x.CategoryId, t=> t.MapFrom(y=>y.Id))
                .ForMember(x=>x.IsLeaf, t=>t.MapFrom(y=>y.IsLeaf))
                .ForMember(x=>x.Level, t=>t.MapFrom(y=>y.Level))
                .ForMember(x=>x.RuName, t=>t.MapFrom(y=> JsonConvert.DeserializeObject<LanguageNames>(y.MultilanguageName)!.Ru))
                .ForMember(x=>x.EnName, t=>t.MapFrom(y=> JsonConvert.DeserializeObject<LanguageNames>(y.MultilanguageName)!.En))
                .ForMember(x=>x.CnName, t=>t.MapFrom(y=> JsonConvert.DeserializeObject<LanguageNames>(y.MultilanguageName)!.Cn));
        }
    }
}
