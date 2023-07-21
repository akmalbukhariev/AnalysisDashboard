using AnalysisDashboard.Authentication;
using AnalysisDashboard.Data;   
using AnalysisDashboard.DataAccess;
using AnalysisDashboard.Models;
using AnalysisDashboard.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
 
builder.Services.AddDbContext<DashboardContext>(options =>
{
    options.UseMySQL(builder.Configuration.GetConnectionString("Default"));
});

// Add services to the container.
builder.Services.AddAuthenticationCore();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddSingleton<UserAccountService>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<DataInfo>();

//builder.Services.AddSingleton<WeatherForecastService>();

//var cultureInfo = new CultureInfo("ru-RU");
//CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
//CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

//builder.Services.Configure<RequestLocalizationOptions>(options =>
//{
//    options.SupportedCultures = new[] { new CultureInfo("en-US"), new CultureInfo("uz-UZ"), new CultureInfo("uz-Cyrl"), new CultureInfo("ru-RU") };
//    options.SupportedUICultures = new[] { new CultureInfo("en-US"), new CultureInfo("uz-UZ"), new CultureInfo("uz-Cyrl"), new CultureInfo("ru-RU") };
//    options.DefaultRequestCulture = new RequestCulture("ru-RU");  
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//var cultures = app.Configuration.GetSection("Cultures").GetChildren().ToDictionary(x => x.Key, x => x.Value);
//var supportedCultures = cultures.Keys.ToArray();
//var localizationOptions = new RequestLocalizationOptions()
//                .AddSupportedCultures(supportedCultures)
//                .AddSupportedUICultures(supportedCultures);

app.UseHttpsRedirection();

app.UseStaticFiles();

var supportedCultures = new[] { "ru-RU", "uz-UZ", "uz-Cyrl", "en-US" };
var localizationOptions = new RequestLocalizationOptions()
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures)
    .SetDefaultCulture(supportedCultures[0]);

app.UseRequestLocalization(localizationOptions);
//app.UseRequestLocalization();

app.UseRouting();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
