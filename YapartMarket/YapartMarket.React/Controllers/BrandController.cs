using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using YapartMarket.Core.BL;
using YapartMarket.Core.Models;
using YapartMarket.React.Queries.Brand;
using YapartMarket.React.Queries.Brands;
using YapartMarket.React.ViewModels;

namespace YapartMarket.React.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrandController : ControllerBase
    {
        private readonly ILogger<BrandController> _logger;
        private readonly IBrandService _brandService;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public BrandController(IBrandService brandService, IMapper mapper, IMediator mediator)
        {
            _brandService = brandService;
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBrand(int id)
        {
            var query = new GetBrandByIdQuery(id);
            var result = await _mediator.Send(query);
            return result != null ? (IActionResult)Ok(result) : NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetBrands()
        {
            var query = new GetAllBrandsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
            
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateBrand([FromBody] BrandViewModel request)
        {
            var brand = await _brandService.CreateBrandAsync(_mapper.Map<Brand>(request));
            _logger.LogInformation($"В БД создана запись Brand");
            return CreatedAtAction("CreateBrand", new {brandId = brand.Id});
        }
    }
}
