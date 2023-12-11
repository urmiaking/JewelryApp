using JewelryApp.Api;
using JewelryApp.Application;
using JewelryApp.Application.Hubs;
using JewelryApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services
    .AddApi(configuration)
    .AddApplication(configuration)
    .AddInfrastructure(configuration);

var app = builder.Build();

app.InitializeDatabase();

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

app.MapRazorPages();
app.MapControllers().RequireAuthorization();

app.MapHub<PriceHub>("/signalr-hub");

app.MapFallbackToFile("index.html");

app.Run();
