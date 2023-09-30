using Application.Repositiories;
using Domain.Aggregates;
using Infrastructure.LocalDatabase.Data;
using Infrastructure.LocalDatabase.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(@"Data Source =.\\AppDB.db;");
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                options.EnableSensitiveDataLogging(true);
            }, ServiceLifetime.Transient);

            services.AddTransient<IGenericRepository<MainBeaver>, GenericRepository<MainBeaver>>();
            services.AddTransient<IMainRepository, MainRepository>();

            return services;
        }
    }
}
