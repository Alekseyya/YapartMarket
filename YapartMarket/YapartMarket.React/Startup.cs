﻿using System;
using System.IO;
using Coravel;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using YapartMarket.BL.Implementation;
using YapartMarket.BL.Implementation.AliExpress;
using YapartMarket.Core.BL;
using YapartMarket.Core.BL.AliExpress;
using YapartMarket.Core.Config;
using YapartMarket.Core.Data;
using YapartMarket.Core.Data.Interfaces.Azure;
using YapartMarket.Core.Mapper;
using YapartMarket.Data;
using YapartMarket.Data.Implementation;
using YapartMarket.Data.Implementation.Azure;
using YapartMarket.React.Invocables;
using YapartMarket.React.Options;
using YapartMarket.React.Services;
using YapartMarket.React.Services.Interfaces;
using CategoryRepository = YapartMarket.Data.Implementation.Azure.CategoryRepository;

namespace YapartMarket.React
{
    public class Startup
    {
        //private readonly ILoggerFactory _loggerFactory;

        public Startup(IConfiguration configuration)
        {
            //_loggerFactory = loggerFactory;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<DbContextOptions<YapartContext>>(provider =>
            {
                var ob = new DbContextOptionsBuilder<YapartContext>();
                ob.UseNpgsql(Configuration["ConnectionStrings:PostgresConnectionString"])
                    /*.UseSnakeCaseNamingConvention()*/;
                //ob.UseLoggerFactory(_loggerFactory);
                return ob.Options;
            });

            services.AddControllers();
            services.AddMvc(option => option.EnableEndpointRouting = false).AddJsonOptions(opt => 
            { 
                opt.JsonSerializerOptions.PropertyNamingPolicy = null;
                opt.JsonSerializerOptions.IgnoreNullValues = true;
            });

            //services.AddAutoMapper(typeof(Startup));
            services.AddAutoMapper(typeof(Startup), typeof(AliExpressOrderProfile));
            services.AddControllers();

            //Ðåãèñòðàöèÿ ñåðâèñîâ
            #region Ðåãèñòðàöè ñåðâèñîâ

            services.AddScoped<IYapartDbAccessor, YapartSingletonDbAccessor>();
            services.AddScoped<IRepositoryFactory, YapartRepositoryFactory>();
            services.AddScoped<IUnitOfWork, YapartUnitOfWork>();

            services.AddTransient<IBrandService, BrandService>();
            services.AddTransient<ICartLineService, CartLineService>();
            services.AddTransient<ICartService, CartService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IGroupService, GroupService>();
            services.AddTransient<IMarkService, MarkService>();
            services.AddTransient<IModelService, ModelService>();
            services.AddTransient<IModificationService, ModificationService>();
            services.AddTransient<IOrderItemService, OrderItemService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IPictureService, PictureService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IProductModificationService, ProductModificationService>();
            services.AddTransient<ISectionService, SectionService>();
            services.AddTransient<IAliExpressTokenService, AliExpressTokenService>();
            services.AddTransient<IAliExpressProductService, AliExpressProductService>();
            services.AddTransient<IAliExpressOrderService, AliExpressOrderService>();
            services.AddTransient<IAliExpressOrderDetailService, AliExpressOrderDetailService>();
            services.AddTransient<IAliExpressOrderReceiptInfoService, AliExpressOrderReceiptInfoService>();
            services.AddTransient<IAliExpressLogisticRedefiningService, AliExpressLogisticRedefiningService>();
            services.AddTransient<IAliExpressLogisticOrderDetailService, AliExpressLogisticOrderDetailService>();
            services.AddTransient<IAliExpressOrderFullfilService, AliExpressOrderFullfilService>();
            services.AddTransient<ILogisticServiceOrderService, LogisticServiceOrderService>();
            services.AddTransient<IAliExpressCategoryService, AliExpressCategoryService>();
            services.AddTransient<ILogisticWarehouseOrderService, AliExpressCreateLogisticWarehouseOrderService>();
            services.AddTransient<IRedefiningDomesticLogisticsCompany, RedefiningDomesticLogisticsCompanyService>();
            services.AddTransient<IWarehouseDetailService, WarehouseDetailService>();
            services.AddTransient<IOrderSizeCargoPlaceService, OrderSizeCargoPlaceService>();
            services.AddTransient<IFullOrderInfoService, FullOrderInfoService>();
            services.AddTransient<IAttributeService, AttributeService>();
            services.AddTransient<ICategoryTreeService, CategoryTreeService>();

            #endregion

            

