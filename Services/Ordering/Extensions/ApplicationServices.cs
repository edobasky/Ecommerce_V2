using System.Reflection;
using FluentValidation;
using MediatR;
using Ordering.Behaviour;
using Ordering.Handlers;

namespace Ordering.Extensions
{
    public static class ApplicationServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var assembly = typeof(ApplicationServices).Assembly;
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly));
             services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>),typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            return services;
        }
    }
}
