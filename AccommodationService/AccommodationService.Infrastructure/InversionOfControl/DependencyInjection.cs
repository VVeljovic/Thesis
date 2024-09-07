using AccommodationService.Application.Interfaces;
using AccommodationService.Infrastructure.MongoDb;
using AccommodationService.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationService.Infrastructure.InversionOfControl
{
    public static class DependencyInjection
    
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IAccommodationService, AccommodationServiceImpl>();
            services.AddScoped<IReviewService, ReviewServiceImpl>();
            services.AddSingleton<AccommodationContext>();
            services.AddSingleton<ReviewContext>();

            return services;

        }
    }
}
