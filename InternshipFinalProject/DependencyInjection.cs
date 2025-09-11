using InternshipFinalProject_Application;
using InternshipFinalProject_Core;
using InternshipFinalProject_Infrastructure;

namespace InternshipFinalProject
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentationDI(this IServiceCollection services)
        {
            services.AddInfraDI()
                   .AddApplicationDI()
                   .AddCoreDI();
            return services;
        }
    }
}
