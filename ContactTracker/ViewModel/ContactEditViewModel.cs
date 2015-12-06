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

using ContactTracker.Common;
using ContactTracker.Helper;
using Microsoft.Crm.Mobile.Samples;
using MyTypes;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace ContactTracker.ViewModel
{
    public class ContactEditViewModel : ViewModelBase
    {
        #region Property

        private Guid id;
        public Guid Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                RetrieveContact(value);                
            }
        }

        private Contact contact;
        public Contact Contact
        {
            get
            {
                return contact;
            }
            set
            {
                contact = value;
                NotifyPropertyChanged();
            }
        }

        private string message;
        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
                NotifyPropertyChanged();
            }
        }

        private ImageSource source;
        public ImageSource Source
        {
            get
            {
                return source;
            }
            set
            {
                source = value;
                NotifyPropertyChanged();
            }
        }

        private OneNoteHelper oneNoteHelper;

        #endregion

        #region Method

        public ContactEditViewModel()
        {
            oneNoteHelper = new OneNoteHelper();
            // Initialize OneNoteHelper.
            oneNoteHelper.Initialize();
            // Create blank Contact Object.
            Contact = new Contact();
            // Display default picture.
            ProcessPicture(file: null);
        }

        /// <summary>
        /// Retrieve a contact for the page.
        /// </summary>
        /// <param name="id">Contact Id</param>
        private async void RetrieveContact(Guid id)
        {
            IsLoading = true;

            Contact = await CrmHelper.RetrieveContact(id);

            ProcessPicture(Contact.EntityImage);

            IsLoading = false;
        }

        /// <summary>
        /// Extract Business Card information
        /// </summary>
        /// <param name="picture">Business Card Image</param>
        public async void ProcessCard(StorageFile picture)
        {
            IsLoading = true;

            Message = "Upload Business Card to OneNote...";

            IRandomAccessStream readStream = await picture.OpenAsync(FileAccessMode.Read);
            Stream stream = readStream.AsStreamForRead();

            // Create Business Card
            string pageUri = await oneNoteHelper.CreatePageWithBusinessCardImage(picture.Name, stream);

            if (String.IsNullOrEmpty(pageUri))
            {
                await Util.DisplayMessage("Failed to create OneNote page with Business Card", "Warning");
                IsLoading = false;
                return;
            }

            Message = "Extracing Business Card...";

            string pageBody = String.Empty;

            // As it may take a bit of time until actual page generated, retry until it gets the result.
            while (String.IsNullOrEmpty(pageBody))
            {
                await Task.Delay(500);
                pageBody = await oneNoteHelper.GetPage(pageUri);
            }
                        
            // Extract VCard uri from result.
            XDocument doc = XDocument.Parse(pageBody);

            // Check if vcard is generated.
            XAttribute typeAttribute = doc.Descendants().Attributes("type").Where(x => x.Value == "text/vcard").FirstOrDefault();
            if (typeAttribute == null)
            {
                await Util.DisplayMessage("Failed to extract Business Card", "Warning");
                Message = "Deleting the OneNote Page...";
                await oneNoteHelper.DeletePage(pageUri);
                IsLoading = false;
                return;
            }

            // Get CardUri
            string cardUri = typeAttribute.Parent.Attribute("data").Value;

            // Get VCard
            string vcf = await oneNoteHelper.GetVCard(cardUri);
            
            // Once VCard obtained, delete the OneNote page.
            await oneNoteHelper.DeletePage(pageUri);

            if (String.IsNullOrEmpty(vcf))
            {
                await Util.DisplayMessage("Failed to download VCard", "Message"); ;              
                IsLoading = false;
                return;
            }

            // Extract the retrieved vcf
            Contact = Util.ExtractVCart(contact, vcf);

            IsLoading = false;
        }

        /// <summary>
        /// Process picture and display
        /// </summary>
        /// <param name="file">Picture file</param>
        public async void ProcessPicture(StorageFile file = null)
        {
            BitmapImage im;

            // If imageBytes has data in it, then use it.
            if (file != null)
            {
                IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read);
                im = new BitmapImage();
                await im.SetSourceAsync(fileStream);

                fileStream.Seek(0);
                // Set to contact EntityImage
                byte[] fileBytes = new byte[fileStream.Size];
                using (DataReader reader = new DataReader(fileStream))
                {
                    await reader.LoadAsync((uint)fileStream.Size);

                    reader.ReadBytes(fileBytes);
                }
                contact.EntityImage = fileBytes;
            }
            // Otherwise use default picture.
            else
                im = new BitmapImage(new Uri("ms-appx:///Assets/noimage.PNG"));

            Source = im;
        }

        /// <summary>
        /// This method set ImageSource to image.
        /// </summary>
        /// <param name="imageBytes">Source Image in byte array</param>
        private async void ProcessPicture(byte[] imageBytes)
        {
            BitmapImage im;

            // If imageBytes has data in it, then use it.
            if (imageBytes != null)
            {
                im = new BitmapImage();
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    await im.SetSourceAsync(ms.AsRandomAccessStream());
                }
            }
            // Otherwise use default picture.
            else
                im = new BitmapImage(new Uri("ms-appx:///Assets/noimage.PNG"));

            Source = im;
        }   

        #endregion

        #region Command

        /// <summary>
        /// Save Command
        /// </summary>
        public RelayCommand SaveCommand
        {
            get
            {
                return new RelayCommand(async() =>
                {
                    IsLoading = true;
                    await CrmHelper.UpsertContact(contact);
                    App.rootFrame.GoBack();
                    IsLoading = false;
                });
            }
        }

        /// <summary>
        /// Scan Command
        /// </summary>
        public RelayCommand ScanCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    FileOpenPicker picker = new FileOpenPicker();
                    // Specify extension
                    picker.FileTypeFilter.Add(".jpg");
                    picker.FileTypeFilter.Add(".jpeg");
                    picker.FileTypeFilter.Add(".png");

                    // If this is WP8.1, then use AndContinue model
                    picker.ContinuationData["BusinessCard"] = "BusinessCard";
                    picker.PickSingleFileAndContinue();
                });
            }
        }

        /// <summary>
        /// Picture Command
        /// </summary>
        public RelayCommand PictureCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    FileOpenPicker picker = new FileOpenPicker();
                    // Specify extension
                    picker.FileTypeFilter.Add(".jpg");
                    picker.FileTypeFilter.Add(".jpeg");
                    picker.FileTypeFilter.Add(".png");

                    // If this is WP8.1, then use AndContinue model
                    picker.ContinuationData["EntityImage"] = "EntityImage";
                    picker.PickSingleFileAndContinue();
                });
            }
        }

        #endregion
    }
}
