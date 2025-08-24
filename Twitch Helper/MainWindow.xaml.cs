using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Windows;

namespace Twitch_Helper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly OAuthWindow _OAuthWindow = new OAuthWindow();
        private readonly FollowerGoalWindow _FollowerGoalWindow = new FollowerGoalWindow();

        private readonly JsonHandler _JsonHandler = new JsonHandler();
        
        private static string OAuthToken;
        private static bool bOAuthSet = false;

        public MainWindow()
        {
            InitializeComponent();
            Init();
            SetOAuthToken();
        }

        public void Init()
        {
            if (!File.Exists("config.json"))
            {
                _JsonHandler.Write("OAuthToken", "OAuth", "config.json");
                _JsonHandler.Write("Username", "User", "config.json");
                _JsonHandler.Write("UserId", "UID", "config.json");
                _JsonHandler.Write("FollowerGoal", "0", "config.json");
            }
            else
            {
                return;
            }
        }

        public void SetOAuthToken()
        {
            try
            {
                OAuthToken = (string)_JsonHandler.Read("config.json", true)?["OAuthToken"] ?? string.Empty;

                if(string.IsNullOrEmpty(OAuthToken) || OAuthToken.ToLower() == "oauth" || OAuthToken == "oauth token")
                {
                    bOAuthSet = false;
                }
                else
                {
                    bOAuthSet = true;
                }

                FollowerGoal.IsEnabled = bOAuthSet;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }

        private void EditOAuthTokenButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _OAuthWindow.InitializeComponent();
                _OAuthWindow.Owner = this;
                _OAuthWindow.Show();
                _OAuthWindow.OAuthBox.Text = (string)_JsonHandler?.Read("config.json", true)?["OAuthToken"] ?? "OAuth Token";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!");
                File.WriteAllText("OAuthWindow.log", ex.ToString());
            }
        }

        private void FollowerGoal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _FollowerGoalWindow.Init();
                _FollowerGoalWindow.Owner = this;
                _FollowerGoalWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!");
                File.WriteAllText("Follower Goal.log", ex.ToString());
            }
        }
    }
}
