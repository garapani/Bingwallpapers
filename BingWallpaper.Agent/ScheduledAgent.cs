using Microsoft.Phone.Scheduler;
using System;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;
using System.Net.NetworkInformation;
using System.IO.IsolatedStorage;
using System.Linq;
using BingWallapapers.Helper;
using BingWallpaper.Model;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace BingWallPaper.Agent
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        private const string LastUpdateVerificationPropertyName = "LastUpdateVerification";
        private const string liveTileImagePath = "Shared/ShellContent/" + "liveTile.jpg";
        private const string bingImageUrl = "http://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1";
        private DateTime _lastCheck;

        protected override async void OnInvoke(ScheduledTask task)
        {
            try
            {
                _lastCheck = IsolatedStorageHelper.GetDateTime(LastUpdateVerificationPropertyName, new DateTime(1, 1, 1));
                if (_lastCheck != null)
                {
                    if (_lastCheck.Year != 1)
                    {
                        if (IsUpdateRequired())
                        {

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
                                            NotifyComplete();
                                        }
                                    }
                                    catch (Exception venkatException)
                                    {
                                        NotifyComplete();
                                    }
                                });
                            }
                            else
                            {
                                NotifyComplete();
                            }
                        }
                        else
                        {
                            NotifyComplete();
                        }
                    }
                    else
                    {
                        IsolatedStorageHelper.SaveSettingValueImmediately(LastUpdateVerificationPropertyName, DateTime.Now);
                        NotifyComplete();
                    }
                }
                else
                {
                    IsolatedStorageHelper.SaveSettingValueImmediately(LastUpdateVerificationPropertyName, DateTime.Now);
                    NotifyComplete();
                }
            }
            catch (Exception)
            {
                NotifyComplete();
            }
        }
        private bool IsUpdateRequired()
        {
            try
            {
                if (!NetworkInterface.GetIsNetworkAvailable())
                {
                    return false;
                }

                if (_lastCheck != null)
                {
                    var timeDiff = DateTime.Now - _lastCheck;
                    if (timeDiff.TotalHours >= 24)
                    {
                        return true;
                    }
                }
            }
            catch (Exception)
            {
            }
            return false;
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