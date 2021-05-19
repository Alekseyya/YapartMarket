using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YapartMarket.Core.Data.Interfaces.Access;
using YapartMarket.Core.Models;
using YapartMarket.React.ViewModels;

namespace YapartMarket.React.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        //private readonly IAccessProductRepository _accessProductRepository;
        private readonly IMapper _mapper;
        private readonly List<YapartMarket.Core.Models.Product> _products;
        //IAccessProductRepository accessProductRepository,
        public ProductController(IMapper mapper)
        {
            //_accessProductRepository = accessProductRepository;
            _mapper = mapper;
            _products = new List<YapartMarket.Core.Models.Product>()
            {
                new YapartMarket.Core.Models.Product()
                {
                    Id = 1, Article = "No-Po-1",
                    Descriptions = "First",
                    Brand = new Brand(){Name = "Norplast", Picture = new Picture(){Path ="\\No-Po-1.png" }},
                    Price = (decimal) 15.6
                },
                new YapartMarket.Core.Models.Product()
                {
                    Id = 2, Article = "NO-1231", Descriptions = "Second",
                    Brand = new Brand(){Name = "Norplast", Picture = new Picture(){Path = "\\NO-1231.png"}},
                    Price = (decimal) 13.2
                }
            };
        }

        //[HttpGet]
        //[Route("GetProducts")]
        //public async Task<ActionResult> GetProducts()
        //{
        //    var products = await _accessProductRepository.GetAsync("select * from Tovari");
        //    return Ok(products);

        //}

        [HttpPost]
        [Route("stocks")]
        [Produces("application/json")]
        public async Task<IActionResult> Stocks([FromBody] Stock stock)
        {
            if (stock == null)
                return BadRequest();
            var listSkuInfo = new List<SkuInfo>();
            foreach (var sku in stock.Skus)
            {
                listSkuInfo.Add(new SkuInfo
                {
                    Sku = sku,
                    WarehouseId = stock.WarehouseId,
                    Items = new List<Product>
                        {
                            new Product {Type = nameof(ProductType.FIT), Count = 1, UpdatedAt = DateTimeOffset.Now.ToString("yyyy-MM-dd'T'HH:mm:ssK")}
                        }
                });
            }
            return Ok(listSkuInfo);
        }

        [HttpGet]
        [Route("ListProducts")]
        public ActionResult ListProducts()
        {
            return Ok(_mapper.Map<List<ProductViewModel>>(_products));
        }

        [HttpGet]
        public ProductViewModel GetProductById(int id)
        {
            return _mapper.Map<ProductViewModel>(_products.FirstOrDefault(x => x.Id == id));
        }
    }

    public class Stock
    {
        public Int64 WarehouseId { get; set; }
        public List<string> Skus { get; set; }
    }

    public class StocksSku
    {
        [JsonPropertyName("skus")]
        public List<SkuInfo> Skus { get; set; }
    }

    public class SkuInfo
    {
        [JsonPropertyName("sku")]
        public string Sku { get; set; }
        [JsonPropertyName("warehouseId")]
        public Int64 WarehouseId { get; set; }
        [JsonPropertyName("items")]
        public List<Product> Items { get; set; }
    }

    public class Product
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("count")]
        public Int64 Count { get; set; }
        [JsonPropertyName("updatedId")]
        public string UpdatedAt { get; set; }
    }

    public enum ProductType
    {
        FIT
    }
}