using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using BingWallpaper.Resources;
using BingWallpaper.ViewModel;
//using vservWindowsPhone;

namespace BingWallpaper
{
    public partial class MainPage : PhoneApplicationPage
    {
        private ViewModel.MainViewModel _viewModel;
        int _offsetKnob = 8;
        int _pageNumber = 1;
        //VservAdControl VAC;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        //void longlist_ItemRealized(object sender, ItemRealizationEventArgs e)
        //{
        //    if (!_viewModel.IsPhotosLoading && longlist.ItemsSource != null && longlist.ItemsSource.Count >= _offsetKnob)
        //    {
        //        if (e.ItemKind == LongListSelectorItemKind.Item)
        //        {
        //            Model.Image tempImage = e.Container.Content as Model.Image;
        //            Model.Image temp1Image = longlist.ItemsSource[longlist.ItemsSource.Count - _offsetKnob] as Model.Image;
        //            if(tempImage !=null && temp1Image != null && tempImage.url == temp1Image.url)
        //            {
        //                _viewModel.ReadImagesFromInternet(++_pageNumber);
        //            }
        //            //if ((e.Container.Content as Model.Image).Equals(longlist.ItemsSource[longlist.ItemsSource.Count - _offsetKnob]))
        //            //{                        
        //            //    _viewModel.ReadImagesFromInternet(_pageNumber++);
        //            //}
        //        }
        //    } 
        //}

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}