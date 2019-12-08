using System;
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
        private string Pass;
        private string OAuth { get; set; } = "";
        private readonly JsonHandler _JsonHandler = new JsonHandler();

        public OAuthWindow()
        {
            try
            {
                MouseLeftButtonDown += ApiKeyWindow_MouseLeftButtonDown;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!");
                File.WriteAllText("OAuthWindow.log", ex.ToString());
            }
            
        }

        private void ApiKeyWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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
                MessageBox.Show(ex.ToString(), "Error!");
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
                MessageBox.Show(ex.ToString(), "Error!");
                File.WriteAllText("OAuthWindow.log", ex.ToString());
            }
        }

        private void PWDBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Pass = PWDBox.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!");
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
                MessageBox.Show(ex.ToString(), "Error!");
                File.WriteAllText("OAuthWindow.log", ex.ToString());
            }
        }

        private void PWDBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                PWDBox.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!");
                File.WriteAllText("OAuthWindow.log", ex.ToString());
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Hide();
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(MainWindow))
                    {
                        (window as MainWindow).FollowerGoal.IsEnabled = true;
                    }
                }
                _JsonHandler.Write("OAuth", OAuth, "config.json");
                FollowerGoalWindow.FollowerTimer.IsEnabled = true;
                FollowerGoalWindow.FollowerTimer.Start();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!");
                File.WriteAllText("OAuthWindow.log", ex.ToString());
            }
        }
    }
}
