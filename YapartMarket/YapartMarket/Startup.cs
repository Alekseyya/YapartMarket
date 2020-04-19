using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YapartMarket.BL.Implementation;
using YapartMarket.Core.BL;
using YapartMarket.Core.Data;
using YapartMarket.Data;
using YapartMarket.Data.Implementation;

namespace YapartMarket.MainApp
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<DbContextOptions<YapartContext>>(provider =>
            {
                var ob = new DbContextOptionsBuilder<YapartContext>();
                var stringConnection = Configuration["Connection:DbConnectionString"]; 
                ob.UseNpgsql(Configuration["Connection:DbConnectionString"]);
                return ob.Options;
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //Todo httpAccessor singleton
            services.AddHttpContextAccessor();

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


            #endregion


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

            app.UseStatusCodePages(async context =>
            {
                context.HttpContext.Response.ContentType = "text/plain";

                await context.HttpContext.Response.WriteAsync(
                    "Status code page, status code: " +
                    context.HttpContext.Response.StatusCode);
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
