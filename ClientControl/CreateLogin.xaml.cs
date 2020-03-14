using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;

namespace RemoteDocumentationGenerator
{
    /// <summary>
    /// Interaction logic for CreateLogin.xaml
    /// </summary>
    public partial class CreateLogin : Window
    {
        ServiceControl.Service server;

        public CreateLogin()
        {
            InitializeComponent();
        }

        private void OnWindowclose(object sender, EventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        private void CreateUser_Click(object sender, RoutedEventArgs e)
        {
            server = new ServiceControl.Service();
            string userName = username.Text;
            string password = userPassword.Text;
            server.AddToXML(userName, password);
            MessageBox.Show("Username Created!");
            Login mainWindow = new Login();
            this.Visibility = Visibility.Hidden;
            mainWindow.Show();
        }

        private void ClearLogin_Click(object sender, RoutedEventArgs e)
        {
            username.Clear();
            userPassword.Clear();
        }
    }
}
