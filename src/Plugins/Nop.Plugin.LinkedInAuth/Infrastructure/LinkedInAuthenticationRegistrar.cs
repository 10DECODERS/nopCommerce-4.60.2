//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.OAuth;
//using Nop.Core.Infrastructure;
//using Nop.Plugin.LinkedInAuth;
//using Nop.Services.Authentication.External;

//namespace Nop.Plugin.LinkedInAuth.Infrastructure
//{
//    /// <summary>
//    /// Represents registrar of Facebook authentication service
//    /// </summary>
//    public class LinkedInAuthenticationRegistrar : IExternalAuthenticationRegistrar
//    {
//        /// <summary>
//        /// Configure
//        /// </summary>
//        /// <param name="builder">Authentication builder</param>
//        public void Configure(AuthenticationBuilder builder)
//        {
//            builder.AddLinkedIn(LinkedInDefaults.AuthenticationScheme, options =>
//            {
//                //set credentials
//                var settings = EngineContext.Current.Resolve<LinkedInExternalAuthSettings>();

//                options.ClientId = string.IsNullOrEmpty(settings?.ClientKeyIdentifier) ? nameof(options.ClientId) : settings.ClientKeyIdentifier;
//                options.ClientSecret = string.IsNullOrEmpty(settings?.ClientSecret) ? nameof(options.ClientSecret) : settings.ClientSecret;
                
//                //store access and refresh tokens for the further usage
//                options.SaveTokens = true;

//                //set custom events handlers
//                options.Events = new OAuthEvents
//                {
//                    //in case of error, redirect the user to the specified URL
//                    OnRemoteFailure = context =>
//                    {
//                        context.HandleResponse();

//                        var errorUrl = context.Properties.GetString(LinkedInAuthenticationDefaults.ErrorCallback);
//                        context.Response.Redirect(errorUrl);

//                        return Task.FromResult(0);
//                    }
//                };
//            });
//        }
//    }
//}