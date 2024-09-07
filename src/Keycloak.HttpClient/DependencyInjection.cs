 

using Keycloak.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Keycloak.HttpClient
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddKeycloakHttpClient(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddHttpClient<KeycloakHttpClient>((provider, client) =>
            {
                var options = provider.GetRequiredService<IOptions<KeycloakConfig>>();
                var baseUrl = new Uri(options.Value.RealmUrl.TrimEnd('/') + "/");
                client.BaseAddress = baseUrl;
                
            }).AddHeaderPropagation();


            return services;
        }

        public static IHttpClientBuilder AddHeaderPropagation(this IHttpClientBuilder builder)
        {
            builder.AddHttpMessageHandler((sp) =>
            {
                var contextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                return new AccessTokenPropagationHandler(contextAccessor);
            });

            return builder;
        }
    }
}
