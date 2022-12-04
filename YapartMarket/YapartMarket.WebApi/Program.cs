using AutoMapper;
using YapartMarket.BL.Implementation;
using YapartMarket.Core.BL;
using YapartMarket.Core.Models.Azure;
using YapartMarket.WebApi.Mapper.AliExpress;
using YapartMarket.WebApi.Model.AliExpress;
using Microsoft.EntityFrameworkCore;
using YapartMarket.Data;
using YapartMarket.Core.Data.Interfaces.Azure;
using YapartMarket.Data.Implementation.Azure;
using YapartMarket.Core.Config;
using YapartMarket.Core;

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

var configuration = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<AliExpressOrder, Order>();
});
builder.Services.AddAutoMapper(typeof(OrderProfile));
builder.Services.AddTransient<IAliExpressOrderService, AliExpressOrderService>();

builder.Services.AddTransient<IAliExpressOrderRepository>(m => new AliExpressOrderRepository(new Logger<AliExpressOrderRepository>(new LoggerFactory()), "dbo.orders", builder.Configuration.GetConnectionString("SQLServerConnectionString")));
builder.Services.AddTransient<IAliExpressOrderDetailRepository>(m => new AliExpressOrderDetailRepository("dbo.order_details", builder.Configuration.GetConnectionString("SQLServerConnectionString")));

builder.Services.Configure<AliExpressOptions>(builder.Configuration.GetSection(AliExpressOptions.AliExpress));

builder.Services.AddSingleton(typeof(Deserializer<IReadOnlyList<AliExpressOrder>>), s => new OrderDeserializer());

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
