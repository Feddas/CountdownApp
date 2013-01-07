using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Windows.Threading;

namespace GuildWars2CountDown
{
    public partial class MainPage : PhoneApplicationPage
    {
        GW2TaskAgent.TimeRemaining timeKeeper;
        private DispatcherTimer countdownTimer = new DispatcherTimer();
        //private DateTime releaseDate = new DateTime(2012, 8, 28);
        //private DateTime prereleaseDate = new DateTime(2012, 8, 25);

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            countdownTimer.Interval = TimeSpan.FromSeconds(1);
            countdownTimer.Tick += new EventHandler(countdownTimer_Tick);
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
            adControl.ErrorOccurred += new EventHandler<Microsoft.Advertising.AdErrorEventArgs>(adControl_ErrorOccurred);
        }

        void adControl_ErrorOccurred(object sender, Microsoft.Advertising.AdErrorEventArgs e)
        {
            Exception test = e.Error;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            countdownTimer.Stop();
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            countdownTimer.Start();
            timeKeeper = new GW2TaskAgent.TimeRemaining();
            TimeRemaining.Text = timeRemaining();
        }

        void countdownTimer_Tick(object sender, EventArgs e)
        {
            TimeRemaining.Text = timeRemaining();
        }

        string timeRemaining()
        {
            (Application.Current as App).TimeRemaining = timeKeeper.GetTimeRemaining();
            //TimeSpan remaining = releaseDate - DateTime.Now;
            //(Application.Current as App).TimeRemaining = String.Format("{0}:{1}:{2}:{3}"
            //    , remaining.Days.ToString("D2")
            //    , remaining.Hours.ToString("D2")
            //    , remaining.Minutes.ToString("D2")
            //    , remaining.Seconds.ToString("D2"));
            return (Application.Current as App).TimeRemaining;
        }

        private void TimeRemaining_Tap(object sender, GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
        }
    }
}