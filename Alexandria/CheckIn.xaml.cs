using Alexandria.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Alexandria
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class CheckIn : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public CheckIn()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private async void CheckInBook(object sender, RoutedEventArgs e)
        {
            Notice.Foreground = new SolidColorBrush(Windows.UI.Colors.Black);
            Notice.Text = "Attempting check in...";
            Dictionary<string, string> checkin = new Dictionary<string, string>();
            checkin["isbn"] = ISBN.Text;
            checkin["patron_barcode"] = Patron.Password;
            checkin["distributor_barcode"] = Distributor.Password;
            string json = JsonConvert.SerializeObject(checkin);
            try
            {
                HttpClient client = new HttpClient();
                StringContent theContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage aResponse = await client.PostAsync(new Uri("http://alexandria.ad.sofse.org:8080/check_in.json"), theContent);
                string content = await aResponse.Content.ReadAsStringAsync();
                ISBN.Text = "";
                Patron.Password = "";
                Distributor.Password = "";
                if ((int)aResponse.StatusCode == 200)
                {
                    Dictionary<string, Dictionary<string,string>> dictionary = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string,string>>>(content);
                    this.Frame.Navigate(typeof(PutAway), dictionary);
                }
                else
                {
                    Dictionary<string, List<string>> dictionary = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(content);
                    string notice = "";
                    foreach (var item in dictionary)
                    {
                        foreach (var error in item.Value)
                        {
                            notice += error + "\n";
                        }
                    }
                    Notice.Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
                    Notice.Text = notice;
                }
            }
            catch (HttpRequestException)
            {
                Notice.Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
                Notice.Text = "You might want to consider connecting to the internet...";
                return;
            }
        }
    }
}
