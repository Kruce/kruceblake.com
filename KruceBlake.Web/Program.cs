using KruceBlake.Web.Options;
using KruceBlake.Web.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();

// Add services to the container.
builder.Services.AddSerilog();
builder.Services.AddControllersWithViews();
builder.Services.Configure<GoogleAnalyticsOptions>(
    builder.Configuration.GetSection(GoogleAnalyticsOptions.GoogleAnalytics));
builder.Services.AddTransient<ITagHelperComponent, GoogleAnalyticsTagHelperComponent>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseStatusCodePagesWithReExecute("/Error/{0}");
    app.UseHsts();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();