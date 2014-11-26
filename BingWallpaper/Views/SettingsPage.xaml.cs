using BingWallpaper.ViewModel;
using Microsoft.Phone.Controls;
using System.Windows;
using System.Windows.Controls;

namespace BingWallpapers.Views
{   
    public partial class SettingsPage
    {
        
        #region Constructor
        public SettingsPage()
        {
            InitializeComponent();  
            if(listPickerResolution != null)
            {
                MainViewModel viewModel = DataContext as MainViewModel;
                if(viewModel != null && viewModel.Settings != null)
                {
                    if(viewModel.Settings.SelectedResolution == "480 x 800")
                    {
                        listPickerResolution.SelectedIndex = 0;
                    }
                    else if(viewModel.Settings.SelectedResolution == "720 x 1280")
                    {
                        listPickerResolution.SelectedIndex = 1;
                    }
                    else if(viewModel.Settings.SelectedResolution == "768 x 1280")
                    {
                        listPickerResolution.SelectedIndex = 2;
                    }
                    else if(viewModel.Settings.SelectedResolution == "1080 x 1920")
                    {
                        listPickerResolution.SelectedIndex = 3;
                    }
                    else
                    {
                        listPickerResolution.SelectedIndex = 0;
                    }
                }
            }
        }
        #endregion
        
        private void listPickerResolution_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MainViewModel viewModel = DataContext as MainViewModel;
            if (listPickerResolution != null)
            {
                if (listPickerResolution.SelectedIndex == 0)
                {
                    viewModel.Settings.SelectedResolution = "480 x 800";
                }
                else if (listPickerResolution.SelectedIndex == 1)
                {
                    viewModel.Settings.SelectedResolution = "720 x 1280";
                }
                else if (listPickerResolution.SelectedIndex == 2)
                {
                    viewModel.Settings.SelectedResolution = "768 x 1280";
                }
                else if (listPickerResolution.SelectedIndex == 3)
                {
                    viewModel.Settings.SelectedResolution = "1080 x 1920";
                }
            }
        }
    }     
}