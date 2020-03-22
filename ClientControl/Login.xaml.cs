///////////////////////////////////////////////////////////////////////
// Login.xaml.cs - Logic for the window for logging into the service //
// ver 1.0                                                           //
// Language:    C#, 2020, .Net Framework 4.7                         //
// Platform:    Lenovo Thinkpad X1 Carbon, Win10 Pro                 //
// Application: Documentation Generator, Project #3, Winter 2020     //
// Author:      Tom Petrangelo, Syracuse University                  //
//              thpetran@syr.edu                                     //
//                                                                   //
///////////////////////////////////////////////////////////////////////
/*
 * Package Operations
 * -------------------
 * 
 * Login is used for front end logic for the WPF window
 * Login will handle and button clicks on the Login window
 * 
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
using ServiceControl;
 


namespace RemoteDocumentationGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class Login : Window
    {
        ServiceControl.Service server;
        ServiceHost service;
        
        //On window initialization, creates a communication channel
        public Login()
        {
            InitializeComponent();
            Title = "Login";
            service = Service.CreateChannel("http://localhost:8000/Service");
            service.Open();
        }

        //Exits the environment on the exiting of a window to avoid unfinished processes
        //from stayong open after the end of a execution
        private void OnWindowclose(object sender, EventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        //Checks for the username and password match
        //Either a user and pw match is found and user can login
        //the password does not match a user and they have to re-login
        //username is not found and asked to create a login
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
                                CreateLogin loginWindow = new CreateLogin(userName,service);
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
                    PostLogin primaryWindow = new PostLogin(userName, service);
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
