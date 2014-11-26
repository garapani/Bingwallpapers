using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using BingWallpaper.ViewModel;

namespace BingWallpaper
{
    public partial class ImagePage : PhoneApplicationPage
    {
        public ImagePage()
        {
            InitializeComponent();        
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MainViewModel mainViewModel = DataContext as MainViewModel;
            if(mainViewModel != null)
            {
                if (mainViewModel.UserSelectedRegion != null)
                {
                    pivotControl.ItemsSource = mainViewModel.SelectedRegionImages;
                }
                else
                {
                    pivotControl.ItemsSource = mainViewModel.Images;
                }
            }
            base.OnNavigatedTo(e);
        }
    }
}