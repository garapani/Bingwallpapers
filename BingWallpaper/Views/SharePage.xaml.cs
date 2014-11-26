using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using BingWallapapers.Helper;

namespace TheHindu
{
    public partial class SharePage : PhoneApplicationPage
    {
        public SharePage()
        {
            InitializeComponent();
        }

        private void ShareViaSocialNetwork_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ShareLinkTask shareLinkTask = new ShareLinkTask();
            shareLinkTask.Title = "DailyWallpapers";
            var storeURI = DeepLinkHelper.BuildApplicationDeepLink();
            //await Windows.System.Launcher.LaunchUriAsync(storeURI);

            //shareLinkTask.LinkUri = new Uri("http://msdn.microsoft.com/en-us/library/windowsphone/develop/ff431744(v=vs.92).aspx", UriKind.Absolute);
            shareLinkTask.LinkUri = new Uri(storeURI);
            shareLinkTask.Message = "\"DailyWallpapers\" App for Windows phone 8";
            shareLinkTask.Show();
        }

        private void ShareViaMail_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var storeURI = DeepLinkHelper.BuildApplicationDeepLink();
            EmailComposeTask emailComposeTask = new EmailComposeTask()
            {
                Subject = "Try \"DailyWallpapers\" App for windows phone 8",
                Body = "\"DailyWallpapers\" App provides download facility from bing images" + storeURI,
            };

            emailComposeTask.Show();
        }

        private void ShareViaSMS_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var storeURI = DeepLinkHelper.BuildApplicationDeepLink();
            SmsComposeTask smsComposeTask = new SmsComposeTask()
            {
                Body = "Try \"DailyWallpapers\" App for windows phone 8. It's great!. " + storeURI,
            };

            smsComposeTask.Show();
        }
    }
}