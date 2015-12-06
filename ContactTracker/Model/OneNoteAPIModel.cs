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

using System.Collections.Generic;
using System.Net;

namespace ContactTracker.Model
{
    public class ApiBaseResponse
    {
        /// <summary>
        /// All OneNote API reponses return a meaningful Http status code
        /// Typical pattern for Http status codes are used: 
        /// 1 1xx Informational
        /// 2 2xx Success. e.g. 200-OK for GETs, 201 -Created for POSTs
        /// 3 3xx Redirection
        /// 4 4xx Client Error e.g. 400-Bad Request
        /// 5 5xx Server Error e.g. 500-Internal Server Error
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Per call identifier that can be logged to diagnose issues with Microsoft support
        /// CorrelationId is included in all Response Headers
        /// </summary>
        public string CorrelationId { get; set; }

        /// <summary>
        /// Body of the OneNote API response represented as a string.
        /// For error cases, this will typically include an error json intended for developers, not for end users.
        /// For success cases, depending on the type API call/HTTP verb this may or may not include a json value
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// URLs to launch OneNote rich client/web app
        /// </summary>
        public Links Links { get; set; }

        /// <summary>
        /// Unique identifier of the object
        /// </summary>
        public string Id { get; set; }
        public string ContentUrl { get; set; }
        public string ThumbnailUrl { get; set; }

    }

    public class Links
    {
        /// <summary>
        /// URL to launch OneNote rich client
        /// </summary>
        public HrefUrl OneNoteClientUrl { get; set; }

        /// <summary>
        /// URL to launch OneNote web experience
        /// </summary>
        public HrefUrl OneNoteWebUrl { get; set; }
    }

    public class HrefUrl
    {
        public string Href { get; set; }
    }

    /// <summary>
    /// This class represents a generic the OneNote API entity response
    /// Any response from the Notebooks/Sections/SectionGroups API (POST/GET etc) can be translated into this object for ease of use.
    /// </summary>
    /// <remarks>
    /// This is not meant to be a comprehensive SDK or data model.
    /// This is ONLY a light-weight representation of a OneNote API's entities (Notebooks, Sections, SectionGroups)
    /// The API's HTTP json response is deserialized into this object
    /// </remarks>
    public class GenericEntityResponse : ApiBaseResponse
    {
        /// <summary>
        /// Name of the entity
        /// </summary>
        public string Name;

        /// <summary>
        /// Self link to the given entity
        /// </summary>
        public string Self { get; set; }

        public List<GenericEntityResponse> Sections { get; set; }

        public List<GenericEntityResponse> SectionGroups { get; set; }

        public override string ToString()
        {
            return "Id: " + Id + ", Name: " + Name;
        }
    }

    /// <summary>
    /// This class represents the OneNote API Pages response
    /// Any response from the Pages API (POST/GET etc) can be translated into this object for ease of use.
    /// </summary>
    /// <remarks>
    /// This is not meant to be a comprehensive SDK or data model.
    /// This is ONLY a light-weight representation of a OneNote API's page response.
    /// The API's HTTP json response is deserialized into this object
    /// </remarks>
    public class PageResponse : ApiBaseResponse
    {
        /// <summary>
        /// Title of the page
        /// </summary>
        public string Title;

        public override string ToString()
        {
            return "Id: " + Id + ", Title: " + Title;
        }
    }
}
