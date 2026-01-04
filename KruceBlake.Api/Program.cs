using KruceBlake.Api.Exceptions;
using KruceBlake.Api.Handlers;
using KruceBlake.Api.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json;
using Serilog;
using System.Net.Http.Headers;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Services.Configure<KruceBlakeApiOptions>(
    builder.Configuration.GetSection(KruceBlakeApiOptions.KruceBlakeApi));

builder.Services.AddSerilog();
builder.Services.AddRateLimiter(options =>
{
    options.OnRejected = async (context, cancellationToken) =>
    {
        Log.Warning($"rate limit exceeded for IP: {context.HttpContext.Connection.RemoteIpAddress}");

        if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
            throw new TooManyRequestsException($"rate limit exceeded. please try again after {retryAfter.TotalMinutes} minute(s).", TimeSpan.FromMinutes(1));
        else
            throw new TooManyRequestsException("rate limit exceeded. please try again later.");
    };

    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 20,
                Window = TimeSpan.FromMinutes(1)
            }));
});

builder.Services.AddHttpClient("CronJob", client =>
{
    client.BaseAddress = new Uri("https://api.cron-job.org/");
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, builder.Configuration["CronJobApiKey"]);
});

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseExceptionHandler();
app.UseStatusCodePages();
app.UseRateLimiter();
app.UseHttpsRedirection();
//app.UseMiddleware<ApiKeyMiddleware>(); //enable if all requests should be authorized using the api key
app.UseAuthorization();
app.MapControllers();

app.Run();