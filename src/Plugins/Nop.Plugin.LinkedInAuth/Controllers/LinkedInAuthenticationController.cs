using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nop.Core;
using Nop.Plugin.LinkedInAuth.Models;
using Nop.Plugin.LinkedInAuth;
using Nop.Services.Authentication.External;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Microsoft.AspNetCore.Authentication.OAuth;
using LinqToDB.Reflection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using RestSharp;

namespace Nop.Plugin.LinkedInAuth.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class LinkedInAuthenticationController : BasePluginController
    {
        #region Fields

        private readonly LinkedInExternalAuthSettings _LinkedInExternalAuthSettings;
        private readonly IAuthenticationPluginManager _authenticationPluginManager;
        private readonly IExternalAuthenticationService _externalAuthenticationService;
        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public LinkedInAuthenticationController(LinkedInExternalAuthSettings LinkedInExternalAuthSettings,
            IAuthenticationPluginManager authenticationPluginManager,
            IExternalAuthenticationService externalAuthenticationService,
            ILocalizationService localizationService,
            INotificationService notificationService,
            IPermissionService permissionService,
            ISettingService settingService,
            IStoreContext storeContext,
            IWorkContext workContext)
        {
            _LinkedInExternalAuthSettings = LinkedInExternalAuthSettings;
            _authenticationPluginManager = authenticationPluginManager;
            _externalAuthenticationService = externalAuthenticationService;
            _localizationService = localizationService;
            _notificationService = notificationService;
            _permissionService = permissionService;
            _settingService = settingService;
            _storeContext = storeContext;
            _workContext = workContext;
        }

        #endregion

        #region Methods

        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        public async Task<IActionResult> Configure()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageExternalAuthenticationMethods))
                return AccessDeniedView();

            var model = new ConfigurationModel
            {
                ClientId = _LinkedInExternalAuthSettings.ClientKeyIdentifier,
                ClientSecret = _LinkedInExternalAuthSettings.ClientSecret
            };

            return View("~/Plugins/LinkedInAuth/Views/Configure.cshtml", model);
        }

        [HttpPost]
        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        public async Task<IActionResult> Configure(ConfigurationModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageExternalAuthenticationMethods))
                return AccessDeniedView();

            if (!ModelState.IsValid)
                return await Configure();

            //save settings
            _LinkedInExternalAuthSettings.ClientKeyIdentifier = model.ClientId;
            _LinkedInExternalAuthSettings.ClientSecret = model.ClientSecret;
            await _settingService.SaveSettingAsync(_LinkedInExternalAuthSettings);

            //clear Facebook authentication options cache
            //_optionsCache.TryRemove(LinkedInDefaults.AuthenticationScheme);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

            return await Configure();
        }

        public async Task<IActionResult> Login(string returnUrl)
        {
            
            var store = await _storeContext.GetCurrentStoreAsync();
            var methodIsAvailable = await _authenticationPluginManager
                .IsPluginActiveAsync(LinkedInAuthenticationDefaults.SystemName, await _workContext.GetCurrentCustomerAsync(), store.Id);
            if (!methodIsAvailable)
                throw new NopException("LinkedIn authentication module cannot be loaded");

            if (string.IsNullOrEmpty(_LinkedInExternalAuthSettings.ClientKeyIdentifier) ||
                string.IsNullOrEmpty(_LinkedInExternalAuthSettings.ClientSecret))
            {
                throw new NopException("LinkedIn authentication module not configured");
            }
            var clientId = _LinkedInExternalAuthSettings.ClientKeyIdentifier;
            var redirectUrl = "https://localhost:57925/LinkedInAuthentication/LoginCallback";
            //configure login callback action
            //var authenticationProperties = new AuthenticationProperties
            //{
            //    RedirectUri = Url.Action("LoginCallback", "LinkedInAuthentication", new { returnUrl = returnUrl })
            //};
            //authenticationProperties.SetString(LinkedInAuthenticationDefaults.ErrorCallback, Url.RouteUrl("Login", new { returnUrl }));

            return Redirect($"https://www.linkedin.com/oauth/v2/authorization?response_type=code&client_id={clientId}&redirect_uri={redirectUrl}&scope=r_liteprofile%20r_emailaddress");
        }

        public async Task<IActionResult> LoginCallback(string code)
        {
            var returnUrl = "/";
            string clientId = _LinkedInExternalAuthSettings.ClientKeyIdentifier;
            string clientSecret = _LinkedInExternalAuthSettings.ClientSecret;
            string redirectUrl = "https://localhost:57925/LinkedInAuthentication/LoginCallback";



            // Exchange the authorization code for an access token
            var client = new RestClient("https://www.linkedin.com/oauth/v2/accessToken");
            var request = new RestRequest();
            request.Method = Method.Post; // Use Method.POST to set the request method to POST
            request.AddParameter("grant_type", "authorization_code");
            request.AddParameter("code", code);
            request.AddParameter("redirect_uri", redirectUrl);
            request.AddParameter("client_id", clientId);
            request.AddParameter("client_secret", clientSecret);



            var response = await client.ExecuteAsync(request);
            var content = response.Content;

            // Parse the response to get the access token
            dynamic tokenResponse = JsonConvert.DeserializeObject(content);

            string firstName;
            string lastName;
            string emailAddress;
            string Id;
            string accessToken = tokenResponse.access_token;
            var claims = new List<ExternalAuthenticationClaim>();
            using (HttpClient httpClient = new HttpClient())
            {
                // Use the access token to fetch user information
                string userProfileUrl = "https://api.linkedin.com/v2/me?projection=(id,firstName,lastName)";
                string usermail = "https://api.linkedin.com/v2/emailAddress?q=members&projection=(elements*(handle~))";
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var userProfileResponse = await httpClient.GetAsync(userProfileUrl);
                var usermailRepsonse = await httpClient.GetAsync(usermail);
                var userProfileContent = await userProfileResponse.Content.ReadAsStringAsync();
                var usermainProfileContenet = await usermailRepsonse.Content.ReadAsStringAsync();
                var userProfileJson = JObject.Parse(userProfileContent);
                var usermailProfileJson = JObject.Parse(usermainProfileContenet);
                firstName = userProfileJson["firstName"]["localized"]["en_US"].ToString();
                lastName = userProfileJson["lastName"]["localized"]["en_US"].ToString();
                Id = userProfileJson["id"].ToString();
                emailAddress = usermailProfileJson["elements"]?[0]?["handle~"]?["emailAddress"]?.ToString();
                // Create claims for user data
                claims = new List<ExternalAuthenticationClaim>
                {
                    new ExternalAuthenticationClaim(ClaimTypes.NameIdentifier, Id),
                    new ExternalAuthenticationClaim(ClaimTypes.Name, firstName + lastName),
                    new ExternalAuthenticationClaim(ClaimTypes.GivenName, firstName),
                    new ExternalAuthenticationClaim(ClaimTypes.Surname, lastName),
                    new ExternalAuthenticationClaim(ClaimTypes.Email, emailAddress),
                    // Add any additional claims you want to include
                };
            }

            var authenticationParameters = new ExternalAuthenticationParameters
            {
                ProviderSystemName = "LinkedIn",
                AccessToken = accessToken,
                Email = emailAddress,
                ExternalIdentifier = Id,
                ExternalDisplayIdentifier = firstName + lastName,
                Claims = claims
            };
            
            //authenticate Nop user
            return await _externalAuthenticationService.AuthenticateAsync(authenticationParameters, returnUrl);
        }

        public async Task<IActionResult> DataDeletionStatusCheck(int earId)
        {
            var externalAuthenticationRecord = await _externalAuthenticationService.GetExternalAuthenticationRecordByIdAsync(earId);
            if (externalAuthenticationRecord is not null)
                _notificationService.WarningNotification(await _localizationService.GetResourceAsync("Plugins.LinkedInAuth.AuthenticationDataExist"));
            else
                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Plugins.LinkedInAuth.AuthenticationDataDeletedSuccessfully"));

            return RedirectToRoute("CustomerInfo");
        }

        #endregion
    }
}