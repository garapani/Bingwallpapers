using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Net.NetworkInformation;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework.Media;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using Windows.Phone.System.UserProfile;
using System.Linq;
using Microsoft.Phone.Scheduler;
using BingWallpaper.Model;
using System.Net.Http;

namespace BingWallpaper.ViewModel
{
    public class Settings
    {
        public string SelectedResolution { get; set; }
        public Settings()
        {
            SelectedResolution = "480 x 800";
        }
    }

    public class RegionData
    {
        public string RegionName { get; set; }
        public string RegionUrl { get; set; }
        public RegionData(string regionName, string regionUrl)
        {
            RegionName = regionName;
            RegionUrl = regionUrl;
        }
        public RegionData()
        {

        }
    }

    public class MainViewModel : ViewModelBase
    {
        #region Properties and variables

        private const string liveTileImagePath = "Shared/ShellContent/" + "liveTile.jpg";
        private const string bingImageUrl = "http://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1";

        PhoneApplicationFrame rootFrame;
        public List<string> ListOfStrings;
        private Image _selectedImage;
        public Image SelectedImage
        {
            get
            {
                return _selectedImage;
            }
            set
            {
                _selectedImage = value;
                RaisePropertyChanged("SelectedImage");
            }
        }

        private ObservableCollection<Image> _images;
        public ObservableCollection<Image> Images
        {
            get
            {
                return _images;
            }
            set
            {
                _images = value;
                RaisePropertyChanged("Images");
            }
        }

        private ObservableCollection<Image> _selectedRegionImages;
        public ObservableCollection<Image> SelectedRegionImages
        {
            get
            {
                return _selectedRegionImages;
            }
            set
            {
                _selectedRegionImages = value;
                RaisePropertyChanged("SelectedRegionImages");
            }
        }
        private List<RegionData> _listOfRegionData;
        public List<RegionData> ListOfRegionData
        {
            get
            {
                return _listOfRegionData;
            }
            set
            {
                _listOfRegionData = value;
                RaisePropertyChanged("ListOfRegionData");
            }
        }

        public Settings Settings { get; private set; }
        public bool IsNetworkAvailable { get; private set; }
        public bool IsPhotosLoading { get; private set; }
        private RegionData _userSelectedRegion;
        public RegionData UserSelectedRegion { get; private set; }
        private RegionData _userPreviouslySelectedRegion;
        #endregion Properties and variables

        #region RelayCommands

        public RelayCommand ReadSelectedImageCommand { get; private set; }
        public RelayCommand NextImageCommand { get; private set; }
        public RelayCommand PreviousImageCommand { get; private set; }
        public RelayCommand ShowAboutCommand { get; private set; }
        public RelayCommand ShowShareTheAppCommand { get; private set; }
        public RelayCommand ShowRateTheAppCommand { get; private set; }
        public RelayCommand ShowSettingsCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand ReadSelectedRegion { get; private set; }
        public RelayCommand LoadedRegionPageCommand { get; private set; }
        public RelayCommand LoadedMainPageCommand { get; private set; }
        public RelayCommand SetAsWallpaperCommand { get; private set; }

        #endregion RelayCommands

        public MainViewModel()
        {
            rootFrame = App.RootFrame;

            LoadSettings();
            if (_images == null)
            {
                _images = new ObservableCollection<Image>();
            }

            if (_selectedRegionImages == null)
            {
                _selectedRegionImages = new ObservableCollection<Image>();
            }

            if (_selectedImage == null)
                _selectedImage = new Image();

            if (_listOfRegionData == null)
            {
                _listOfRegionData = new List<RegionData>();
            }
            if (_userPreviouslySelectedRegion != null)
            {
                _userPreviouslySelectedRegion = new RegionData();
            }

            if (_userSelectedRegion == null)
            {
                _userSelectedRegion = new RegionData();
            }

            FillRegionData();
            LoadImages();
            InitializeReplayCommands();
        }

