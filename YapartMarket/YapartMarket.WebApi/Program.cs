using System;
using System.Threading;
using System.Collections.Generic;
using Quartz;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YapartMarket.Core;
using YapartMarket.Data;
using YapartMarket.WebApi;
using YapartMarket.Core.BL;
using YapartMarket.Core.DTO;
using YapartMarket.WebApi.Job;
using YapartMarket.Core.Config;
using YapartMarket.WebApi.Services;
using YapartMarket.BL.Implementation;
using YapartMarket.Core.Models.Azure;
using YapartMarket.WebApi.Mapper.AliExpress;
using YapartMarket.Data.Implementation.Azure;
using YapartMarket.Core.Data.Interfaces.Azure;
using YapartMarket.WebApi.Services.Interfaces;

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
var storageAccountSettings = builder.Configuration.GetSection("StorageAccount").Get<StorageAccountSettings>()!;
builder.Services.AddSingleton(storageAccountSettings);
var connectionString = builder.Configuration.GetSection("ConnectionStrings").Get<ConnectionSettings>()!;
builder.Services.AddSingleton(connectionString);

builder.Services.AddAutoMapper(typeof(OrderProfile));
builder.Services.AddTransient<IAliExpressOrderService, AliExpressOrderService>();
builder.Services.AddTransient<IAliExpressProductService, AliExpressProductService>();
builder.Services.AddTransient<YmlServiceBase, YmlService>();

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

var commonSemaphore = new SemaphoreSlim(1);
builder.Services.AddSingleton(commonSemaphore);


builder.Services.AddQuartz(q =>
{
    var jobKey = new JobKey("UpdateInventoryJob");
    q.AddJob<UpdateInventoryJob>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("UpdateInventoryTrigger")
        .WithCronSchedule("0 */2 * * * ?"));

});

builder.Services.AddQuartz(q =>
{
    var jobKey = new JobKey("CreateLogisticOrderJob");
    q.AddJob<CreateLogisticOrderJob>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("CreateLogisticOrderTrigger")
        .WithCronSchedule("*/15 * * * * ?"));

});
//builder.Services.AddQuartz(q =>
//{
//    var jobKey = new JobKey("CreateYMLFileJob");
//    q.AddJob<CreateYMLFileJob>(opts => opts.WithIdentity(jobKey));

//    q.AddTrigger(opts => opts
//        .ForJob(jobKey)
//        .WithIdentity("CreateYMLFileJob")
//        .WithCronSchedule("*/20 * * * * ?"));

//});
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
builder.Services.AddApplicationInsightsTelemetry();

builder.Services.AddTransient<RequestBodyLoggingMiddleware>();
builder.Services.AddTransient<ResponseBodyLoggingMiddleware>();

var app = builder.Build();



if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
if (app.Environment.IsProduction())
{
    app.UseRequestBodyLogging();
    app.UseResponseBodyLogging();
}


app.UseSwagger();
app.UseSwaggerUI(option => option.SwaggerEndpoint("/swagger/v1/swagger.json", "YapartStore v1"));

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