            services.AddTransient<IAzureProductRepository>(m => new AzureProductRepository("products", Configuration.GetConnectionString("SQLServerConnectionString")));
            services.AddTransient<IAzureAliExpressProductRepository>(m => new AzureAliExpressProductRepository("aliExpressProducts", Configuration.GetConnectionString("SQLServerConnectionString")));
            services.AddTransient<IAliExpressOrderRepository>(m => new AliExpressOrderRepository(new Logger<AliExpressOrderRepository>(new LoggerFactory()),"dbo.orders", Configuration.GetConnectionString("SQLServerConnectionString")));
            services.AddTransient<IAliExpressOrderDetailRepository>(m => new AliExpressOrderDetailRepository("dbo.order_details", Configuration.GetConnectionString("SQLServerConnectionString")));
            services.AddTransient<IAzureAliExpressOrderReceiptInfoRepository>(m => new AzureAliExpressOrderReceiptInfoRepository(new Logger<AzureAliExpressOrderReceiptInfoRepository>(new LoggerFactory()), "dbo.order_receipt_infos", Configuration.GetConnectionString("SQLServerConnectionString")));
            services.AddTransient<IAzureAliExpressOrderLogisticRedefiningRepository>(m => new AzureAliExpressOrderLogisticRedefiningRepository("dbo.order_redefining", Configuration.GetConnectionString("SQLServerConnectionString")));
            services.AddTransient<IAliExpressOrderSizeCargoPlaceRepository>(m => new AliExpressOrderSizeCargoPlaceRepository("dbo.order_size_cargo_places", Configuration.GetConnectionString("SQLServerConnectionString")));
            services.AddTransient<IAliExpressLogisticOrderDetailRepository>(m => new AliExpressLogisticOrderDetailRepository("dbo.logistic_order_detail", Configuration.GetConnectionString("SQLServerConnectionString")));
            services.AddTransient<ILogisticServiceOrderRepository>(m => new LogisticServiceOrderRepository("dbo.logistic_service_order", Configuration.GetConnectionString("SQLServerConnectionString")));
            services.AddTransient<IProductPropertyRepository>(m => new ProductPropertiesRepository("dbo.ali_product_properties", Configuration.GetConnectionString("SQLServerConnectionString")));
            services.AddTransient<ICategoryRepository>(m => new CategoryRepository("dbo.ali_category", Configuration.GetConnectionString("SQLServerConnectionString")));

            services.AddHttpClient("goodsClient", c => c.BaseAddress = new Uri("https://partner.goodsteam.tech"));
            services.AddHttpClient("aliClient", c => c.BaseAddress = new Uri("https://eco.taobao.com/router/rest"));
            services.AddTransient<IGoodsService, GoodsService>();

            services.AddScheduler();

            services.AddTransient<UpdateInventoryAliExpressInvocable>();
            services.AddTransient<UpdateProductIdFromAliExpressInvocable>();
            services.AddTransient<DoSomethingInvocable>();
            services.AddTransient<UpdateOrdersFromAliExpressInvocable>();
            services.AddTransient<UpdateLogisticRedefiningInvocable>();

            services.AddMediatR(typeof(Startup));
            //CustomMapper для Dapper
            TypeMapper.Initialize("YapartMarket.Core.Models.Azure");
            //Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            services.Configure<AliExpressOptions>(Configuration.GetSection(AliExpressOptions.AliExpress));
            services.ConfigureWritable<AliExpressOptions>(Configuration.GetSection(AliExpressOptions.AliExpress));
            services.Configure<Connections>(Configuration.GetSection(Connections.Section));
            services.ConfigureWritable<Connections>(Configuration.GetSection(Connections.Section));

            //DefaultTopLogger.FilePath = "c:/tmp/topsdk.log";

            //TODO âîò ýòî îñòàâèòü ïîä âîïðîñîì íàäî ëè âîîáùå!!
            //TODO Ïåðåëîäèòü ïîçæå â ïàïêó äðóãóþ!!
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "yapartclient/build";
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseMvc();

            app.UseRouting();

            app.UseAuthorization();
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = Path.Join(env.ContentRootPath, "yapartclient");

                if (env.IsDevelopment())
                {
                    //Çàïóñêàåò ñáîðêó React ïðèëîæåíèÿ
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.ApplicationServices.UseScheduler(scheduler =>
            {
                //scheduler.Schedule(() =>
                //{
                //    Console.Write("Doing expensive calculation for 5 sec...");
                //    Console.Write("Expensive calculation done.");
                //}).EverySeconds(3);
                //scheduler.Schedule<DoSomethingInvocable>();

                //scheduler.OnWorker("EmailTasks");
                //scheduler.Schedule(() => Console.WriteLine("Hourly on Mondays.")).EverySeconds(3);

                //scheduler.OnWorker("UpdateOrdersFromAliExpress");
                //scheduler.Schedule<UpdateOrdersFromAliExpressInvocable>().EveryFiveMinutes();
                
                //scheduler.OnWorker("UpdateProductIdFromAliExpress");
                //scheduler.Schedule<UpdateProductIdFromAliExpressInvocable>().DailyAt(20, 00);

                //scheduler.OnWorker("UpdateInventoryProductInAliExpress");
                //scheduler.Schedule<UpdateInventoryAliExpressInvocable>().Hourly();

            }); 
        }
    }
}
