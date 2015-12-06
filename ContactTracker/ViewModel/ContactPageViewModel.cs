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
using MyTypes;
using System;

namespace ContactTracker.ViewModel
{
    public class ContactPageViewModel : ViewModelBase
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

        #endregion

        #region Method

        public ContactPageViewModel()
        {            
        }

        /// <summary>
        /// Retrieve a contact for the page
        /// </summary>
        /// <param name="id">Contact Id</param>
        private async void RetrieveContact(Guid id)
        {
            IsLoading = true;

            Contact = await CrmHelper.RetrieveContact(id);

            IsLoading = false;
        }

        #endregion

        #region Command

        /// <summary>
        /// Go to ContactEid Page
        /// </summary>
        public RelayCommand EditCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    App.rootFrame.Navigate(typeof(ContactEdit), contact.Id);
                });
            }
        }

        #endregion
    }
}
