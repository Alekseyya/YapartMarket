using System.Collections.Generic;
using Newtonsoft.Json;

namespace YapartMarket.Core.DTO
{
    public class AliExpressBatchProductInventoryUpdateResponseDTO
    {
        [JsonProperty("aliexpress_solution_batch_product_inventory_update_response")]
        public AliExpressSolutionBatchProductInventoryUpdateResponseDTO InventoryUpdate { get; set; }
        [JsonProperty("update_successful_list")]
        public UpdateSuccessfulList UpdateSuccessfulList { get; set; }
    }

    public class UpdateSuccessfulList
    {
        [JsonProperty("synchronize_product_response_dto")]
        public SynchronizeProductResponse SynchronizeProductResponse { get; set; }
    }

    public class AliExpressSolutionBatchProductInventoryUpdateResponseDTO
    {
        [JsonProperty("update_error_code")]
        public string UpdateErrorCode { get; set; }
        [JsonProperty("update_error_message")]
        public string UpdateErrorMessage { get; set; }
        [JsonProperty("update_success")]
        public string UpdateSuccess { get; set; }
        [JsonProperty("update_failed_list")]
        public AliExpressUpdateFailedList UpdateFailedList { get; set; }
    }

    public class AliExpressUpdateFailedList
    {
        [JsonProperty("synchronize_product_response_dto")]
        public List<SynchronizeProductResponse> SynchronizeProductResponses { get; set; }
    }

    public class SynchronizeProductResponse
    {
        public List<ProductResponse> ProductResponses { get; set; }
    }

    public class ProductResponse
    {
        [JsonProperty("error_code")]
        public string ErrorCode { get; set; }
        [JsonProperty("error_message")]
        public string ErrorMessage { get; set; }
        [JsonProperty("product_id")]
        public long ProductId { get; set; }
    }
}
