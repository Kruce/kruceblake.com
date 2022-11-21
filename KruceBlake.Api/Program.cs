using KruceBlake.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

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
app.UseStatusCodePages();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();