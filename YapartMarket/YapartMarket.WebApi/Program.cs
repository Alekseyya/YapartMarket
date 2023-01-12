using YapartMarket.BL.Implementation;
using YapartMarket.Core.BL;
using YapartMarket.Core.Models.Azure;
using YapartMarket.WebApi.Mapper.AliExpress;
using YapartMarket.Data;
using YapartMarket.Core.Data.Interfaces.Azure;
using YapartMarket.Data.Implementation.Azure;
using YapartMarket.Core.Config;
using YapartMarket.Core;
using YapartMarket.WebApi.Job;
using Microsoft.EntityFrameworkCore;
using YapartMarket.WebApi.Services;
using YapartMarket.WebApi.Services.Interfaces;
using Quartz;
using Dapper;
using YapartMarket.Core.DTO.Goods;
using System.ComponentModel.DataAnnotations.Schema;

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
builder.Services.AddSwaggerGen();

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
//Todo Add correct
SqlMapper.SetTypeMap(
    typeof(Order),
    new CustomPropertyTypeMap(
        typeof(Order),
        (type, columnName) =>
            type.GetProperties().FirstOrDefault(prop =>
                prop.GetCustomAttributes(false)
                    .OfType<ColumnAttribute>()
                    .Any(attr => attr.Name == columnName || prop.Name == columnName))));
SqlMapper.SetTypeMap(
    typeof(OrderItem),
    new CustomPropertyTypeMap(
        typeof(OrderItem),
        (type, columnName) =>
            type.GetProperties().FirstOrDefault(prop =>
                prop.GetCustomAttributes(false)
                    .OfType<ColumnAttribute>()
                    .Any(attr => attr.Name == columnName || prop.Name == columnName))));
SqlMapper.SetTypeMap(
    typeof(YapartMarket.Core.DTO.Goods.Product),
    new CustomPropertyTypeMap(
        typeof(OrderItem),
        (type, columnName) =>
            type.GetProperties().FirstOrDefault(prop =>
                prop.GetCustomAttributes(false)
                    .OfType<ColumnAttribute>()
                    .Any(attr => attr.Name == columnName || prop.Name == columnName))));

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
