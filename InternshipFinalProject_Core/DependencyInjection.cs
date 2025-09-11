using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipFinalProject_Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCoreDI(this IServiceCollection services)
        {
            //services.AddScoped<UserCreationResult>();

            return services;
        }
    }
}
