using Newtonsoft.Json.Linq;
using System;
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
        private readonly OAuthWindow _ApiKeyWindow = new OAuthWindow();
        private readonly FollowerGoalWindow _FollowerGoalWindow = new FollowerGoalWindow();
        private readonly EditUserWindow _EditUserWindow = new EditUserWindow();

        private readonly JsonHandler _JsonHandler = new JsonHandler();
        
        private static string OAuthTokenPassword;
        private static string OAuthToken = null;
        private static bool bOAuthSet = false;

        public MainWindow()
        {
            InitializeComponent();
            //FollowerGoal.IsEnabled = bOAuthSet;
            EditOAuthTokenButton.IsEnabled = false;

            
        }

        public void SetOAuthKeyPassword(string _OAuthTokenPassword)
        {
            try
            {
                OAuthTokenPassword = _OAuthTokenPassword;
                OAuthToken = (string)_JsonHandler.Read("config.json", true)["OAuth"];
                OAuthTokenPassword = null;
                bOAuthSet = true;
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
                _ApiKeyWindow.InitializeComponent();
                _ApiKeyWindow.Owner = this;
                _ApiKeyWindow.Show();
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

        private void EditUsername_Click(object sender, RoutedEventArgs e)
        {
            _EditUserWindow.InitializeComponent();
            _EditUserWindow.Owner = this;
            _EditUserWindow.SetUsername();
            _EditUserWindow.Show();
        }
    }
}
