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

using ContactTracker.Model;
using Microsoft.Crm.Mobile.Samples;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ContactTracker.Helper
{
    /// <summary>
    /// OneNote API helper. See https://github.com/OneNoteDev for more samples and details.
    /// </summary>
    public class OneNoteHelper
    {
        private AuthHelper authHelper;
        
        /// <summary>
        /// Initializing the helper.
        /// </summary>
        /// <returns></returns>
        public async Task Initialize()
        {
            // Loading data from cache. Assumption here is CrmHelper already generates the cache.
            string oAuthUrl = (string)await Util.ReadFromLocal<string>("OAuthUrl.dat");
            // Resource name for OneNote.
            string resourceName = "https://onenote.com";

            authHelper = new AuthHelper(oAuthUrl, resourceName);
        }

        /// <summary>
        /// Generate HttpClient for OneNoteApi
        /// </summary>
        /// <returns></returns>
        private async Task<HttpClient> GetHttpClient()
        {
            // Instantiate HttpClient with compression enabled
            HttpClient client = new HttpClient(new HttpClientHandler() { AutomaticDecompression = System.Net.DecompressionMethods.GZip });

            // Set base address
            client.BaseAddress = new Uri("https://www.onenote.com/api/v1.0/");
           
            // Note: API only supports JSON return type.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Not adding the Authentication header would produce an unauthorized call and the API will return a 401
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                await Authenticate());
                        
            return client;
        }

        /// <summary>
        /// Obtain AccessToken
        /// </summary>
        /// <returns>AccessToken string</returns>
        private async Task<string> Authenticate()
        {
            // Make sure AccessToken is valid.
            await authHelper.GetTokenSilent();

            // Wait until AccessToken assigned
            while (String.IsNullOrEmpty(authHelper.AccessToken))
            {
                await System.Threading.Tasks.Task.Delay(10);
            }

            return authHelper.AccessToken;
        }

        /// <summary>
        /// Create Page with Business Card. 
        /// </summary>
        /// <param name="imageName">Name of captured Business Card image.</param>
        /// <param name="image">Captured Image Stream.</param>
        /// <returns>Page Uri for created OneNote page.</returns>
        public async Task<string> CreatePageWithBusinessCardImage(string imageName, Stream image)
        {
            using (HttpClient client = await GetHttpClient())
            {
                string pageUri = "";

                string simpleHtml = "<html>" +
                                    "<head>" +
                                    "<title>Extracted Business Card</title>" +
                                    "<meta name=\"created\" content=\"data\" />" +
                                    "</head>" +
                                    "<body>" +
                                    "<h1>Extracted Business Card page</h1>" +
                                    "<div data-render-method=\"extract.businesscard\" data-render-src=\"name:" + imageName + "\" />" +
                                    "</body>" +
                                    "</html>";

                // Create the image part - make sure it is disposed after we've sent the message in order to close the stream.
                using (StreamContent imageContent = new StreamContent(image))
                {
                    imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                   
                    HttpResponseMessage response = await client.PostAsync("me/notes/pages", 
                        new MultipartFormDataContent
					    {
						    {new StringContent(simpleHtml, Encoding.UTF8, "text/html"), "Presentation"},
						    {imageContent, imageName}
					    });

                    // Must send the request within the using block, or the image stream will have been disposed.
                    //HttpResponseMessage response = await client.SendAsync(createMessage);

                    // Create result depending on http status
                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        // Read the response to string.
                        string body = await response.Content.ReadAsStringAsync();
                        ApiBaseResponse apiBaseResponse = JsonConvert.DeserializeObject<PageResponse>(body);
                        if (!string.IsNullOrEmpty(apiBaseResponse.ContentUrl))
                            pageUri = apiBaseResponse.ContentUrl;
                    }
                }

                return pageUri;
            }
        }

        /// <summary>
        /// Return OneNote page as HTML string.
        /// </summary>
        /// <param name="pageUri">Page Uri.</param>
        /// <returns>Page Body. Returns String.Empty if page not available.</returns>
        public async Task<string> GetPage(string pageUri)
        {
            using (HttpClient client = await GetHttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(pageUri);

                string body = await response.Content.ReadAsStringAsync();

                // If returned result contains error, then clear the body.
                if (body.Contains("The specified resource ID does not exist"))
                    body = String.Empty;

                return body;
            }
        }

        /// <summary>
        /// Return VCard generated by Business Card extract method if page contains it
        /// </summary>
        /// <param name="pageUri"></param>
        /// <returns>VCF strings</returns>
        public async Task<string> GetVCard(string cardUri)
        {
            using (HttpClient client = await GetHttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(cardUri);
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        /// <summary>
        /// Return OneNote page as HTML string.
        /// </summary>
        /// <param name="pageUri">Page Uri.</param>
        /// <returns>Page Body. Returns String.Empty if page not available.</returns>
        public async Task DeletePage(string pageUri)
        {
            using (HttpClient client = await GetHttpClient())
            {
                string pageIdUri = pageUri.Replace("/content","");
                HttpResponseMessage response = await client.DeleteAsync(pageIdUri);

                await response.Content.ReadAsStringAsync();               
            }
        }
    }
}
