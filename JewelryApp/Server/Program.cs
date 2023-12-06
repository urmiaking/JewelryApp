using JewelryApp.Api;
using JewelryApp.Business;
using JewelryApp.Business.Hubs;
using JewelryApp.Data;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services
    .AddApi(builder.Configuration)
    .AddBusiness(builder.Configuration)
    .AddData(builder.Configuration);

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
