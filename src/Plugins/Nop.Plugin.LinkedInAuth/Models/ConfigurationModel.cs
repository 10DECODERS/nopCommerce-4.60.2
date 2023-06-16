using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.LinkedInAuth.Models
{
    /// <summary>
    /// Represents plugin configuration model
    /// </summary>
    public record ConfigurationModel : BaseNopModel
    {
        [NopResourceDisplayName("Plugins.LinkedInAuth.ClientKeyIdentifier")]
        public string ClientId { get; set; }

        [NopResourceDisplayName("Plugins.LinkedInAuth.ClientSecret")]
        public string ClientSecret { get; set; }
    }
}