using System.IO;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace YapartMarket.React
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMvc(option => option.EnableEndpointRouting = false).AddJsonOptions(opt => opt.JsonSerializerOptions.PropertyNamingPolicy = null );

            services.AddAutoMapper(typeof(Startup));
            services.AddControllers();

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
        }
    }
}
