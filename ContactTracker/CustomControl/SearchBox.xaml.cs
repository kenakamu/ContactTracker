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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace ContactTracker.CustomControl
{
    /// <summary>
    /// SearchBox custom Control
    /// </summary>

    public sealed partial class SearchBox : UserControl
    {
        // Exposed property
        private string searchCriteria;
        public string SearchCriteria
        {
            get
            {
                return GetValue(SearchCriteriaProperty).ToString();
            }
            set
            {
                SetValue(SearchCriteriaProperty, value);
            }
        }

        public static readonly DependencyProperty SearchCriteriaProperty =
            DependencyProperty.Register("SearchCriteria", typeof(string), typeof(SearchBox), new PropertyMetadata(string.Empty));

        // Let Page set searchCommand and Do serach (for voice command)
        public string voiceCommandSearchCriteria
        {
            set
            {
                if (String.IsNullOrEmpty(value))
                    return;
                txtSearchCriteria.Text = value;
                PreSearch();
            }
        }

        public SearchBox()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when search button clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            PreSearch();
        }

        /// <summary>
        /// Invoked everytime user types with keyboard.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSearchCriteria_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            // If user hits Enter, asusme user wants to do Search.
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                PreSearch();
            }
            else
            {
                // If user changes search criteria, then reset button
                if (searchCriteria != txtSearchCriteria.Text)
                {
                    btnClear.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    btnSearch.Visibility = Windows.UI.Xaml.Visibility.Visible;
                }
            }
        }

        /// <summary>
        /// Does work before actual Search called.
        /// </summary>
        private void PreSearch()
        {
            btnSearch.Focus(FocusState.Programmatic);
            // If no search criteria, do nothing.
            if (String.IsNullOrEmpty(txtSearchCriteria.Text))
                return;

            // Swap buttons.
            btnSearch.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            btnClear.Visibility = Windows.UI.Xaml.Visibility.Visible;


            // If user uses wildcard (*), replace to %.
            // By assign value to SearchCriteria, it triggeres dependency action.
            SearchCriteria = txtSearchCriteria.Text.Replace('*', '%');
        }

        /// <summary>
        /// Invoked when user clear search criteria by tap clear button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        /// <summary>
        /// Exposed method. Reset the control
        /// </summary>
        public void Reset()
        {
            // Clear search criteria and reset button.
            txtSearchCriteria.Text = SearchCriteria = "";
            btnClear.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            btnSearch.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }
    }
}
