using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Top.Api;
using Top.Api.Request;
using YapartMarket.Core.BL;
using YapartMarket.Core.Config;
using YapartMarket.Core.Data.Interfaces.Azure;
using YapartMarket.Core.DTO;
using YapartMarket.Core.DTO.AliExpress;
using YapartMarket.Core.Extensions;
using Category = YapartMarket.Core.DTO.AliExpress.Category;

namespace YapartMarket.BL.Implementation
{
    public sealed class AliExpressCategoryService : IAliExpressCategoryService
    {
        private readonly IOptions<AliExpressOptions> _options;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAzureAliExpressProductRepository _aliExpressProductRepository;
        private readonly IAliExpressProductService _aliExpressProductService;
        private readonly IMapper _mapper;
        private readonly IOptions<Connections> _connections;
        private ITopClient _client;
        public AliExpressCategoryService(IMapper mapper,IOptions<Connections> connections, IOptions<AliExpressOptions> options, ICategoryRepository categoryRepository, IAzureAliExpressProductRepository aliExpressProductRepository, IAliExpressProductService aliExpressProductService)
        {
            _options = options;
            _categoryRepository = categoryRepository;
            _aliExpressProductRepository = aliExpressProductRepository;
            _aliExpressProductService = aliExpressProductService;
            _mapper = mapper;
            _connections = connections;
            _client = new DefaultTopClient(options.Value.HttpsEndPoint, options.Value.AppKey, options.Value.AppSecret, "Json");
        }

        public async Task<List<Category>> QueryCategoryAsync(long categoryId)
        {
            var categoryThreeRoot = new CategoryRoot();
            do
            {
                var request = new AliexpressCategoryRedefiningGetpostcategorybyidRequest() { Param0 = categoryId };
                try
                {
                    var rsp =  _client.Execute(request, _options.Value.AccessToken);
                    var body = rsp.Body;
                    if (body.TryParseJson(out CategoryRoot outCategoryThreeRoot))
                    {
                        categoryThreeRoot = outCategoryThreeRoot;
                        break;
                    }
                    if (body.TryParseJson(out AliExpressError error))
                        throw new Exception(error.AliExpressErrorMessage.Message + "/n" + error.AliExpressErrorMessage.SubCode);
                    if(body.TryParseJson(out CategoryThreeRootError categoryError))
                        break;

                }
                catch (WebException ex)
                {
                    if (ex.Status == WebExceptionStatus.Timeout)
                    {
                        await Task.Delay(3000);
                    }
                }
            } while (true);
            return categoryThreeRoot?.Result?.CategoryList?.PostCategoryList?.CategoryInfo;
        }

        private async Task UpdateCategoryById(long categoryId)
        {
            var queryGetCategories = await QueryCategoryAsync(categoryId);
            if (queryGetCategories == null)
                return;
            var categories = _mapper.Map<List<Category>, List<Core.Models.Azure.Category>>(queryGetCategories);
            await InsertCategoryAsync(categoryId, categories);
        }

        public async Task ProcessUpdateCategories()
        {
            var categoryIds = new List<long>();
            using (var connection = new SqlConnection(_connections.Value.SQLServerConnectionString))
            {
                await connection.OpenAsync();
                categoryIds = (await connection.QueryAsync<long>("select DISTINCT category_id from aliExpressProducts where category_id is not null")).ToList();
            }
            if (!categoryIds.IsAny())
                return;
            foreach (var categoryId in categoryIds)
            {
                await UpdateCategoryById(categoryId);
            }
        }

        public async Task UpdateCategoryByProductId(long productId)
        {
            var product = (await _aliExpressProductRepository.GetAsync("select * from aliExpressProducts where productId = @productId", new { productId = productId })).FirstOrDefault();
            if (product != null && product.CategoryId.HasValue)
            {
                var queryGetCategories = await QueryCategoryAsync(product.CategoryId.Value);
                if (queryGetCategories != null)
                {
                    var categories = _mapper.Map<List<Category>, List<Core.Models.Azure.Category>>(queryGetCategories);
                    await InsertCategoryAsync(product.CategoryId.Value, categories);
                }
            }
        }

        private async Task InsertCategoryAsync(long categoryId, List<Core.Models.Azure.Category> categories)
        {
            
            var categoryDb = await _categoryRepository.GetAsync("select * from ali_category where category_id = @category_id", new { category_id = categoryId});
            var newCategories = categories.Except(categoryDb);
            var modifiedCategories = categories.Where(x => categoryDb.Any(t =>
                t.CategoryId == x.CategoryId && t.IsLeaf!= x.IsLeaf &&
                t.CnName != x.CnName && t.EnName!= x.EnName && t.RuName != x.RuName));
            if (modifiedCategories.IsAny())
            {
                var updateCategoriesSql = new Category().UpdateString("dbo.ali_category", "category_id = @category_id");
                await _categoryRepository.Update(updateCategoriesSql, modifiedCategories.Select(x => new
                {
                    category_id = x.CategoryId,
                    is_leaf = x.IsLeaf,
                    level = x.Level,
                    ru_language_name = x.RuName,
                    en_language_name = x.EnName,
                    cn_language_name = x.CnName
                }));
            }
            if (!categoryDb.Any())
            {
                var insertString = new Core.Models.Azure.Category().InsertString("dbo.ali_category");
                await _categoryRepository.InsertAsync(insertString, newCategories.Select(x => new
                {
                    category_id = x.CategoryId,
                    is_leaf = x.IsLeaf,
                    level = x.Level,
                    ru_language_name = x.RuName,
                    en_language_name = x.EnName,
                    cn_language_name = x.CnName
                }));
            }
        }
    }
}
