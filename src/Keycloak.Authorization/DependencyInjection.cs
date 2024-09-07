using Keycloak.Configuration;
using Keycloak.HttpClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Keycloak.Authorization
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddKeycloakAuthorization(this IServiceCollection services, string configurationSection = "Keycloak")
        {
            services.AddKeycloakConfiguration(configurationSection);
            services.AddKeycloakHttpClient();
            services.AddSingleton<IAuthorizationHandler, DecisionRequirementHandler>();

            return services;
        }
    }
}
