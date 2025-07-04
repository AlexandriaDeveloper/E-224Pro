using API.EndPoints.Daily;
using API.EndPoints.FormDetails;
using API.EndPoints.Reports;
using API.EndPoints;
using API.Forms;
using Microsoft.Extensions.FileProviders;
using Persistence.Extensions;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Core.Models;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();
builder.Services
.AddInfrastructure(builder.Configuration)
.AddPersistence(builder.Configuration)
.AddApplication(builder.Configuration)
;

// Add Identity services
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


// Add Authentication services
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

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
builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAntiforgery();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Seed admin user if no users exist
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Core.Models.AppUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await Infrastructure.IdentitySeeder.SeedAdminUserAsync(userManager, roleManager);
}

//use controller

app.MapDailiesEndPoint();
app.MapFormsEndPoint();
app.MapFormsDetailsEndPoint();
app.MapReportEndPoint();
app.MapSubsidiaryJournalEndPoint();
app.MapAccountsEndPoint();
app.MapCollgaesEndPoint();
app.MapFundsEndpoint();
app.MapSubAccountsEndPoint();
app.MapAuthEndpoints();

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

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery(); // Enable CSRF protection

app.Run();
