using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Alexandria
{
    /// <summary>
    /// The home page for the application
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void GoToCheckout(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Checkout));
        }

        private void GoToCheckIn(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CheckIn));
        }

        private void GoToAddBook(object sender, RoutedEventArgs e)
        {

        }

        private void GoToViewBooks(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ViewBooks));
        }
    }
}
