using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace TheHindu.Views
{
    public partial class AboutUs : PhoneApplicationPage
    {
        //private NavigationService _navigationService;
        public AboutUs()
        {
            //_navigationService = navigateService;
            InitializeComponent();
        }

        private void Rate_Click(object sender, RoutedEventArgs e)
        {
            var mp = new MarketplaceReviewTask();
            mp.Show();
        }

        private void Share_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/SharePage.xaml", UriKind.Relative));
        }
        
        private void Feedback_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EmailComposeTask emailComposeTask = new EmailComposeTask
                {
                    To = "venkatachalapathi.g@outlook.com",
                    Subject = "Feedback",
                };
                emailComposeTask.Show();
            }
            catch
            {
            }
        }

    }
}