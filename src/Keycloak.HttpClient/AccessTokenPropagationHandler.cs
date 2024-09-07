using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;

namespace Keycloak.HttpClient
{
    /// <summary>
    /// Delegating handler to propagate headers
    /// </summary>
    public class AccessTokenPropagationHandler(IHttpContextAccessor contextAccessor) 
        : DelegatingHandler
    {
        /// <inheritdoc/>
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (contextAccessor.HttpContext == null)
            {
                return await Continue();
            }

            var httpContext = contextAccessor.HttpContext;
            var token = await httpContext
                .GetTokenAsync(JwtBearerDefaults.AuthenticationScheme, "access_token");

            if (!StringValues.IsNullOrEmpty(token))
            {
                SetToken(request, JwtBearerDefaults.AuthenticationScheme, token!);
            }

            return await Continue();

            Task<HttpResponseMessage> Continue() =>
                base.SendAsync(request, cancellationToken);
        }

        public static void SetToken(HttpRequestMessage request, string scheme, string token)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue(scheme, token);
        }
    }
}
