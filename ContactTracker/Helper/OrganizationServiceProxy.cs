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

using ContactTracker.Helper;
using Microsoft.Crm.Sdk.Messages.Samples;
using Microsoft.Xrm.Sdk.Query.Samples;
using Microsoft.Xrm.Sdk.Samples;
using System;
using System.Threading.Tasks;

namespace Microsoft.Crm.Mobile.Samples
{
    /// <summary>
    /// OrganizationDataWebServiceProxy wrapper. Adding Authentication and
    /// error handling feature.
    /// </summary>
    public class OrganizationServiceProxy : OrganizationDataWebServiceProxy
    {
        private AuthHelper authHelper;

        // Indicate if need to clear ADAL token cache.
        public bool SignOut { get; set; }

        #region Method

        /// <summary>
        /// Constructor
        /// </summary>
        public OrganizationServiceProxy()
        {
        }

        public async Task Initialize()
        {           
            // Loading data from cache.
            ServiceUrl = (string)await Util.ReadFromLocal<string>("ServiceUrl.dat");
            string oAuthUrl = (string)await Util.ReadFromLocal<string>("OAuthUrl.dat");
            string resourceName = ServiceUrl;

            authHelper = new AuthHelper(oAuthUrl, resourceName);

            await base.EnableProxyTypes();
            base.Timeout = 600;
        }

        public async Task Associate(string entityName, Guid entityId, Relationship relationship,
            EntityReferenceCollection relatedEntities)
        {
            await Authenticate();
            try
            {
                await base.Associate(entityName, entityId, relationship, relatedEntities);
            }
            catch (Exception ex)
            {
                Util.DisplayError(ex);
            }
        }

        public async Task<Guid> Create(Entity entity)
        {
            Guid result = Guid.Empty;

            await Authenticate();
            try
            {
                result = await base.Create(entity);
            }
            catch (Exception ex)
            {
                Util.DisplayError(ex);
            }

            return result;
        }

        public async Task Delete(string entityName, Guid id)
        {
            await Authenticate();
            try
            {
                await base.Delete(entityName, id);
            }
            catch (Exception ex)
            {
                Util.DisplayError(ex);
            }
        }

        public async Task Disassociate(string entityName, Guid entityId, Relationship relationship,
            EntityReferenceCollection relatedEntities)
        {
            await Authenticate();
            try
            {
                await base.Disassociate(entityName, entityId, relationship, relatedEntities);
            }
            catch (Exception ex)
            {
                Util.DisplayError(ex);
            }
        }

        public async Task<OrganizationResponse> Execute(OrganizationRequest request)
        {
            OrganizationResponse result = null;

            await Authenticate();
            try
            {
                result = await base.Execute(request);
            }
            catch (Exception ex)
            {
                Util.DisplayError(ex);
            }

            return result;
        }

        public async Task<Entity> Retrieve(string entityName, Guid id, ColumnSet columnSet)
        {
            Entity result = null;

            await Authenticate();
            try
            {
                result = await base.Retrieve(entityName, id, columnSet);
            }
            catch (Exception ex)
            {
                Util.DisplayError(ex);
            }

            return result;
        }

        public async Task<EntityCollection> RetrieveMultiple(QueryBase query)
        {
            EntityCollection result = null;

            await Authenticate();
            try
            {
                result = await base.RetrieveMultiple(query);
            }
            catch (Exception ex)
            {
                Util.DisplayError(ex);
            }

            return result;
        }

        public async Task Update(Entity entity)
        {
            await Authenticate();
            try
            {
                await base.Update(entity);
            }
            catch (Exception ex)
            {
                Util.DisplayError(ex);
            }
        }

        private async Task Authenticate()
        {
            // Make sure AccessToken is valid.
            await authHelper.GetTokenSilent();

            // Wait until AccessToken assigned
            while (String.IsNullOrEmpty(authHelper.AccessToken))
            {
                await System.Threading.Tasks.Task.Delay(10);
            }

            AccessToken = authHelper.AccessToken;
        }

        /// <summary>
        /// This method ensures AccessToken is valid. The reason why you need to this method is
        /// that when GetTokenSlient failed, the method released synchronization and does AndConitnue
        /// pattern, which let main thread execute. In that case, service may execute with invalid 
        /// AccessToken. This method ensures it waits until AccessToken aquired.
        /// </summary>
        /// <returns></returns>
        private async Task<bool> CheckToken()
        {
            bool result = true;

            try
            {
                await base.Execute(new WhoAmIRequest());
            }
            catch
            {
                result = false;
            }

            return result;
        }


        #endregion

    
    }
}
