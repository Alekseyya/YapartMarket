using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using YapartMarket.Core.Extensions;
using YapartMarket.Data;
using YapartMarket.WebApi.Controllers;
using YapartMarket.WebApi.Services;
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
builder.Services.AddControllers()
    .AddApplicationPart(typeof(GoodsController).Assembly)
    .AddApplicationPart(typeof(AliExpressController).Assembly);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient("goodsClient", c => c.BaseAddress = new Uri("https://partner.goodsteam.tech"));
builder.Services.AddTransient<IGoodsService, GoodsService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }