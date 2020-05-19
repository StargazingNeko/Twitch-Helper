using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Twitch_Helper
{
    /// <summary>
    /// Interaction logic for FollowerGoalWindow.xaml
    /// </summary>
    public partial class FollowerGoalWindow : Window
    {
        private readonly TwitchHandler _TwitchHandler = new TwitchHandler();
        private readonly JsonHandler _JsonHandler = new JsonHandler();
        public static DispatcherTimer FollowerTimer = new DispatcherTimer();

        public FollowerGoalWindow()
        {
            
        }

        public void Init()
        {
            InitializeComponent();

            if (File.Exists("config.json"))
            {
                    FollowerGoal.Text = (string)_JsonHandler?.Read("config.json", true)?["FollowerGoal"] ?? "0";
            }

            FollowerTimer.Tick += (o, e) => { CurrentFollowerCount.Text = _TwitchHandler.GetFollowerCount().ToString(); CompareFollowerCount(); };
            FollowerTimer.IsEnabled = true;
        }

        private void CompareFollowerCount()
        {
            FollowerTimer.Interval = TimeSpan.FromSeconds(5);
            string Followers = $"{CurrentFollowerCount.Text}/{FollowerGoal.Text}";
            if (!File.Exists("Follower Goal.txt"))
            {
                File.WriteAllText("Follower Goal.txt", $"{Followers}");
            }
            else if (Followers != File.ReadAllText("Follower Goal.txt"))
            {
                WriteToFile();
                Console.WriteLine("Written new follower count/goal to Follower Goal.txt");
            }
        }

        private void InterceptClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            WriteToFile();
            this.Hide();
        }

        private void WriteToFile()
        {
            File.WriteAllText("Follower Goal.txt", CurrentFollowerCount.Text + Slash.Content + FollowerGoal.Text);
            _JsonHandler.Write("FollowerGoal", FollowerGoal.Text ?? "0", "config.json");
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            WriteToFile();
        }

        private void FollowerGoal_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FollowerGoal.Text = "";
        }
    }
}
