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

using MyTypes;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;

namespace Microsoft.Crm.Mobile.Samples
{
    public class Util
    {
        #region Caching data locally

        // current application folder
        static private StorageFolder localFolder = ApplicationData.Current.LocalFolder;

        /// <summary>
        /// Cache data locally
        /// </summary>
        /// <param name="content">object to be cached</param>
        /// <param name="filename">file name for cache</param>
        /// <returns></returns>
        static public async Task SaveToLocal(object content, string filename)
        {
            if (string.IsNullOrEmpty(filename))
                return;
            StorageFile File = await localFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);

            if (content.GetType().Equals(typeof(string)))
            {
                await FileIO.WriteTextAsync(File, content.ToString());
                return;
            }

            string json = JsonConvert.SerializeObject(content, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Full
            });

            try
            {
                using (StreamWriter writer = new StreamWriter(await File.OpenStreamForWriteAsync()))
                {
                    await writer.WriteAsync(json);
                }
            }

            catch (Exception ex)
            {
                Util.DisplayError(ex);
            }

        }

        /// <summary>
        /// Restore from cached data
        /// </summary>
        /// <typeparam name="T">object type of the cached data</typeparam>
        /// <param name="filename">cached file name</param>
        /// <returns></returns>
        static public async Task<object> ReadFromLocal<T>(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                return default(T);
            T result = default(T);

            StorageFile file;

            var data = await localFolder.GetFilesAsync();
            try
            {
                file = await localFolder.GetFileAsync(filename);
            }
            catch (Exception ex)
            {
                return default(T);
            }

            if (typeof(T) == typeof(string))
            {
                return await FileIO.ReadTextAsync(file);
            }

            try
            {
                result = JsonConvert.DeserializeObject<T>(await FileIO.ReadTextAsync(file), new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                    TypeNameAssemblyFormat = FormatterAssemblyStyle.Full
                });
            }

            catch (Exception ex)
            {
                return default(T);
            }

            return result;
        }

        /// <summary>
        /// Delete local cache
        /// </summary>
        /// <param name="filename">Specify file name to be deleted. If no file name specified, 
        /// then delete all cache.</param>
        /// <returns></returns>
        static public async Task DeleteLocalCache(string filename = null)
        {
            // If file name specified
            if (!string.IsNullOrEmpty(filename))
            {
                // delete specified file
                StorageFile file = await localFolder.GetFileAsync(filename);
                await file.DeleteAsync();

                return;
            }
            // otherwise, delete all

            var files = await localFolder.GetFilesAsync();
            foreach (StorageFile file in files)
            {
                await file.DeleteAsync();
            }
        }

        #endregion

        #region Show messages

        /// <summary>
        /// This method displays  message.
        /// </summary>
        /// <param name="ex"></param>
        static public async Task DisplayMessage(string message, string title)
        {
            MessageDialog dialog;

            dialog = new MessageDialog(message, title);

            await dialog.ShowAsync();
        }

        /// <summary>
        /// This method displays error message.
        /// </summary>
        /// <param name="ex"></param>
        static public async Task DisplayError(Exception ex)
        {
            MessageDialog dialog;

            // Get the deepest error message as it makes more sense.
            while (ex.InnerException != null)
                ex = ex.InnerException;

            dialog = new MessageDialog(String.Format("Error Message:{0}, Source:{1}", ex.Message, ex.Source), "Error");

            await dialog.ShowAsync();
        }

        #endregion

        #region Extract VCard

        static public Contact ExtractVCart(Contact contact, string vcard)
        {
            string[] extractedResults = vcard.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            // Extrace each element and update fields
            if (extractedResults.Where(x => x.StartsWith("FN:")).FirstOrDefault() != null)
            {
                string fullname = extractedResults.Where(x => x.StartsWith("FN:")).First().Replace("FN:", "");
                contact.FirstName = fullname.Split(' ')[0];
                contact.LastName = fullname.Split(' ')[fullname.Split(' ').Count() - 1];
            }
            if (extractedResults.Where(x => x.StartsWith("TITLE:")).FirstOrDefault() != null)
                contact.JobTitle = extractedResults.Where(x => x.StartsWith("TITLE:")).First().Replace("TITLE:", "");
            if (extractedResults.Where(x => x.Contains("VOICE:")).FirstOrDefault() != null)
            {
                string telephone = extractedResults.Where(x => x.Contains("VOICE:")).First();
                contact.Telephone1 = telephone.Substring(telephone.IndexOf(":") + 1);
            }
            if (extractedResults.Where(x => x.StartsWith("EMAIL;")).FirstOrDefault() != null)
            {
                string email = extractedResults.Where(x => x.StartsWith("EMAIL;")).First();
                contact.EMailAddress1 = email.Substring(email.IndexOf(":") + 1);
            }
            if (extractedResults.Where(x => x.StartsWith("URL:")).FirstOrDefault() != null)
                contact.WebSiteUrl = extractedResults.Where(x => x.StartsWith("URL:")).First().Replace("URL:", "");
            contact.Description = vcard;

            return contact;
        }

        #endregion
    }
}
