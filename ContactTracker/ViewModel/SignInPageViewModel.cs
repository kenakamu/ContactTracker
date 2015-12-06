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
using ContactTracker.View;
using Microsoft.Crm.Mobile.Samples;
using System;
using Windows.UI.Xaml.Controls;

namespace ContactTracker.ViewModel
{
    public class SignInPageViewModel : ViewModelBase
    {
        #region Property

        private string serviceUrl;
        public string ServiceUrl
        {
            get
            {
                return serviceUrl;
            }
            set
            {
                serviceUrl = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region Method

        private async void SignIn()
        {
            IsLoading = true;

            // When sign in to different org, then clear cache.
            if (!String.IsNullOrEmpty(CrmHelper._proxy.ServiceUrl) &&
                ServiceUrl != CrmHelper._proxy.ServiceUrl)
            {
                await CrmHelper.Clear();
            }

            // Check if ServiceUrl is valid.
            string OAuthUrl = await CrmHelper.DiscoveryAuthority(ServiceUrl);
            if (String.IsNullOrEmpty(OAuthUrl))
            {
                // If not, return to page.
                IsLoading = false;
                ServiceUrl = "";
                return;
            }

            // Then save values
            await Util.SaveToLocal(ServiceUrl, "ServiceUrl.dat");
            await Util.SaveToLocal(OAuthUrl, "OAuthUrl.dat");
            // Initialize proxy
            await CrmHelper._proxy.Initialize();

            string username;

            try
            {
                // Now get current user
                username = await CrmHelper.RetrieveUser();
            }
            catch (Exception ex)
            {
                Util.DisplayError(ex);
                IsLoading = false;
                return;
            }

            // Show welcome message
            await Util.DisplayMessage(String.Format("Welcome {0}", username), "Welcome!");

            // Go back to MainPage 
            App.rootFrame.Navigate(typeof(MainPage));

            IsLoading = false;
        }

        #endregion

        #region Command

        /// <summary>
        /// CheckIn Command
        /// </summary>
        public RelayCommand SubmitCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    SignIn();
                });
            }
        }

        /// <summary>
        /// Invoked when ComboBox selection changed
        /// </summary>
        public RelayCommandEx<SelectionChangedEventArgs> SelectionChangedCommand
        {
            get
            {
                return new RelayCommandEx<SelectionChangedEventArgs>((e) =>
                {
                    if (e.AddedItems.Count != 1)
                        return;

                    ServiceUrl = (e.AddedItems[0] as ComboBoxItem).DataContext as string;
                });
            }
        }

        #endregion
    }
}
