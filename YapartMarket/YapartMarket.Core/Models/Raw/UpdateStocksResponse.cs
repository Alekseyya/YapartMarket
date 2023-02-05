using System.Collections.Generic;

namespace YapartMarket.Core.Models.Raw
{
    public class Errors
    {
    }

    public class Result
    {
        public bool ok { get; set; }
        public string task_id { get; set; }
        public Errors errors { get; set; }
    }

    public class UpdateStocksResponse
    {
        public string group_id { get; set; }
        public List<Result> results { get; set; }
    }
}
