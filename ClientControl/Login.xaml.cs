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
        ServiceControl.Service server;
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
            server = new ServiceControl.Service();

            string userName = username.Text;
            string password = userPassword.Password;

            bool foundUsername = server.FindUsername(userName);
            bool matchPassword = server.MatchPassword(userName, password);


                if (!foundUsername)
                {
                    string usernameNotFound = "Username not found. Create username?";
                    string title = "User Not Found";
                    System.Windows.MessageBoxButton options = MessageBoxButton.YesNo;
                    MessageBoxResult createUser = MessageBox.Show(usernameNotFound, title, options);
                    switch (createUser)
                    {
                        case MessageBoxResult.Yes:
                            {
                                CreateLogin loginWindow = new CreateLogin();
                                this.Visibility = Visibility.Hidden;
                                loginWindow.Show();
                                break;
                            }

                        case MessageBoxResult.No:
                            {
                                username.Clear();
                                userPassword.Clear();
                                break;
                            }
                    }
                }
                //username is found, but password is wrong
                else if (!matchPassword)
                {
                    string wrongPassword = "Password is incorrect, please re-enter";
                    string title = "Incorrect Password";
                    MessageBox.Show(wrongPassword, title);
                }
                //password is correct
                else
                {
                    PostLogin primaryWindow = new PostLogin();
                    this.Visibility = Visibility.Hidden;
                    primaryWindow.Show();
                }
        }

        private void ClearLogin_Click(object sender, RoutedEventArgs e)
        {
            username.Clear();
            userPassword.Clear();
        }
    }
}
