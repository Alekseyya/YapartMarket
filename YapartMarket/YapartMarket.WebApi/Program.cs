using YapartMarket.BL.Implementation;
using YapartMarket.Core.BL;
using YapartMarket.Core.Models.Azure;
using YapartMarket.WebApi.Mapper.AliExpress;
using YapartMarket.Data;
using YapartMarket.Core.Data.Interfaces.Azure;
using YapartMarket.Data.Implementation.Azure;
using YapartMarket.Core.Config;
using YapartMarket.Core;
using Microsoft.EntityFrameworkCore;
using YapartMarket.WebApi.Services;
using YapartMarket.WebApi.Services.Interfaces;
using Microsoft.OpenApi.Models;
using Quartz;
using YapartMarket.WebApi.Job;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<DbContextOptions<YapartContext>>(provider =>
{
    var ob = new DbContextOptionsBuilder<YapartContext>();
    ob.UseNpgsql(builder.Configuration["ConnectionStrings:PostgresConnectionString"])
        /*.UseSnakeCaseNamingConvention()*/;
    //ob.UseLoggerFactory(_loggerFactory);
    return ob.Options;
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ToDo API",
        Description = "An ASP.NET Core Web API for managing ToDo items",
    });
});

builder.Services.AddAutoMapper(typeof(OrderProfile));
builder.Services.AddTransient<IAliExpressOrderService, AliExpressOrderService>();
builder.Services.AddTransient<IAliExpressProductService, AliExpressProductService>();

builder.Services.AddTransient<IAzureProductRepository>(m => new AzureProductRepository("products", builder.Configuration.GetConnectionString("SQLServerConnectionString")));
builder.Services.AddTransient<IProductPropertyRepository>(m => new ProductPropertiesRepository("dbo.ali_product_properties", builder.Configuration.GetConnectionString("SQLServerConnectionString")));
builder.Services.AddTransient<IAzureAliExpressProductRepository>(m => new AzureAliExpressProductRepository("aliExpressProducts", builder.Configuration.GetConnectionString("SQLServerConnectionString")));
builder.Services.AddTransient<IAliExpressOrderRepository>(m => new AliExpressOrderRepository(new Logger<AliExpressOrderRepository>(new LoggerFactory()), "dbo.orders", builder.Configuration.GetConnectionString("SQLServerConnectionString")));
builder.Services.AddTransient<IAliExpressOrderDetailRepository>(m => new AliExpressOrderDetailRepository("dbo.order_details", builder.Configuration.GetConnectionString("SQLServerConnectionString")));
builder.Services.Configure<AliExpressOptions>(builder.Configuration.GetSection(AliExpressOptions.AliExpress));

builder.Services.AddTransient<IGoodsService, GoodsService>();

builder.Services.AddSingleton(typeof(Deserializer<IReadOnlyList<AliExpressOrder>>), s => new OrderDeserializer());
builder.Services.AddHttpClient("goodsClient", c => c.BaseAddress = new Uri("https://partner.sbermegamarket.ru"));

builder.Services.AddHttpClient("aliExpress", c => c.BaseAddress = new Uri(builder.Configuration["AliExpress:Url"]));

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionScopedJobFactory();
    var jobKey = new JobKey("DemoJob");
    q.AddJob<UpdateInventoryJon>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("DemoJob-trigger")
        .WithCronSchedule("0 */4 * * * ?"));

});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(option => option.SwaggerEndpoint("/swagger/v1/swagger.json", "YapartStore v1"));

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
