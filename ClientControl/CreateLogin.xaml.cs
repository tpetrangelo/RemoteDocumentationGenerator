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
            string userName = username.Text;
            string password = userPassword.Text;

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load("../../UsernamesPasswords.xml");

            XmlNode user = xmlDocument.CreateElement("User");
            XmlNode _username = xmlDocument.CreateElement("Username");
            XmlNode pw = xmlDocument.CreateElement("Password");
            _username.InnerText = userName;
            pw.InnerText = password;
            user.AppendChild(_username);
            user.AppendChild(pw);
            xmlDocument.DocumentElement.AppendChild(user);
            xmlDocument.Save("../../UsernamesPasswords.xml");
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
