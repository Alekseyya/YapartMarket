using AutoMapper;
using Newtonsoft.Json;
using YapartMarket.Core.DTO.AliExpress;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Core.Mapper
{
    public sealed class AliCategoryProfile : Profile
    {
        public AliCategoryProfile()
        {
            CreateMap<CategoryInfo, Category>()
                .ForMember(x=>x.ChildrenCategoryId, t=> t.MapFrom(y=>y.ChildrenCategoryId))
                .ForMember(x=>x.LeafCategory, t=>t.MapFrom(y=>y.LeafCategory))
                .ForMember(x=>x.Level, t=>t.MapFrom(y=>y.Level))
                .ForMember(x=>x.RuName, t=>t.MapFrom(y=> JsonConvert.DeserializeObject<LanguageNames>(y.MultilanguageName).Ru))
                .ForMember(x=>x.EnName, t=>t.MapFrom(y=> JsonConvert.DeserializeObject<LanguageNames>(y.MultilanguageName).En))
                .ForMember(x=>x.CnName, t=>t.MapFrom(y=> JsonConvert.DeserializeObject<LanguageNames>(y.MultilanguageName).Cn))
                ;
        }
    }
}
