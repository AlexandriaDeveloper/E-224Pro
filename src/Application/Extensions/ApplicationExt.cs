
using Application.Services;
using Core.Interfaces.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repository;

namespace Persistence.Extensions;

public static class ApplicationExt
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<DailyService>();
        services.AddScoped<FormService>();
        services.AddScoped<FormDetailsService>();
        services.AddScoped<ReportService>();
        services.AddScoped<SubsidiaryJournalService>();
        services.AddScoped<AccountService>();
        services.AddScoped<FundService>();
        return services;
    }
}
