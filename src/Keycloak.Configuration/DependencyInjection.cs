using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Keycloak.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddKeycloakConfiguration(this IServiceCollection services, string configurationSection = "Keycloak")
        {
            services.AddOptions<KeycloakConfig>()
                .BindConfiguration(configurationSection)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddTransient(provider => provider.GetRequiredService<IOptions<KeycloakConfig>>().Value);

            return services;
        }
    }
}
