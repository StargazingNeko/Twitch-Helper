using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Twitch_Helper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            /*if(File.Exists("config.json"))
            {
                StartupUri = new Uri("/Twitch Helper;component/PasswordWindow.xaml", UriKind.Relative);
            }
            else
            {
                StartupUri = new Uri("/Twitch Helper;component/MainWindow.xaml", UriKind.Relative);
            }*/
            StartupUri = new Uri("/Twitch Helper;component/MainWindow.xaml", UriKind.Relative);
        }

    }
}
