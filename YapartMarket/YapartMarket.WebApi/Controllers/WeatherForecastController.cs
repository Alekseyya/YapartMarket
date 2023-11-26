using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using YapartMarket.WebApi.Services.Interfaces;

namespace YapartMarket.WebApi.Controllers
{
    /// <summary>
    /// Wheather
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private IGoodsService _goodsService;
        private readonly ILogger<WeatherForecastController> _logger;

        /// <summary>
        /// as
        /// </summary>
        /// <param name="goodsService"></param>
        /// <param name="logger"></param>
        public WeatherForecastController(IGoodsService goodsService, ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            _goodsService = goodsService;
        }
        /// <summary>
        /// GetWeatherForecast
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetWeatherForecast")]
        public IActionResult Get()
        {
            try
            {
                var a = Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                });
                return Ok(a);
            }
            catch (Exception e)
            {

               return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}