        private void FillRegionData()
        {
            if (_listOfRegionData != null)
            {
                _listOfRegionData.Add(new RegionData("World Wide", "http://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=15&mkt=en-WW-HD"));
                _listOfRegionData.Add(new RegionData("USA", "http://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=15&mkt=en-US"));
                _listOfRegionData.Add(new RegionData("India", "http://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=15&mkt=en-IN"));
                _listOfRegionData.Add(new RegionData("Canada", "http://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=15&mkt=en-CA"));
                _listOfRegionData.Add(new RegionData("UK", "http://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=15&mkt=en-UK"));
                _listOfRegionData.Add(new RegionData("Australia", "http://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=15&mkt=en-AU"));
                _listOfRegionData.Add(new RegionData("France", "http://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=15&mkt=fr-FR"));
                _listOfRegionData.Add(new RegionData("China", "http://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=15&mkt=zh-CN"));
                _listOfRegionData.Add(new RegionData("Japan", "http://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=15&mkt=ja-JP"));
                _listOfRegionData.Add(new RegionData("Germany", "http://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=15&mkt=de-DE"));
            }
        }
        private void InitializeReplayCommands()
        {
            ReadSelectedImageCommand = new RelayCommand(ReadImage);
            SaveCommand = new RelayCommand(SaveImage);
            //NextImageCommand = new RelayCommand(NextImage, () => CanMoveToNextImage);
            //PreviousImageCommand = new RelayCommand(PreviousImage, () => CanMoveToPreviousImage);
            ShowAboutCommand = new RelayCommand(ShowAboutPage);
            ShowShareTheAppCommand = new RelayCommand(ShowSharetheAppPage);
            ShowRateTheAppCommand = new RelayCommand(ShowRateTheAppFunction);
            ShowSettingsCommand = new RelayCommand(ShowSettingsPage);
            ReadSelectedRegion = new RelayCommand(ReadRegion);
            LoadedRegionPageCommand = new RelayCommand(LoadRegionPage);
            LoadedMainPageCommand = new RelayCommand(LoadedMainPage);
            SetAsWallpaperCommand = new RelayCommand(SetAsWallPaper);
        }

        private void SetAsWallPaper()
        {
            try
            {
                if (SelectedImage != null && !string.IsNullOrEmpty(SelectedImage.userPreferredurl))
                {
                    if (NetWorkAvailable() == false)
                    {
                        rootFrame.Navigate(new Uri("/Views/NetworkError.xaml", UriKind.Relative));
                        return;
                    }
                    var webClient = new WebClient();
                    webClient.OpenReadCompleted += (object sender, OpenReadCompletedEventArgs e) =>
                    {
                        try
                        {
                            var streamResourceInfo = new StreamResourceInfo(e.Result, null);

                            var userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication();
                            string fileName;
                            Uri presentLockScreenImage = null;
                            try
                            {
                                presentLockScreenImage = LockScreen.GetImageUri();
                            }
                            catch (Exception ex)
                            {
                                presentLockScreenImage = null;
                            }

                            if (presentLockScreenImage == null)
                            {
                                fileName = "TempJPEG_A.jpg";
                            }
                            else
                            {
                                if (presentLockScreenImage.ToString().EndsWith("_A.jpg"))
                                {
                                    fileName = "TempJPEG_B.jpg";
                                }
                                else
                                {
                                    fileName = "TempJPEG_A.jpg";
                                }
                            }
                            var isolatedStorageFileStream = userStoreForApplication.CreateFile(fileName);

                            var bitmapImage = new BitmapImage { CreateOptions = BitmapCreateOptions.None };
                            bitmapImage.SetSource(streamResourceInfo.Stream);

                            var writeableBitmap = new WriteableBitmap(bitmapImage);
                            writeableBitmap.SaveJpeg(isolatedStorageFileStream, writeableBitmap.PixelWidth, writeableBitmap.PixelHeight, 0, 85);
                            var listOfFiles = userStoreForApplication.GetFileNames();

                            isolatedStorageFileStream.Close();

                            DispatcherHelper.UIDispatcher.BeginInvoke(async () =>
                            {
                                if (!LockScreenManager.IsProvidedByCurrentApplication)
                                    await LockScreenManager.RequestAccessAsync();
                                if (LockScreenManager.IsProvidedByCurrentApplication)
                                {
                                    var uri = new Uri("ms-appdata:///local/" + fileName, UriKind.Absolute);
                                    LockScreen.SetImageUri(uri);
                                    MessageBox.Show("successfully set the wallpaper");
                                }
                            });
                        }
                        catch (Exception ex)
                        {
                            DispatcherHelper.UIDispatcher.BeginInvoke(
                                () =>
                                {
                                    MessageBox.Show(string.Format("Failed to set wallpaper"));
                                });
                            try
                            {
                                GoogleAnalytics.EasyTracker.GetTracker().SendException(ex.Message, false);
                            }
                            catch (Exception)
                            {

                            }
                        }
                    };

                    webClient.OpenReadAsync(new Uri(SelectedImage.userPreferredurl, UriKind.Absolute));
                }
            }
            catch (Exception e)
            {

            }
        }

