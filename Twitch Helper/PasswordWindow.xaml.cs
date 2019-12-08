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
    /// Interaction logic for PasswordWindow.xaml
    /// </summary>
    public partial class PasswordWindow : Window
    {
        private MainWindow _MainWindow;
        public PasswordWindow()
        {
            InitializeComponent();
        }

        private void PassOKButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            _MainWindow = new MainWindow();
            _MainWindow.Show();
            _MainWindow.SetOAuthKeyPassword(OAuthPasswordBox.Password);
        }
    }
}
