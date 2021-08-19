using System;
using System.IO;
using System.Text.Json;
using AutoMapper;
using Coravel;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using YapartMarket.BL.Implementation;
using YapartMarket.Core.BL;
using YapartMarket.Core.Config;
using YapartMarket.Core.Data;
using YapartMarket.Data;
using YapartMarket.Data.Implementation;
using YapartMarket.React.Invocables;
using YapartMarket.React.Options;


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
            services.AddMvc(option => option.EnableEndpointRouting = false).AddJsonOptions(opt => opt.JsonSerializerOptions.PropertyNamingPolicy = null);

            services.AddAutoMapper(typeof(Startup));
            services.AddControllers();

            //Регистрация сервисов
            #region Регистраци сервисов

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
            #endregion

            services.AddScheduler();

            services.AddMediatR(typeof(Startup));

            services.Configure<AliExpressOptions>(Configuration.GetSection(AliExpressOptions.AliExpress));
            services.ConfigureWritable<AliExpressOptions>(Configuration.GetSection(AliExpressOptions.AliExpress));
            
            //TODO вот это оставить под вопросом надо ли вообще!!
            //TODO Перелодить позже в папку другую!!
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
                    //Запускает сборку React приложения
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.ApplicationServices.UseScheduler(scheduler =>
            {
                scheduler.OnWorker("UpdateInventoryProductInAliExpress");

                scheduler.OnWorker("UpdateProductIdFromAliExpress");
                scheduler.Schedule<UpdateProductIdFromAliExpress>().DailyAt(20, 00);
            });
        }
    }
}