        private static bool isLoaded = false;
        private async void LoadedMainPage()
        {
            UserSelectedRegion = null;
            if (isLoaded == false)
            {
                await Task.Run(() => SaveAndSetLiveTile());
                isLoaded = true;
            }
        }

        private void UnloadedMainPage()
        {
            //UserSelectedRegion = null;   
        }

        private void LoadRegionPage()
        {
            //if( UserSelectedRegion != null)
            //{
            //    if (_userPreviouslySelectedRegion == null || _userPreviouslySelectedRegion.RegionName != UserSelectedRegion.RegionName)
            //    {
            //        _userPreviouslySelectedRegion = UserSelectedRegion;
            //        ReadRegionImagesFromInternet(UserSelectedRegion.RegionUrl, 0);
            //    }                
            //}
        }

        private void ReadRegion()
        {
            if (NetWorkAvailable() == false)
            {
                DispatcherHelper.UIDispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show("No Internet Connectivity!");
                });
            }

            if (UserSelectedRegion != null)
            {
                if (_userPreviouslySelectedRegion == null || _userPreviouslySelectedRegion.RegionName != UserSelectedRegion.RegionName)
                {
                    _userPreviouslySelectedRegion = UserSelectedRegion;
                    ReadRegionImagesFromInternet(UserSelectedRegion.RegionUrl, 0);
                }
            }
            rootFrame.Navigate(new Uri("/Views/RegionPage.xaml", UriKind.Relative));
        }

        private void ReadImage()
        {
            string selectedRegion = null;
            if (UserSelectedRegion != null)
            {
                selectedRegion = UserSelectedRegion.RegionName;
            }
            if (SelectedImage != null && !string.IsNullOrEmpty(SelectedImage.url))
            {
                rootFrame.Navigate(new Uri("/Views/ImagePage.xaml", UriKind.Relative));
            }
            //NextImageCommand.RaiseCanExecuteChanged();
            //PreviousImageCommand.RaiseCanExecuteChanged();
        }

        public void Save()
        {
            SaveSettings();
            UpdateAgent();
        }
        public void Load()
        {
            IsNetworkAvailable = NetWorkAvailable();
            LoadSettings();
        }
        private void ShowSettingsPage()
        {
            rootFrame.Navigate(new Uri("/Views/SettingsPage.xaml", UriKind.Relative));
        }

        private void ShowRateTheAppFunction()
        {
            var mp = new MarketplaceReviewTask();
            mp.Show();
        }

        private void ShowSharetheAppPage()
        {
            rootFrame.Navigate(new Uri("/Views/SharePage.xaml", UriKind.Relative));
        }

        private void ShowAboutPage()
        {
            rootFrame.Navigate(new Uri("/Views/AboutPage.xaml", UriKind.Relative));
        }

        private void LoadImages()
        {
            if (Images == null)
            {
                Images = new ObservableCollection<Image>();
            }
            if (Images.Count == 0)
            {
                ReadImagesFromInternet();
            }
        }

        //private void NextImage()
        //{
        //    if (UserSelectedRegion != null)
        //    {
        //        int index = SelectedRegionImages.IndexOf(SelectedImage);
        //        if (index != Images.Count - 1)
        //        {
        //            SelectedImage = SelectedRegionImages[index + 1];
        //            NextImageCommand.RaiseCanExecuteChanged();
        //            PreviousImageCommand.RaiseCanExecuteChanged();
        //        }
        //    }
        //    else
        //    {
        //        int index = Images.IndexOf(SelectedImage);
        //        if (index != Images.Count - 1)
        //        {
        //            SelectedImage = Images[index + 1];
        //            NextImageCommand.RaiseCanExecuteChanged();
        //            PreviousImageCommand.RaiseCanExecuteChanged();
        //        }
        //    }
        //}

        //private void PreviousImage()
        //{
        //    if (UserSelectedRegion == null)
        //    {
        //        int index = Images.IndexOf(SelectedImage);
        //        if (index > 0)
        //        {
        //            SelectedImage = Images[index - 1];
        //            NextImageCommand.RaiseCanExecuteChanged();
        //            PreviousImageCommand.RaiseCanExecuteChanged();
        //        }
        //    }
        //    else
        //    {
        //        int index = SelectedRegionImages.IndexOf(SelectedImage);
        //        if (index > 0)
        //        {
        //            SelectedImage = SelectedRegionImages[index - 1];
        //            NextImageCommand.RaiseCanExecuteChanged();
        //            PreviousImageCommand.RaiseCanExecuteChanged();
        //        }
        //    }
        //}

        //private bool CanMoveToNextImage
        //{
        //    get
        //    {
        //        if (UserSelectedRegion != null)
        //        {
        //            int index = SelectedRegionImages.IndexOf(SelectedImage);
        //            if (index == SelectedRegionImages.Count - 1)
        //            {
        //                return false;
        //            }
        //            else
        //            {
        //                return true;
        //            }
        //        }
        //        else
        //        {
        //            int index = Images.IndexOf(SelectedImage);
        //            if (index == Images.Count - 1)
        //            {
        //                return false;
        //            }
        //            else
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //}

        //private bool CanMoveToPreviousImage
        //{
        //    get
        //    {
        //        if (UserSelectedRegion != null)
        //        {
        //            int index = SelectedRegionImages.IndexOf(SelectedImage);
        //            if (index == 0)
        //            {
        //                return false;
        //            }
        //            else
        //            {
        //                return true;
        //            }
        //        }
        //        else
        //        {
        //            int index = Images.IndexOf(SelectedImage);
        //            if (index == 0)
        //            {
        //                return false;
        //            }
        //            else
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //}

        private bool NetWorkAvailable()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                return true;
            }
            return false;
        }

        private void SaveImage()
        {
            if (NetWorkAvailable() == false)
            {
                rootFrame.Navigate(new Uri("/Views/NetworkError.xaml", UriKind.Relative));
                return;
            }
            var webClient = new WebClient();
            webClient.OpenReadCompleted += (object sender, OpenReadCompletedEventArgs e) =>
            {
                try
                {
                    const string tempJpeg = "TempJPEG.jpg";
                    var streamResourceInfo = new StreamResourceInfo(e.Result, null);

                    var userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication();
                    if (userStoreForApplication.FileExists(tempJpeg))
                    {
                        userStoreForApplication.DeleteFile(tempJpeg);
                    }

                    var isolatedStorageFileStream = userStoreForApplication.CreateFile(tempJpeg);

                    var bitmapImage = new BitmapImage { CreateOptions = BitmapCreateOptions.None };
                    bitmapImage.SetSource(streamResourceInfo.Stream);

                    var writeableBitmap = new WriteableBitmap(bitmapImage);
                    writeableBitmap.SaveJpeg(isolatedStorageFileStream, writeableBitmap.PixelWidth, writeableBitmap.PixelHeight, 0, 85);

                    isolatedStorageFileStream.Close();
                    isolatedStorageFileStream = userStoreForApplication.OpenFile(tempJpeg, FileMode.Open, FileAccess.Read);

                    // Save the image to the camera roll or saved pictures album.
                    var mediaLibrary = new MediaLibrary();
                    if (Settings.SelectedResolution == "480 x 800")
                    {
                        SelectedImage.userPreferredurl = SelectedImage.url480x800;
                    }
                    else if (Settings.SelectedResolution == "720 x 1280")
                    {
                        SelectedImage.userPreferredurl = SelectedImage.url720x1280;
                    }
                    else if (Settings.SelectedResolution == "768 x 1280")
                    {
                        SelectedImage.userPreferredurl = SelectedImage.url768x1280;
                    }
                    else if (Settings.SelectedResolution == "1080 x 1920")
                    {
                        SelectedImage.userPreferredurl = SelectedImage.url1080x1920;
                    }
                    else
                    {
                        SelectedImage.userPreferredurl = SelectedImage.url480x800;
                    }

                    string fileName = Path.GetFileName(SelectedImage.userPreferredurl);
                    // Save the image to the saved pictures album.                  
                    Picture picture = mediaLibrary.SavePicture(fileName, isolatedStorageFileStream);
                    if (picture.Name.Contains(fileName))
                    {
                        DispatcherHelper.UIDispatcher.BeginInvoke(
                       () =>
                       {
                           MessageBox.Show(string.Format("successfully saved the image in picture hub"));
                       });
                    }
                    else
                    {
                        DispatcherHelper.UIDispatcher.BeginInvoke(
                       () =>
                       {
                           MessageBox.Show(string.Format("Failed to save the image"));
                       });
                    }

                    isolatedStorageFileStream.Close();
                }
                catch (Exception ex)
                {
                    DispatcherHelper.UIDispatcher.BeginInvoke(
                        () =>
                        {
                            MessageBox.Show(string.Format("Failed to save the image"));
                        });
                }
            };

            webClient.OpenReadAsync(new Uri(SelectedImage.userPreferredurl, UriKind.Absolute));
        }

        private void WebClientOpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {

        }

        public void ReadImagesFromInternet(int pageNumber = 0)
        {
            if (NetWorkAvailable() == false)
            {
                DispatcherHelper.UIDispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show("No Internet Connectivity!");
                });
                return;
            }
            IsPhotosLoading = true;
            string url1 = "http://www.bing.com/HPImageArchive.aspx?format=js&idx=9&n=8";
            WebClient webclient1 = new WebClient();
            webclient1.DownloadStringAsync(new Uri(url1));
            webclient1.DownloadStringCompleted += webclient_DownloadStringCompleted;
            string url = string.Format("http://www.bing.com/HPImageArchive.aspx?format=js&idx={0}&n=8", pageNumber);
            WebClient webclient = new WebClient();
            webclient.DownloadStringCompleted += webclient_DownloadStringCompleted;
            webclient.DownloadStringAsync(new Uri(url));

        }

        public void ReadRegionImagesFromInternet(string regionUrl, int pageNumber = 0)
        {
            if (NetWorkAvailable() == false)
            {
                return;
            }
            IsPhotosLoading = true;
            //string url1 = "http://www.bing.com/HPImageArchive.aspx?format=js&idx=9&n=8";
            WebClient webclient1 = new WebClient();
            webclient1.DownloadStringAsync(new Uri(regionUrl));
            webclient1.DownloadStringCompleted += webclient_DownloadRegionStringCompleted;
            //string url = string.Format("http://www.bing.com/HPImageArchive.aspx?format=js&idx={0}&n=8", pageNumber);
            //WebClient webclient = new WebClient();
            //webclient.DownloadStringCompleted += webclient_DownloadStringCompleted;
            //webclient.DownloadStringAsync(new Uri(url));

        }

        private void webclient_DownloadRegionStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null)
            {
                try
                {
                    RootObject rootOject = new RootObject();
                    rootOject = JsonConvert.DeserializeObject<RootObject>(e.Result);
                    if (rootOject != null && rootOject.images != null)
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            SelectedRegionImages.Clear();
                            foreach (var image in rootOject.images)
                            {
                                if (!image.url.Contains("www.bing.com"))
                                {
                                    image.url = "http://www.bing.com" + image.url;
                                }
                                if (image.url.Contains("1366x768"))
                                {
                                    image.url1080x1920 = image.url.Replace("1366x768", "1080x1920");
                                    image.url480x800 = image.url.Replace("1366x768", "480x800");
                                    image.url720x1280 = image.url.Replace("1366x768", "720x1280");
                                    image.url768x1280 = image.url.Replace("1366x768", "768x1280");
                                    image.url = image.url.Replace("1366x768", "240x400");
                                    if (Settings.SelectedResolution == "480 x 800")
                                    {
                                        image.userPreferredurl = image.url480x800;
                                    }
                                    else if (Settings.SelectedResolution == "720 x 1280")
                                    {
                                        image.userPreferredurl = image.url720x1280;
                                    }
                                    else if (Settings.SelectedResolution == "768 x 1280")
                                    {
                                        image.userPreferredurl = image.url768x1280;
                                    }
                                    else if (Settings.SelectedResolution == "1080 x 1920")
                                    {
                                        image.userPreferredurl = image.url1080x1920;
                                    }
                                    else
                                    {
                                        image.userPreferredurl = image.url480x800;
                                    }
                                }
                                if (!SelectedRegionImages.Contains(image))
                                    SelectedRegionImages.Add(image);
                            }
                            IsPhotosLoading = false;
                        });
                    }
                }
                catch (Exception ex)
                {
                    DispatcherHelper.UIDispatcher.BeginInvoke(() =>
                    {
                        IsPhotosLoading = false;
                    });
                }
            }
            else
            {
                DispatcherHelper.UIDispatcher.BeginInvoke(() =>
                    {
                        rootFrame.Navigate(new Uri("/Views/NetworkError.xaml", UriKind.RelativeOrAbsolute));
                    });
                //MessageBox.Show(string.IsNullOrEmpty(e.Result) ? e.Result :
                //    "testing exception");
            }
            DispatcherHelper.UIDispatcher.BeginInvoke(() => { IsPhotosLoading = false; });
        }


        private void webclient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null)
            {
                try
                {
                    RootObject rootOject = new RootObject();
                    rootOject = JsonConvert.DeserializeObject<RootObject>(e.Result);
                    if (rootOject != null && rootOject.images != null)
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            foreach (var image in rootOject.images)
                            {
                                if (!image.url.Contains("www.bing.com"))
                                {
                                    image.url = "http://www.bing.com" + image.url;
                                }
                                if (image.url.Contains("1366x768"))
                                {
                                    image.url1080x1920 = image.url.Replace("1366x768", "1080x1920");
                                    image.url480x800 = image.url.Replace("1366x768", "480x800");
                                    image.url720x1280 = image.url.Replace("1366x768", "720x1280");
                                    image.url768x1280 = image.url.Replace("1366x768", "768x1280");
                                    image.url = image.url.Replace("1366x768", "240x400");
                                    if (Settings.SelectedResolution == "480 x 800")
                                    {
                                        image.userPreferredurl = image.url480x800;
                                    }
                                    else if (Settings.SelectedResolution == "720 x 1280")
                                    {
                                        image.userPreferredurl = image.url720x1280;
                                    }
                                    else if (Settings.SelectedResolution == "768 x 1280")
                                    {
                                        image.userPreferredurl = image.url768x1280;
                                    }
                                    else if (Settings.SelectedResolution == "1080 x 1920")
                                    {
                                        image.userPreferredurl = image.url1080x1920;
                                    }
                                    else
                                    {
                                        image.userPreferredurl = image.url480x800;
                                    }
                                }
                                if (!Images.Contains(image))
                                    Images.Add(image);
                            }
                            IsPhotosLoading = false;
                        });
                    }
                }
                catch (Exception ex)
                {
                    DispatcherHelper.UIDispatcher.BeginInvoke(() =>
                    {
                        IsPhotosLoading = false;
                    });
                }
            }
            else
            {
                DispatcherHelper.UIDispatcher.BeginInvoke(() =>
                {
                    rootFrame.Navigate(new Uri("/Views/NetworkError.xaml", UriKind.RelativeOrAbsolute));
                });
            }
            DispatcherHelper.UIDispatcher.BeginInvoke(() => { IsPhotosLoading = false; });
        }

        private void LoadSettings()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains("Settings"))
            {
                Settings = (Settings)IsolatedStorageSettings.ApplicationSettings["Settings"];
            }
            else
            {
                Settings = new Settings();
            }
        }

        private void SaveSettings()
        {
            try
            {
                IsolatedStorageSettings.ApplicationSettings["Settings"] = Settings;

                if (SelectedImage != null)
                {
                    IsolatedStorageSettings.ApplicationSettings["SelectedImage"] = SelectedImage;
                }
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
            catch (Exception e)
            {

            }
            finally
            {
            }
        }
               
        private async void SaveAndSetLiveTile()
        {
            if (NetworkInterface.GetIsNetworkAvailable() == false)
            {
                return;
            }

            var imageStream = await GetLiveTileImageStreamAsync();
            if (imageStream != null)
            {
                var userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication();
                if (userStoreForApplication.FileExists(liveTileImagePath))
                {
                    userStoreForApplication.DeleteFile(liveTileImagePath);
                }
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    try
                    {
                        var isolatedStorageFileStream = userStoreForApplication.CreateFile(liveTileImagePath);
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.CreateOptions = BitmapCreateOptions.None;
                        bitmapImage.SetSource(imageStream);
                        var writeableBitmap = new WriteableBitmap(bitmapImage);
                        writeableBitmap.SaveJpeg(isolatedStorageFileStream, writeableBitmap.PixelWidth, writeableBitmap.PixelHeight, 0, 85);
                        isolatedStorageFileStream.Close();

                        var uri = new Uri(string.Format("isostore:/{0}", liveTileImagePath), UriKind.Absolute);
                        ShellTile appTile = ShellTile.ActiveTiles.First();
                        if (appTile != null)
                        {
                            appTile.Update(new FlipTileData
                            {
                                BackgroundImage = uri,
                                WideBackgroundImage = uri,
                                BackTitle = "DailyWallpapers"
                            });
                        }
                    }
                    catch (Exception venkatException)
                    {
                    }
                });
            }
        }

        private static string agentName = "BingWallpaper.Agent";
        public void UpdateAgent()
        {
            StartAgent();
        }
        private static void StartAgent()
        {
            StopAgentIfStarted();
            try
            {
                ScheduledAction action = ScheduledActionService.Find(agentName);
                if (action == null)
                {
                    ScheduledActionService.Add(new PeriodicTask(agentName) { Description = "Periodically download articles in the background if the Internet connection is Wi-Fi or Ethernet." });
                }
#if DEBUG
                ScheduledActionService.LaunchForTest(agentName, new TimeSpan(0, 0, 0, 3));
#endif
            }
            catch (Exception Ex)
            {
            }
        }

        private static void StopAgentIfStarted()
        {
            if (ScheduledActionService.Find(agentName) != null)
            {
                ScheduledActionService.Remove(agentName);
            }
        }

        private async Task<Stream> GetLiveTileImageStreamAsync()
        {
            HttpClient client = new HttpClient();
            string downloadedContent = await client.GetStringAsync(bingImageUrl);
            try
            {
                string imageUrl = "";
                RootObject rootOject = new RootObject();
                rootOject = JsonConvert.DeserializeObject<RootObject>(downloadedContent);
                if (rootOject != null && rootOject.images != null)
                {
                    foreach (var image in rootOject.images)
                    {
                        if (!image.url.Contains("www.bing.com"))
                        {
                            imageUrl = "http://www.bing.com" + image.url;
                        }
                        if (image.url.ToLower().Contains("1366x768"))
                        {
                            imageUrl = imageUrl.ToLower().Replace("1366x768", "400x240");
                        }
                        if (!string.IsNullOrEmpty(imageUrl))
                        {
                            return await GetImageStreamAsync(imageUrl);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        private async Task<Stream> GetImageStreamAsync(string imageUrl)
        {
            HttpClient client = new HttpClient();
            var responseMessage = await client.GetAsync(imageUrl, HttpCompletionOption.ResponseContentRead);
            return await responseMessage.Content.ReadAsStreamAsync();
        }
    }
}