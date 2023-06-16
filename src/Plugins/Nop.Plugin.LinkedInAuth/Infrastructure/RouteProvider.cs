using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Plugin.LinkedInAuth;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.LinkedInAuth.Infrastructure
{
    /// <summary>
    /// Represents plugin route provider
    /// </summary>
    public class RouteProvider : IRouteProvider
    {
        /// <summary>
        /// Register routes
        /// </summary>
        /// <param name="endpointRouteBuilder">Route builder</param>
        public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapControllerRoute(LinkedInAuthenticationDefaults.DataDeletionCallbackRoute, $"facebook/data-deletion-callback/",
                new { controller = "LinkedInDataDeletion", action = "DataDeletionCallback" });

            endpointRouteBuilder.MapControllerRoute(LinkedInAuthenticationDefaults.DataDeletionStatusCheckRoute, $"facebook/data-deletion-status-check/{{earId:min(0)}}",
                new { controller = "LinkedInAuthentication", action = "DataDeletionStatusCheck" });
        }

        /// <summary>
        /// Gets a priority of route provider
        /// </summary>
        public int Priority => 0;
    }
}
