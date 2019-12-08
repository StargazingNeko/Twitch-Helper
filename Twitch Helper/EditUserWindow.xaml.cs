using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Twitch_Helper
{
    /// <summary>
    /// Interaction logic for EditUserWindow.xaml
    /// </summary>
    public partial class EditUserWindow : Window
    {
        private readonly JsonHandler _JsonHandler = new JsonHandler();
        private string UserTxt;
        public EditUserWindow()
        {
            try
            {
                UserTxt = (string)_JsonHandler.Read("config.json", true)["Username"];
                UsernameBox.Text = UserTxt;
            }
            catch
            {

            }
        }

        public void SetUsername()
        {
            UsernameBox.Text = UserTxt;
        }

        private void UsernameSaveButton_Click(object sender, RoutedEventArgs e)
        {
            _JsonHandler.Write("Username", UsernameBox.Text, "config.json");
            TwitchHandler.bHasInitRun = false;
            this.Hide();
        }
    }
}
