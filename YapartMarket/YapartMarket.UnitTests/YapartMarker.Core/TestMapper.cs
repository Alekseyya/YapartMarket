using System.Linq;
using AutoMapper;
using Newtonsoft.Json;
using Xunit;
using YapartMarket.Core.DTO.AliExpress;
using YapartMarket.Core.Mapper;
using YapartMarket.Core.Models.Azure;
using Category = YapartMarket.Core.DTO.AliExpress.Category;

namespace YapartMarket.UnitTests.YapartMarker.Core
{
    public class TestMapper
    {
        private IMapper _mapper;
        public TestMapper()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AliExpressOrderProfile());
                cfg.AddProfile(new AliExpressOrderLogisticProfile());
                cfg.AddProfile(new AliCategoryProfile());
            });
            _mapper = mockMapper.CreateMapper();
        }
        [Fact]
        public void AliCategoryProfile_Call_ReturnSuccess()
        {
            //Arrange
            var json =
                "{\r\n    \"aliexpress_category_redefining_getpostcategorybyid_response\": {\r\n        \"result\": {\r\n            \"aeop_post_category_list\": {\r\n                \"aeop_post_category_dto\": [\r\n                    {\r\n                        \"features\": \"{}\",\r\n                        \"id\": 200095145,\r\n                        \"isleaf\": true,\r\n                        \"level\": 4,\r\n                        \"names\": \"{\\\"de\\\":\\\"Block & Teile\\\",\\\"hi\\\":\\\"ब्लॉक और भागों\\\",\\\"ru\\\":\\\"Блоки и детали\\\",\\\"ko\\\":\\\"블록 및 부품\\\",\\\"pt\\\":\\\"Bloco e peças\\\",\\\"in\\\":\\\"Blok & Bagian\\\",\\\"en\\\":\\\"Block & Parts\\\",\\\"it\\\":\\\"Blocco & parti\\\",\\\"fr\\\":\\\"Blocs et pièces\\\",\\\"es\\\":\\\"Bloque y piezas\\\",\\\"iw\\\":\\\"בלוק & חלקים\\\",\\\"zh\\\":\\\"缸体及零件\\\",\\\"ar\\\":\\\"كتلة و أجزاء\\\",\\\"vi\\\":\\\"khối & Phụ Tùng\\\",\\\"th\\\":\\\"บล็อกและชิ้นส่วน\\\",\\\"ja\\\":\\\"ブロック&パーツ\\\",\\\"nl\\\":\\\"blok & Onderdelen\\\",\\\"tr\\\":\\\"Blok ve Parçaları\\\"}\"\r\n                    }\r\n                ]\r\n            },\r\n            \"success\": true\r\n        },\r\n        \"request_id\": \"15raj3e9db2u0\"\r\n    }\r\n}";
            var categoryInfo = JsonConvert.DeserializeObject<CategoryRoot>(json);
            var category = categoryInfo.Result.CategoryList.PostCategoryList.CategoryInfo.First();
            var languages = JsonConvert.DeserializeObject<LanguageNames>(category.MultilanguageName);
            //Act
            var result = _mapper.Map<Category, global::YapartMarket.Core.Models.Azure.Category>(category);
            //Assert
            Assert.Equal(result.CategoryId, category.Id);
            Assert.Equal(result.IsLeaf, category.IsLeaf);
            Assert.Equal(result.Level, category.Level);
            Assert.Equal(result.RuName, languages.Ru);
            Assert.Equal(result.EnName, languages.En);
        }
    }
}
