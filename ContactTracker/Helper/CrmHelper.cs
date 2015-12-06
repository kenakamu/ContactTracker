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

using Microsoft.Crm.Sdk.Messages.Samples;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Xrm.Sdk.Query.Samples;
using Microsoft.Xrm.Sdk.Samples;
using MyTypes;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Microsoft.Crm.Mobile.Samples
{
    /// <summary>
    /// Helper class for this application.
    /// </summary>
    static public class CrmHelper
    {
        #region Property

        static public OrganizationServiceProxy _proxy;

        // Indicate if this class loaded cached data. As constructor cannot mark as async
        // use this property to let application wait until data loaded.
        static public bool IsLoading { get; set; }

        #endregion

        #region Method

        /// <summary>
        /// Constructor. As async/await cannot be used in constructor, call Initialize and
        /// use IsLoading flag to indicator the status.
        /// </summary>
        static CrmHelper()
        {
            // Mark IsLoaded false
            IsLoading = true;

            // Initialize data which needs to be done asynchronously
            Initialize();
        }

        /// <summary>
        /// This method Loads data from cache
        /// </summary>
        static async void Initialize()
        {
            // Instantiate proxy
            _proxy = new OrganizationServiceProxy();
            await _proxy.Initialize();
            // Mark loaded as true
            IsLoading = false;
        }

        /// <summary>
        /// Clear all caches
        /// </summary>
        /// <returns></returns>
        static public async Task Clear()
        {
            // Recreate proxy
            _proxy = new OrganizationServiceProxy();
            await _proxy.Initialize();
            // Delete all locally stored cache files
            await Util.DeleteLocalCache();
            // Mark proxy as SingOut status
            _proxy.SignOut = true;
        }

        /// <summary>
        /// This method retrives a login user name. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        static public async Task<string> RetrieveUser()
        {
            // Execute WhoAmI to get SystemUserId
            WhoAmIResponse response = (WhoAmIResponse)await _proxy.Execute(new WhoAmIRequest());
            // Then get user full name.
            Entity user = await _proxy.Retrieve("systemuser", response.UserId, new ColumnSet("fullname"));

            // As user record always has fullname, no null check.
            return user["fullname"].ToString();
        }

        /// <summary>
        /// Retrieve top 50 contacts and fields using FetchXML
        /// </summary>
        /// <returns>Contact records</returns>
        static public async Task<ObservableCollection<Contact>> RetrieveContacts(string searchCriteria="")
        {
            ObservableCollection<Contact> contacts = new ObservableCollection<Contact>();
            string fetch = String.Format(@"<fetch version='1.0' output-format='xml-platform' mapping='logical' count='50' distinct='false'>
  <entity name='contact'>
    <attribute name='fullname' />
    <attribute name='jobtitle' />
    <attribute name='contactid' />
    <order attribute='fullname' descending='false' />
    <filter type='and'>
      <condition attribute='firstname' operator='like' value='%{0}%' />
    </filter>
  </entity>
</fetch>", searchCriteria);

            EntityCollection results = await _proxy.RetrieveMultiple(new FetchExpression(fetch));

            // Add to result set
            foreach (Contact result in results.Entities)
            {
                contacts.Add(result);
            }

            return contacts;
        }

        /// <summary>
        /// Retrieve a contact record
        /// </summary>
        /// <param name="id">Record Id</param>
        /// <returns>Contact record</returns>
        static public async Task<Contact> RetrieveContact(Guid id)
        {
            Contact result = (Contact)await _proxy.Retrieve("contact", id,
                new ColumnSet("firstname", "lastname", "fullname", "jobtitle", "telephone1", "emailaddress1", "websiteurl", "parentcustomerid", "entityimage"));
            
            return result;
        }

        /// <summary>
        /// Update existing record, otherwise create new one.
        /// </summary>
        /// <param name="contact">Contact record object</param>
        static public async Task UpsertContact(Contact contact)
        {
            if (contact.Id == Guid.Empty)
                await _proxy.Create(contact);
            else
                await _proxy.Update(contact);
        }

        /// <summary>
        /// Retrieve OAuth Url. See http://msdn.microsoft.com/en-us/library/dn531009.aspx#bkmk_oauth_discovery for detail
        /// </summary>
        /// <returns></returns>
        static public async Task<string> DiscoveryAuthority(string serviceUrl)
        {
            string OAuthUrl = String.Empty;

            // If server url is null, then return false.
            if (String.IsNullOrEmpty(serviceUrl))
                return OAuthUrl;

            // As I cannot display message in catch, as it does not allow await inside, use this flag to indicate if 
            // OAuthUrl has been retrieved successfully or not.
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer");
                    // need to specify soap endpoint with client version,.
                    HttpResponseMessage httpResponse = await httpClient.GetAsync(
                        serviceUrl + "/XRMServices/2011/Organization.svc/web?SdkClientVersion=6.1.0.533");
                    // For phone, we dont need oauth2/authorization part.
                    OAuthUrl = AuthenticationParameters.CreateFromResponseAuthenticateHeader(httpResponse.Headers.WwwAuthenticate.ToString()).Authority;

                    await Util.SaveToLocal(OAuthUrl, "OAuthUrl.dat");
                }
            }
            catch (Exception ex)
            {
                OAuthUrl = String.Empty;
                Util.DisplayError(ex);
            }

            return OAuthUrl;
        }

        #endregion
    }
}
