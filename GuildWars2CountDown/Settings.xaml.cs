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
using Microsoft.Phone.Shell;
using GuildWars2CountDown;
using Microsoft.Phone.Scheduler;
using System.IO.IsolatedStorage;

namespace GuildWars2Countdown
{
    public partial class Settings : PhoneApplicationPage
    {
        public Settings()
        {
            InitializeComponent();
            btnDateType.Content = dateTypeText();
        }

        private void LiveTile_Click(object sender, RoutedEventArgs e)
        {
            setupTask();

            (new GW2TaskAgent.ScheduledAgent()).UpdateTile();

            //run task
            //Microsoft.Phone.Shell.ShellTile liveTile = Microsoft.Phone.Shell.ShellTile.ActiveTiles.First();
            //if (liveTile != null)
            //{
            //    // Set the properties to update for the Application Tile.
            //    // Empty strings for text & URIs will result in the property being cleared.
            //    StandardTileData newTileData = new StandardTileData
            //    {
            //        Title = (Application.Current as App).TimeRemaining,
            //        //BackgroundImage = new Uri(textBoxBackgroundImage.Text, UriKind.Relative),
            //        //Count = levelNum,
            //        //BackTitle = "Bouncy Lasers",
            //        //BackBackgroundImage = new Uri(textBoxBackBackgroundImage.Text, UriKind.Relative),
            //        //BackContent = "on level " + levelNum.ToString(),
            //    };

            //    liveTile.Update(newTileData);
            //}
        }

        const string AgentName = "GW2CountdownAgent";
        bool agentsDisabled = false;
        void setupTask()
        {
            if (agentsDisabled)
                return;

            PeriodicTask periodicTask = new PeriodicTask(AgentName);

            periodicTask.Description = "My live tile periodic task";
            periodicTask.ExpirationTime = System.DateTime.Now.AddDays(1);

            // If the agent is already registered with the system,
            if (ScheduledActionService.Find(periodicTask.Name) != null)
            {
                ScheduledActionService.Remove(AgentName);
            }

            //handle inability to add scheduler http://msdn.microsoft.com/en-us/library/hh202944(v=vs.92).aspx
            try
            {
                //only can be called when application is running in foreground
                ScheduledActionService.Add(periodicTask);

                //ScheduledActionService.LaunchForTest(periodicTask.Name, TimeSpan.FromSeconds(10));
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message.Contains("BNS Error: The action is disabled"))
                {
                    MessageBox.Show("Background agents for this application have been disabled by the user.");
                    agentsDisabled = true;
                }
                if (ioe.Message.Contains("BNS Error: The maximum number of ScheduledActions of this type have already been added."))
                {
                    MessageBox.Show("Either the maximum number of Schedulers have been added or you are using a 256MB device which does not support scheduling live tile updates. The live tile can still be updated manually on this page.");
                    agentsDisabled = true;
                    // No user action required. The system prompts the user when the hard limit of periodic tasks has been reached.
                }
            }
            catch (SchedulerServiceException)
            {
                // No user action required.
            }
        }

        private string dateTypeText()
        {
            string buttonText = "Using Release Date";
            if (IsolatedStorageSettings.ApplicationSettings.Contains("DateType"))
            {
                int dateType = (int)IsolatedStorageSettings.ApplicationSettings["DateType"];
                switch (dateType)
                {
                    case 1:
                        buttonText = "Using Pre-Release Date";
                        break;
                    case 0:
                    default:
                        break;
                }
            }
            return buttonText;
        }

        private void PreRelease_Click(object sender, RoutedEventArgs e)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains("DateType"))
            {
                int dateType = (int)IsolatedStorageSettings.ApplicationSettings["DateType"];
                switch (dateType)
                {
                    case 0:
                        IsolatedStorageSettings.ApplicationSettings["DateType"] = 1;
                        break;
                    case 1:
                    default:
                        IsolatedStorageSettings.ApplicationSettings["DateType"] = 0;
                        break;
                }
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings["DateType"] = 1;
            }

            Button btn = sender as Button;
            if (btn != null)
                btn.Content = dateTypeText();
        }
    }
}