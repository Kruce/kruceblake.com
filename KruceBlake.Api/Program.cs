using System.Net.Http.Headers;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRateLimiter(options =>
{
    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
        {
            await context.HttpContext.Response.WriteAsync(
                $"too many requests. please try again after {retryAfter.TotalMinutes} minute(s). ", cancellationToken: token);
        }
        else
        {
            await context.HttpContext.Response.WriteAsync(
                "too many requests. please try again later.", cancellationToken: token);
        }
    };
    options.AddPolicy("Api", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter($"{httpContext.Request.Path}{httpContext.Connection.RemoteIpAddress}",
        partition => new FixedWindowRateLimiterOptions
        {
            AutoReplenishment = true,
            PermitLimit = 20,
            Window = TimeSpan.FromMinutes(1)
        }));
});

builder.Services.AddHttpClient("CronJob", client =>
{
    client.BaseAddress = new Uri("https://api.cron-job.org/");
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", builder.Configuration["CronJobApiBearerToken"]);
});
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseMiddleware<ApiKeyMiddleware>(); //enable if all requests should be authorized using the api key
app.UseRateLimiter();
app.UseStatusCodePages();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();