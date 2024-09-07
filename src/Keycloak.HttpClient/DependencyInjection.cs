 

using Microsoft.Extensions.DependencyInjection;

namespace Keycloak.HttpClient
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddKeycloakHttpClient(this IServiceCollection services, string configurationSection = "Keycloak")
        {
            services.AddHttpClient<KeycloakHttpClient>();

            return services;
        }
    }
}
