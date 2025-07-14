
using Application.Services;
using Core.Interfaces.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Persistence.Repository;
using QuestPDF.Infrastructure;
namespace Persistence.Extensions;

public static class ApplicationExt
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration config)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        services.AddScoped<DailyService>();
        services.AddScoped<FormService>();
        services.AddScoped<FormDetailsService>();
        services.AddScoped<ReportService>();
        services.AddScoped<SubsidiaryJournalService>();
        services.AddScoped<SubSidaryDailyService>();
        services.AddScoped<AccountService>();
        services.AddScoped<FundService>();
        services.AddScoped<PDFReportService>(provider =>
        {
            var reportService = provider.GetRequiredService<ReportService>();
            var subsidaryDailyService = provider.GetRequiredService<SubSidaryDailyService>();
            var config = provider.GetRequiredService<IConfiguration>();
            var webHostEnvironment = provider.GetRequiredService<IWebHostEnvironment>();
            return new PDFReportService(reportService, subsidaryDailyService, config, webHostEnvironment);
        });
        services.AddScoped<CollageService>();
        services.AddScoped<SubAccountService>();
        services.AddScoped<ExcelService>();
        services.AddScoped<TokenService>();
        //services.AddScoped<NpoiService>();


        return services;
    }
}
