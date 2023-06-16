

namespace Nop.Plugin.LinkedInAuth
{
    /// <summary>
    /// Represents plugin constants
    /// </summary>
    public class LinkedInAuthenticationDefaults
    {
        /// <summary>
        /// Gets a plugin system name
        /// </summary>
        public static string SystemName => "LinkedInAuth";

        /// <summary>
        /// Gets a name of the route to the data deletion callback
        /// </summary>
        public static string DataDeletionCallbackRoute => "Plugin.LinkedInAuth.DataDeletionCallback";

        /// <summary>
        /// Gets a name of the route to the data deletion status check
        /// </summary>
        public static string DataDeletionStatusCheckRoute => "Plugin.LinkedInAuth.DataDeletionStatusCheck";

        /// <summary>
        /// Gets a name of error callback method
        /// </summary>
        public static string ErrorCallback => "ErrorCallback";
    }
}