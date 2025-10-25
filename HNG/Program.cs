using System.Threading.RateLimiting;
using HNG.Services.Implementation;
using HNG.Services.Interface;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


builder.Services.AddScoped<IHelperService, HelperService>();
builder.Services.AddScoped<INaturalLanguageParsingService, NaturalLanguageParsingService>();
builder.Services.AddScoped<IStringAnalysisService, StringAnalysisService>();


const string myAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter(policyName: "fixed", limiterOptions =>
    {
        limiterOptions.PermitLimit = 100;
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 5;
    });
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

// Configure Forwarded Headers to trust the reverse proxy
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});


builder.Services.AddEndpointsApiExplorer();
builder.WebHost.UseUrls("http://0.0.0.0:8080");
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/health", () => Results.Ok("alive"));

// Use Forwarded Headers middleware before other middleware.
app.UseForwardedHeaders();

//app.UseHttpsRedirection();

app.UseCors(myAllowSpecificOrigins);
app.UseRateLimiter();
Console.WriteLine("Starting HNGTAsk2 Web API...");


app.MapControllers().RequireRateLimiting("fixed");

app.Run();
