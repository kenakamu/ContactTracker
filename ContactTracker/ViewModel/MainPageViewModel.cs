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
using ContactTracker.Common;
using Microsoft.Crm.Mobile.Samples;
using System.Collections.ObjectModel;
using ContactTracker.View;

namespace ContactTracker.ViewModel
{
    public class MainPageViewModel : ViewModelBase
    {
        #region Property

        // Search Criteria for Contact List
        private string searchCriteria;
        public string SearchCriteria
        {
            get
            {
                return searchCriteria;
            }
            set
            {
                if (searchCriteria == value)
                    return;

                searchCriteria = value;
                this.NotifyPropertyChanged();
                LoadData(searchCriteria);
            }
        }        

        private ObservableCollection<Contact> contacts;
        public ObservableCollection<Contact> Contacts
        {
            get
            {
                return contacts;
            }
            set
            {
                contacts = value;
            }
        }

        #endregion

        #region Method

        public MainPageViewModel()
        {
            Contacts = new ObservableCollection<Contact>();
            LoadData();
        }

        /// <summary>
        /// Seach Contacts and load data for the view.
        /// </summary>
        /// <param name="searchCriteria">Search Criteria</param>
        private async void LoadData(string searchCriteria = "")
        {
            IsLoading = true;

            ObservableCollection<Contact> results = await CrmHelper.RetrieveContacts(searchCriteria);

            Contacts.Clear();
            
            foreach (Contact result in results)
            {
                Contacts.Add(result);
            }

            if (results.Count > 50)
            {
                await Util.DisplayMessage("More than 50 contacts returned. Use filter to find records.", "Information");
            }

            IsLoading = false;
        }

        #endregion

        #region Command

        /// <summary>
        /// Invoked when item clicked in ListView
        /// </summary>
        public RelayCommandEx<Contact> ItemClickCommand
        {
            get
            {
                return new RelayCommandEx<Contact>((item) =>
                {
                    App.rootFrame.Navigate(typeof(ContactPage), item.Id);
                });
            }
        }

        /// <summary>
        /// Go to Settings Page
        /// </summary>
        public RelayCommand GoSettingsCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    App.rootFrame.Navigate(typeof(SignInPage));
                });
            }
        }

        /// <summary>
        /// Go to ContactEid Page
        /// </summary>
        public RelayCommand AddCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    App.rootFrame.Navigate(typeof(ContactEdit));
                });
            }
        }

        #endregion
    }
}
