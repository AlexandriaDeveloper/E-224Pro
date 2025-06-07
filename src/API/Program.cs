using API.EndPoints.Daily;
using API.EndPoints.FormDetails;
using API.EndPoints.Reports;
using API.Forms;
using FastEndpoints;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Persistence.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();
builder.Services
.AddInfrastructure(builder.Configuration)
.AddPersistence(builder.Configuration)
.AddApplication(builder.Configuration)



;

//configur cros origin 
builder.Services.AddCors(opt =>
{

    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
       .WithExposedHeaders("WWW-Authenticate")
                .WithExposedHeaders("Access-Control-Allow-Origin")
                .WithOrigins([
                    "http://localhost:4200",
                    "https://localhost:4200",
                    "http://localhost:5000",
                    "https://localhost:5001",
                    "http://localhost",
                    "https://localhost",
                ]);
    });
});

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

//use controller

app.MapDailiesEndPoint();
app.MapFormsEndPoint();
app.MapFormsDetailsEndPoint();
app.MapReportEndPoint();
app.MapSubsidiaryJournalEndPoint();
app.MapAccountsEndPoint();
app.MapFundsEndpoint();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(opt =>
    {
        opt.WithTheme(ScalarTheme.Moon);
        opt.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}
app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), @"Content")),
    RequestPath = "/content"
});



app.Run();
