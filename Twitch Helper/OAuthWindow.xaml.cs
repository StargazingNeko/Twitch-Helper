using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Newtonsoft.Json.Linq;

namespace Twitch_Helper
{
    /// <summary>
    /// Interaction logic for ApiKeyWindow.xaml
    /// </summary>
    public partial class OAuthWindow : Window
    {
        private string OAuth { get; set; } = "";
        private readonly JsonHandler _JsonHandler = new JsonHandler();
        private readonly TwitchHandler _TwitchHandler = new TwitchHandler();

        public OAuthWindow()
        {
            try
            {
                MouseLeftButtonDown += OAuthTokenWindow_MouseLeftButtonDown;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                File.WriteAllText("OAuthWindow.log", ex.ToString());
            }
            
        }

        private void OAuthTokenWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.ChangedButton == MouseButton.Left)
                {
                    DragMove();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                File.WriteAllText("OAuthWindow.log", ex.ToString());
            }

        }

        private void OAuthBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                OAuth = OAuthBox.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                File.WriteAllText("OAuthWindow.log", ex.ToString());
            }
        }

        private void OAuthBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                OAuthBox.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                File.WriteAllText("OAuthWindow.log", ex.ToString());
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OAuth = OAuthBox.Text;

                if (!string.IsNullOrEmpty(OAuth) && !OAuth.ToLower().Contains("oauth"))
                {
                    _JsonHandler.Write("OAuthToken", OAuth, "config.json");
                    _TwitchHandler.SetOAuthToken(OAuth);

                    string Username = _TwitchHandler.GetUsername(OAuth);
                    string UserId = _TwitchHandler.GetUserId(OAuth);

                    if (!string.IsNullOrEmpty(Username))
                    {
                        _JsonHandler.Write("Username", Username, "config.json");
                    }
                    else
                    {
                        return;
                    }

                    if (!string.IsNullOrEmpty(UserId))
                    {
                        _JsonHandler.Write("UserId", UserId, "config.json");
                    }
                    else
                    {
                        return;
                    }

                    this.Hide();
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(MainWindow))
                        {
                            (window as MainWindow).FollowerGoal.IsEnabled = true;
                        }
                    }

                    FollowerGoalWindow.FollowerTimer.IsEnabled = true;
                    FollowerGoalWindow.FollowerTimer.Start();
                }
                else
                {
                    MessageBox.Show("You must set an OAuth token!", "Please set an OAuth token!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                File.WriteAllText("OAuthWindow.log", ex.ToString());
            }
        }

        private void OAuthUrlLabel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = $"https://id.twitch.tv/oauth2/authorize?client_id={TwitchHandler.ClientID}&redirect_uri=https://www.entropicgamers.com/TTVHelper/oauth&scope=channel:read:subscriptions&response_type=token",
                UseShellExecute = true
            };
            
            Process.Start(psi);
        }
    }
}
