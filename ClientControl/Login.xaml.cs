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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace RemoteDocumentationGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void OnWindowclose(object sender, EventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string userName = username.Text;
            string password = userPassword.Text;

            ValidateUser(userName, password);

        }

        private void ClearLogin_Click(object sender, RoutedEventArgs e)
        {
            username.Clear();
            userPassword.Clear();
        }

        private void ValidateUser(string _username, string password)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load("../../UsernamesPasswords.xml");

            //If username is not found
            if (!findUsername(_username, xmlDocument))
            {
                string usernameNotFound = "Username not found. Create username?";
                string title = "User Not Found";
                System.Windows.MessageBoxButton options = MessageBoxButton.YesNo;
                MessageBoxResult createUser = MessageBox.Show(usernameNotFound, title, options);
                switch (createUser)
                {
                    case MessageBoxResult.Yes:
                        {
                            xmlDocument.Save("../../UsernamesPasswords.xml");
                            CreateLogin loginWindow = new CreateLogin();
                            this.Visibility = Visibility.Hidden;
                            loginWindow.Show();
                            break;
                        }

                    case MessageBoxResult.No:
                        {
                            xmlDocument.Save("../../UsernamesPasswords.xml");
                            username.Clear();
                            userPassword.Clear();
                            break;
                        }
                }
            }
            //username is found, but password is wrong
            else if (!matchPassword(_username, password, xmlDocument))
            {
                xmlDocument.Save("../../UsernamesPasswords.xml");
                string wrongPassword = "Password is incorrect, please re-enter";
                string title = "Incorrect Password";
                MessageBox.Show(wrongPassword, title);
            }
            //password is correct
            else
            {
                xmlDocument.Save("../../UsernamesPasswords.xml");
                PostLogin primaryWindow = new PostLogin();
                this.Visibility = Visibility.Hidden;
                primaryWindow.Show();
            }
        }

        private bool findUsername(string username, XmlDocument xmlDoc)
        {
            XmlNodeList nodeList = xmlDoc.SelectNodes("/Users/User/Username");
            foreach (XmlNode xmlNode in nodeList)
            {
                if (xmlNode.InnerText == username)
                    return true;
            }
            return false;
        }

        private bool matchPassword(string username, string password, XmlDocument xmlDoc)
        {
            XmlNodeList nodeListUsername = xmlDoc.SelectNodes("/Users/User/Username");
            XmlNodeList nodeListPassword = xmlDoc.SelectNodes("/Users/User/Password");
            foreach (XmlNode xmlNodeUsername in nodeListUsername)
            {
                if (xmlNodeUsername.InnerText == username)
                {
                    if (xmlNodeUsername.NextSibling.InnerText == password)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }
}
