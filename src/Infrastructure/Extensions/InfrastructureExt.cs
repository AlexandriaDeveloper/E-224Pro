
using Infrastructure;
using Infrastructure.Interceptors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Extensions;

public static class InfrastructureExt
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
          {
              var auditableInterceptor = sp.GetService<AuditLogsInterceptor>();
              options.UseSqlServer(config.GetConnectionString("DefaultConnection"))
              //add logging

              .AddInterceptors(auditableInterceptor!)


              //.AddInterceptors(auditableInterceptor)
              ;
          });
        services.AddSingleton<AuditLogsInterceptor>();
        return services;
    }
}
