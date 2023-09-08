using System.Reflection;
using AutoMapper;
using JewelryApp.Api.Extensions;
using JewelryApp.Business.Hubs;
using JewelryApp.Business.Jobs;
using JewelryApp.Models.AppModels;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDatabase(configuration);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

builder.Services.RegisterAutoMapper();

builder.Services.RegisterServices();

builder.Services.AddCustomIdentity();

builder.Services.AddCustomAuthentication(configuration);

builder.Services.AddPriceUpdateJob(configuration);

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseInitializer();


app.MapRazorPages();
app.MapControllers().RequireAuthorization();

app.MapHub<PriceHub>("/signalr-hub");

app.MapFallbackToFile("index.html");

app.Run();
