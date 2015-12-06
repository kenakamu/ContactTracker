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

using System;
using System.IO;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace ContactTracker.CustomControl
{
    /// <summary>
    /// EntityImage Control to display Entity Image
    /// </summary>
    public sealed partial class EntityImageControl : UserControl
    {
        public EntityImageControl()
        {
            this.InitializeComponent();
            SetImage(null);
        }

        private byte[] imageBytes;
        public byte[] ImageBytes
        {
            set
            {
                imageBytes = value;
                SetImage(value);
            }
        }

        // Expose ImageBytes as Property
        public static DependencyProperty ImageBytesProperty = DependencyProperty.Register("ImageBytes",
            typeof(byte[]), typeof(EntityImageControl), new PropertyMetadata(null, ImageBytesChangedCallback));

        /// <summary>
        /// Invoked when ImageBytes property is set
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void ImageBytesChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EntityImageControl entityImage = (EntityImageControl)d;
            entityImage.ImageBytes = e.NewValue as byte[];
        }

        /// <summary>
        /// This method set ImageSource to image.
        /// </summary>
        /// <param name="imageBytes"></param>
        private async void SetImage(byte[] imageBytes)
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
                im = new BitmapImage(new Uri("ms-appx:///Assets/icon-contact.png"));

            image.Source = im;
        }
    }
}
