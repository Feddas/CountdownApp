using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;

namespace GW2TaskAgent
{
    public class TimeRemaining
    {
        private DateTime releaseDate = new DateTime(2012, 8, 28);
        private DateTime prereleaseDate = new DateTime(2012, 8, 25);
        private DateTime userDate;

        public TimeRemaining()
        {
            int dateType = 0;

            if (IsolatedStorageSettings.ApplicationSettings.Contains("DateType"))
                dateType = (int)IsolatedStorageSettings.ApplicationSettings["DateType"];

            switch (dateType)
            {
                case 1:
                    userDate = prereleaseDate;
                    break;
                case 0:
                default:
                    userDate = releaseDate;
                    break;
            }
        }

        public string GetTimeRemaining(bool getSeconds = true)
        {
            TimeSpan remaining = userDate - DateTime.Now;
            string result = String.Format("{0}:{1}:{2}"
                , remaining.Days.ToString("D2")
                , Math.Abs(remaining.Hours).ToString("D2")
                , Math.Abs(remaining.Minutes).ToString("D2"));

            if (getSeconds)
                result += String.Format(":{0}"
                    , Math.Abs(remaining.Seconds).ToString("D2"));

            return result;
        }
    }
}
