using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using YapartMarket.Core.Models;
using YapartMarket.React.ViewModels;

namespace YapartMarket.React.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IMapper _mapper;

        //public async Task<IActionResult> Products()
        //{
        //    return Ok(await )
        //}
        public ProductController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet]
        [Route("Products")]
        public IEnumerable<ProductViewModel> Products()
        {
            var list = new List<Product>()
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
            return _mapper.Map<List<ProductViewModel>>(list);
        }
    }
}