using Newtonsoft.Json;
using Xunit;
using YapartMarket.Core.DTO;

namespace YapartMarket.UnitTests.YapartMarker.Core
{
    public class TestDeserializeToDTO
    {
        [Fact]
        public void TestDeserializeDTO_AliExpressBatchProductInventoryUpdateResponseDTO()
        {
            //act
            var json = @"{
    ""aliexpress_solution_batch_product_inventory_update_response"":{
        ""update_error_code"":"""",
        ""update_error_message"":"""",
        ""update_success"": true,
        ""update_failed_list"":{
            ""synchronize_product_response_dto"":[
                {
                    ""error_code"":"""",
                    ""error_message"":"""",
                    ""product_id"": 1
                }
            ]
        },
        ""update_successful_list"":{
            ""synchronize_product_response_dto"":[
                {
                    ""product_id"": 1
                }
            ]
        }
    }
}";
            //arrange
            var aliExpressBatchProductInventoryUpdateResponseDto = JsonConvert.DeserializeObject<AliExpressBatchProductInventoryUpdateResponseDTO>(json);
            //assert
            Assert.NotNull(aliExpressBatchProductInventoryUpdateResponseDto);
        }
    }
}
