using System.Linq;
using AutoMapper;
using Newtonsoft.Json;
using Xunit;
using YapartMarket.Core.DTO.AliExpress;
using YapartMarket.Core.Mapper;
using YapartMarket.Core.Models.Azure;

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
                "{\r\n    \"aliexpress_solution_seller_category_tree_query_response\":{\r\n        \"children_category_list\":{\r\n            \"category_info\":[\r\n                {\r\n                    \"children_category_id\":5090301,\r\n                    \"is_leaf_category\":true,\r\n                    \"level\":2,\r\n                    \"multi_language_names\":\"{   \\\"de\\\": \\\"Mobiltelefon\\\",   \\\"ru\\\": \\\"Мобильные телефоны\\\",   \\\"pt\\\": \\\"Telefonia\\\",   \\\"in\\\": \\\"Ponsel\\\",   \\\"en\\\": \\\"Mobile Phones\\\",   \\\"it\\\": \\\"Telefoni cellulari\\\",   \\\"fr\\\": \\\"Smartphones\\\",   \\\"es\\\": \\\"Smartphones\\\",   \\\"tr\\\": \\\"Cep Telefonu\\\",   \\\"nl\\\": \\\"Mobiele telefoons\\\" }\"\r\n                }\r\n            ]\r\n        },\r\n        \"is_success\":true\r\n    }\r\n}";
            var categoryInfo = JsonConvert.DeserializeObject<CategoryThreeRoot>(json).Response.ChildrenCategoryList.CategoryInfo.First();
            var languages = JsonConvert.DeserializeObject<LanguageNames>(categoryInfo.MultilanguageName);
            //Act
            var result = _mapper.Map<CategoryInfo, Category>(categoryInfo);
            //Assert
            Assert.Equal(result.ChildrenCategoryId, categoryInfo.ChildrenCategoryId);
            Assert.Equal(result.LeafCategory,  categoryInfo.LeafCategory);
            Assert.Equal(result.Level, categoryInfo.Level);
            Assert.Equal(result.RuName, languages.Ru);
            Assert.Equal(result.EnName, languages.En);
        }
    }
}
