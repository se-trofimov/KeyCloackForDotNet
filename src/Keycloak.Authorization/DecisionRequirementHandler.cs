using Keycloak.HttpClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Keycloak.Authorization
{
    /// <summary>
    /// </summary>
    public partial class DecisionRequirementHandler(KeycloakHttpClient client,
        ILogger<DecisionRequirementHandler> logger) : AuthorizationHandler<DecisionRequirement>
    {
        [LoggerMessage(103, LogLevel.Debug,
            "[{Requirement}] Access outcome {Outcome} for user {UserName}")]
        partial void DecisionAuthorizationResult(ILogger logger, string requirement, bool outcome, string? userName);

        /// <inheritdoc/>
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            DecisionRequirement requirement)
        {
            if (context.User.Identity?.IsAuthenticated ?? false)
            {
                var success = await client.VerifyAccessToResource(
                    requirement.Resource, requirement.Scope, CancellationToken.None);

                DecisionAuthorizationResult(logger,
                    requirement.ToString(), success, context.User.Identity?.Name);

                if (success)
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
            }
            else
            {
                DecisionAuthorizationResult(logger,
                    requirement.ToString(), false, context.User.Identity?.Name);
            }
        }
    }
}
