using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Top.Api;
using Top.Api.Request;
using YapartMarket.Core.BL;
using YapartMarket.Core.Config;
using YapartMarket.Core.Data.Interfaces.Azure;
using YapartMarket.Core.DTO;
using YapartMarket.Core.DTO.AliExpress;
using YapartMarket.Core.Extensions;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.BL.Implementation
{
    public sealed class AliExpressCategoryService : IAliExpressCategoryService
    {
        private readonly IOptions<AliExpressOptions> _options;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAzureAliExpressProductRepository _aliExpressProductRepository;
        private readonly IAliExpressProductService _aliExpressProductService;
        private readonly IMapper _mapper;
        private ITopClient _client;
        public AliExpressCategoryService(IMapper mapper, IOptions<AliExpressOptions> options, ICategoryRepository categoryRepository, IAzureAliExpressProductRepository aliExpressProductRepository, IAliExpressProductService aliExpressProductService)
        {
            _options = options;
            _categoryRepository = categoryRepository;
            _aliExpressProductRepository = aliExpressProductRepository;
            _aliExpressProductService = aliExpressProductService;
            _mapper = mapper;
            _client = new DefaultTopClient(options.Value.HttpsEndPoint, options.Value.AppKey, options.Value.AppSecret, "Json");
        }

        private async Task<List<CategoryInfo>> QueryCategoryThreeAsync(long categoryId)
        {
            var repeat = true;
            var categoryThreeRoot = new CategoryThreeRoot();
            do
            {
                var request = new AliexpressSolutionSellerCategoryTreeQueryRequest
                {
                    CategoryId = categoryId,
                    FilterNoPermission = true
                };
                try
                {
                    var rsp =  _client.Execute(request, _options.Value.AccessToken);
                    var body = rsp.Body;
                    if (body.TryParseJson(out CategoryThreeRoot categoryThreeRootResult))
                    {
                        categoryThreeRoot = categoryThreeRootResult;
                        repeat = false;
                    }
                    if (body.TryParseJson(out AliExpressError error))
                        throw new Exception(error.AliExpressErrorMessage.Message + "/n" + error.AliExpressErrorMessage.SubCode);

                }
                catch (WebException ex)
                {
                    if (ex.Status == WebExceptionStatus.Timeout)
                    {
                        await Task.Delay(3000);
                    }
                }
                catch (Exception e)
                {
                    throw e;
                    break;
                }
            } while (repeat);
            return categoryThreeRoot?.Response?.ChildrenCategoryList?.CategoryInfo;
        }

        public async Task UpdateCategoryByProductId(long productId)
        {
            var product = (await _aliExpressProductRepository.GetAsync("select * from aliExpressProducts where productId = @productId", new {productId = productId})).FirstOrDefault();
            if (product != null && product.CategoryId.HasValue)
            {
                var queryGetCategories = await QueryCategoryThreeAsync(product.CategoryId.Value);
                if (queryGetCategories != null)
                {
                    var categories = _mapper.Map<List<CategoryInfo>, List<Category>>(queryGetCategories);
                    await InsertCategoryAsync(productId, categories);
                }
            }
        }

        public async Task ProcessUpdateCategories()
        {
            var products = await _aliExpressProductRepository.GetAsync("select * from dbo.aliExpressProducts where category_id is not null");
            if (!products.IsAny())
                await Task.CompletedTask;
            foreach (var product in products)
            {
                await UpdateCategoryByProductId(product.CategoryId.GetValueOrDefault());
            }
        }

        private async Task InsertCategoryAsync(long categoryId, List<Category> categories)
        {
            
            var categoryDb = await _categoryRepository.GetInAsync("category_id", new { category_id = categoryId});
            var newCategories = categories.Except(categoryDb);
            if (newCategories.Any())
            {
                var insertString = new Category().InsertString("dbo.ali_category");
                await _categoryRepository.InsertAsync(insertString, newCategories.Select(x => new
                {
                    category_id = x.CategoryId,
                    children_category_id = x.ChildrenCategoryId,
                    is_leaf_category = x.LeafCategory,
                    level = x.Level,
                    ru_language_name = x.RuName,
                    en_language_name = x.EnName,
                    cn_language_name = x.CnName
                }));
            }
        }
    }
}
