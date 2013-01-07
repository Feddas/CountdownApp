using System.Windows;
using System.Linq;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;

namespace GW2TaskAgent
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        private static volatile bool _classInitialized;
        TimeRemaining timeKeeper = new TimeRemaining();

        /// <remarks>
        /// ScheduledAgent constructor, initializes the UnhandledException handler
        /// </remarks>
        public ScheduledAgent()
        {
            if (!_classInitialized)
            {
                _classInitialized = true;
                // Subscribe to the managed exception handler
                Deployment.Current.Dispatcher.BeginInvoke(delegate
                {
                    Application.Current.UnhandledException += ScheduledAgent_UnhandledException;
                });
            }
        }

        /// Code to execute on Unhandled Exceptions
        private void ScheduledAgent_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        /// <summary>
        /// Agent that runs a scheduled task
        /// http://rodrigueh.com/wp7-live-tiles-with-background-agents
        /// </summary>
        /// <param name="task">
        /// The invoked task
        /// </param>
        /// <remarks>
        /// This method is called when a periodic or resource intensive task is invoked
        /// </remarks>
        protected override void OnInvoke(ScheduledTask task)
        {
            /// If application uses both PeriodicTask and ResourceIntensiveTask
            if (task is PeriodicTask)
            {
                // Execute periodic task actions here.
                UpdateTile();
            }
            NotifyComplete();
        }

        public void UpdateTile()
        {
            ShellTile TileToFind = ShellTile.ActiveTiles.First();//.FirstOrDefault(x => x.NavigationUri.ToString().Contains("TileID=2"));
            if (TileToFind != null)
            {
                StandardTileData NewTileData = new StandardTileData
                {
                    Title = timeKeeper.GetTimeRemaining(false) + "@" + System.DateTime.Now.ToShortTimeString(),
                };
                TileToFind.Update(NewTileData);
            }
        }
    }
}