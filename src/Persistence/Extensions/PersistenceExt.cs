using System;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Core.Interfaces.Repository;
using Persistence.Repository;
namespace Persistence.Extensions;

public static class PersistenceExt
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration config)
    {
        //add ihttpcontextaccessor
        services.AddHttpContextAccessor();
        services.AddScoped<ISubsidiaryJournalRepository, SubsidiaryJournalRepository>();
        services.AddScoped<ICollageRepository, CollageRepository>();
        services.AddScoped<IFundRepository, FundRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IFormDetailsRepository, FormDetailsRepository>();
        services.AddScoped<IDailyRepository, DailyRepository>();
        services.AddScoped<IFormRepository, FormRepository>();
        services.AddScoped<ISubAccountRepository, SubAccountRepository>();
        services.AddScoped<IUow, UOW>();
        return services;
    }
}
