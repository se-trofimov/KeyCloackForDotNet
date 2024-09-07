using Keycloak.Configuration;
using System.Net;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;


namespace Keycloak.HttpClient
{
    /// <summary>
    /// A client for verifying access to a resource using Keycloak.
    /// </summary>
    /// <param name="client">HttpClient.</param>
    /// <param name="options">Keycloak config options.</param>
    public sealed partial class KeycloakHttpClient(
        System.Net.Http.HttpClient client,
        IOptions<KeycloakConfig> options,
        ILogger<KeycloakHttpClient> logger)
    {
        public const string TokenEndpointPath = "protocol/openid-connect/token";

        public async Task<bool> VerifyAccessToResource(
            string resource, string scope, CancellationToken cancellationToken)
        {
            var data = new Dictionary<string, string>
                {
                    {"grant_type", "urn:ietf:params:oauth:grant-type:uma-ticket"},
                    {"response_mode", "decision"},
                    {"audience", options.Value.Audience },
                    {"permission", $"{resource}#{scope}"}
                };

            LogSendingRequest(logger, resource, scope);

            var response = await client.PostAsync(
                TokenEndpointPath, new FormUrlEncodedContent(data), cancellationToken);

            LogResponse(logger, response.StatusCode);

            return response.IsSuccessStatusCode;
        }

        [LoggerMessage(0, LogLevel.Trace, "Sending authorization request {resource}#{scope}")]
        partial void LogSendingRequest(ILogger logger, string resource, string scope);

        [LoggerMessage(0, LogLevel.Trace, "Authorization request finished with result <{result}>")]
        partial void LogResponse(ILogger logger, HttpStatusCode result);
    }
}
