using Intelly_Web.Interfaces;
using Intelly_Web.Models;
using Intelly_Web.Implementations;
using Microsoft.Extensions.Caching.StackExchangeRedis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddMvc();
builder.Services.AddHttpContextAccessor();
builder.Services.AddResponseCaching();
builder.Services.AddSession();

builder.Services.AddSingleton<IUserModel, UserModel>();
builder.Services.AddSingleton<ICompanyModel, CompanyModel>();
builder.Services.AddSingleton<IMarketing, MarketingModel>();
builder.Services.AddSingleton<ICustomer, CustomerModel>();
builder.Services.AddSingleton<ILocalModel, LocalModel>();
builder.Services.AddSingleton<ITools, Tools>();
builder.Services.AddSingleton<ICharts, Charts>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Authentication}/{action=Login}/{id?}");

app.Run();

