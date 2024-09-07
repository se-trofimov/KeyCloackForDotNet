using Microsoft.AspNetCore.Authorization;

namespace Keycloak.Authorization
{
    /// <summary>
    /// Decision requirement
    /// </summary>
    public class DecisionRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// Resource name
        /// </summary>
        public string Resource { get; }


        /// <summary>
        /// Resource scope
        /// </summary>
        public string Scope { get; }

        /// <summary>
        /// Constructs requirement
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="scope"></param>
        public DecisionRequirement(string resource, string scope)
        {
            Resource = resource;
            Scope = scope;
        }

        /// <summary>
        /// Constructs requirement based on resource id
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="id"></param>
        /// <param name="scope"></param>
        public DecisionRequirement(string resource, string id, string scope)
            : this($"{resource}/{id}", scope)
        {
        }

        /// <inheritdoc />
        public override string ToString() => $"{nameof(DecisionRequirement)}: {this.Resource}#{this.Scope}";
    }
}
