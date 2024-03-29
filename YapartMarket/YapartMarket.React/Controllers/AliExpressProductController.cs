﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using YapartMarket.Core.BL;
using YapartMarket.Core.Config;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.React.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AliExpressProductController : Controller
    {
        private readonly IAliExpressProductService _aliExpressProductService;
        private readonly IAliExpressCategoryService _aliExpressCategoryService;
        private readonly IAliExpressTokenService _aliExpressTokenService;
        private readonly AliExpressOptions _option;

        public AliExpressProductController(IOptions<AliExpressOptions> option, IAliExpressProductService aliExpressProductService, IAliExpressCategoryService aliExpressCategoryService,  IAliExpressTokenService aliExpressTokenService)
        {
            _aliExpressProductService = aliExpressProductService;
            _aliExpressCategoryService = aliExpressCategoryService;
            _aliExpressTokenService = aliExpressTokenService;
            _option = option.Value;
        }


        [HttpPost]
        [Route("processUpdateExpressId")]
        [Produces("application/json")]
        public async Task<IActionResult> ProcessUpdateExpressId()
        {
            try
            {
                await _aliExpressProductService.ProcessUpdateProductsSku();
                await _aliExpressProductService.ProcessUpdateDatabaseAliExpressProductId();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Route("productsExcept")]
        [Produces("application/json")]
        public async Task<IActionResult> GetProductsExceptFromDatabase()
        {
            try
            {
                var products = _aliExpressProductService.GetProductsAliExpress();
                await _aliExpressProductService.ExceptProductsFromDataBase(products);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("productInfo")]
        [Produces("application/json")]
        public IActionResult GetProductInfo(long productId)
        {
            try
            {
                if (productId == 0)
                    return BadRequest("Не указан productId");
                var productInfo =_aliExpressProductService.GetProductInfo(productId);
                return Ok(productInfo);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("updateProductAndCategory")]
        [Produces("application/json")]
        public async Task<IActionResult> UpdateProductAndCategory()
        {
            try
            {
                //await _aliExpressProductService.ProcessUpdateProduct();
                //await _aliExpressCategoryService.ProcessUpdateCategories();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message + "/n" + e.StackTrace);
            }
        }

        [HttpPut]
        [Route("updateProductInventory")]
        [Produces("application/json")]
        public IActionResult UpdateProductInventory(long productId, string sku, int inventory)
        {
            try
            {
                if (productId == 0)
                    return BadRequest("Не указан productId");
                if(string.IsNullOrEmpty(sku))
                    return BadRequest("Не указан sku");

                var products = new List<Product>
                {
                    new()
                    {
                        Sku = sku,
                        Count = inventory,
                        AliExpressProductId = productId
                    }
                };
                _aliExpressProductService.UpdateInventoryProducts(products);
                var productInfo = _aliExpressProductService.GetProductInfo(productId);
                return Ok(productInfo);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPatch]
        [Route("updateProductsInventory")]
        [Produces("application/json")]
        public async Task<IActionResult> UpdateProductsInventory()
        {
            try
            {
               var products = await _aliExpressProductService.ListProductsForUpdateInventory();
               await _aliExpressProductService.UpdateInventoryProducts(products);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
