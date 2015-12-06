/*================================================================================================================================

  This Sample Code is provided for the purpose of illustration only and is not intended to be used in a production environment.  

  THIS SAMPLE CODE AND ANY RELATED INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, 
  INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.  

  We grant You a nonexclusive, royalty-free right to use and modify the Sample Code and to reproduce and distribute the object 
  code form of the Sample Code, provided that You agree: (i) to not use Our name, logo, or trademarks to market Your software 
  product in which the Sample Code is embedded; (ii) to include a valid copyright notice on Your software product in which the 
  Sample Code is embedded; and (iii) to indemnify, hold harmless, and defend Us and Our suppliers from and against any claims 
  or lawsuits, including attorneys’ fees, that arise or result from the use or distribution of the Sample Code.

 =================================================================================================================================*/

using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Security.Authentication.Web;
using Windows.UI.Popups;

namespace ContactTracker.Helper
{
    public class AuthHelper
    {
        #region Property

        // You need to update this section to match your environment if you don't want to see consent. 
        private string ClientId = "87f65d63-9ccd-4b1c-91d3-067d9d1908a2";

        private string RedirectUri = WebAuthenticationBroker.GetCurrentApplicationCallbackUri().ToString();

        // OAuthUrl. This value should be resolved automatically by using discovery service.
        // You can also hardcord it and avoid calling discovery serivice.
        private string oAuthUrl;
        private string resourceName;

        public string AccessToken;
        
        static private AuthenticationContext authContext = null;

        #endregion

        #region Method

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="oAuthUrl">OAuthUrl</param>
        /// <param name="resourceName">ResourceName</param>
        public AuthHelper(string oAuthUrl, string resourceName)
        {
            this.oAuthUrl = oAuthUrl;
            this.resourceName = resourceName;
        }
        
        /// <summary>
        /// Try to get AccessToken Silently, and if failed, it shows SignIn dialog instead.
        /// </summary>
        /// <returns></returns>
        public async Task GetTokenSilent(bool signOut = false)
        {
            // Check if user Signed Out
            if (signOut)
            {
                // If authContext exists, then clear cache
                if (authContext != null)
                    authContext.TokenCache.Clear();

                signOut = false;
            }

            // If no authContext, then create it.
            if (authContext == null)
                authContext = AuthenticationContext.CreateAsync(oAuthUrl).GetResults();

            // Try to acquire token without prompting user first.
            AuthenticationResult result = await authContext.AcquireTokenSilentAsync(resourceName, ClientId);

            // Check the result, if failed, then prompt login to user.
            if (result != null && result.Status == AuthenticationStatus.Success)
                StoreToken(result);
            else
                authContext.AcquireTokenAndContinue(resourceName, ClientId, new Uri(RedirectUri), StoreToken);
        }

        /// <summary>
        /// This mothod called when ADAL obtained AccessToken
        /// </summary>
        /// <param name="result"></param>
        private void StoreToken(AuthenticationResult result)
        {
            // Double check the result again.
            if (result.Status == AuthenticationStatus.Success)
            {
                AccessToken = result.AccessToken;
            }
            else
            {
                DisplayErrorWhenAcquireTokenFails(result);
            }
        }

        /// <summary>
        /// This is helper method to display authentication error detail.
        /// </summary>
        /// <param name="result"></param>
        private async void DisplayErrorWhenAcquireTokenFails(AuthenticationResult result)
        {
            MessageDialog dialog;

            switch (result.Error)
            {
                case "authentication_canceled":
                    // User cancelled, so no need to display a message.
                    break;
                case "temporarily_unavailable":
                case "server_error":
                    dialog = new MessageDialog("Please retry the operation. If the error continues, please contact your administrator.", "Sorry, an error has occurred.");
                    await dialog.ShowAsync();
                    break;
                default:
                    // An error occurred when acquiring a token. Show the error description in a MessageDialog. 
                    dialog = new MessageDialog(string.Format("If the error continues, please contact your administrator.\n\nError: {0}\n\nError Description:\n\n{1}", result.Error, result.ErrorDescription), "Sorry, an error has occurred.");
                    await dialog.ShowAsync();
                    break;
            }
        }

        static public async void ContinueWebAuthentication(WebAuthenticationBrokerContinuationEventArgs args)
        {
            // pass the authentication interaction results to ADAL, which will conclude the token acquisition operation and invoke the callback specified in AcquireTokenAndContinue. 
            await authContext.ContinueAcquireTokenAsync(args);
        }

        #endregion       
    }
}
