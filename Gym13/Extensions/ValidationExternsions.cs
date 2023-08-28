using FluentValidation;
using Gym13.Application.Validators;

namespace Gym13.Extensions
{
    public static class ValidationExternsions
    {
        public static IServiceCollection AddFluentValidation(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<RegistrationRequestValidator>();
            return services;
        }
    }
}
