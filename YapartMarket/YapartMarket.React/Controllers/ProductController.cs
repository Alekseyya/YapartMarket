using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
        private readonly List<Product> _products;
        //IAccessProductRepository accessProductRepository,
        public ProductController( IMapper mapper)
        {
            //_accessProductRepository = accessProductRepository;
            _mapper = mapper;
            _products = new List<Product>()
            {
                new Product()
                {
                    Id = 1, Article = "No-Po-1",
                    Descriptions = "First",
                    Brand = new Brand(){Name = "Norplast", Picture = new Picture(){Path ="\\No-Po-1.png" }},
                    Price = (decimal) 15.6
                },
                new Product()
                {
                    Id = 2, Article = "NO-1231", Descriptions = "Second",
                    Brand = new Brand(){Name = "Norplast", Picture = new Picture(){Path = "\\NO-1231.png"}},
                    Price = (decimal) 13.2
                }
            };
        }

        //[HttpGet]
        //[Route("GetProducts")]
        //public async Task<IActionResult> GetProducts()
        //{
        //    var products = await _accessProductRepository.GetAsync("select * from Tovari");
        //    return Ok(products);

        //}

        [HttpGet]
        [Route("ListProducts")]
        public IActionResult ListProducts()
        {
            return Ok(_mapper.Map<List<ProductViewModel>>(_products));
        }

        [HttpGet]
        public ProductViewModel GetProductById(int id)
        {
            return _mapper.Map<ProductViewModel>(_products.FirstOrDefault(x => x.Id == id));
        }
    }
}