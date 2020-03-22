///////////////////////////////////////////////////////////////////////
// CreateLogin.xaml.cs - logic for creating a login and username     //
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
 * CreateLogin is used for front end logic for the WPF window
 * CreateLogin will handle and button clicks on the CreateLogin window
*/




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
using System.ServiceModel;

namespace RemoteDocumentationGenerator
{
    /// <summary>
    /// Interaction logic for CreateLogin.xaml
    /// </summary>
    public partial class CreateLogin : Window
    {
        ServiceControl.Service server;
        string user;
        ServiceHost _service;
        
        public CreateLogin(string userName, ServiceHost service)
        {
            InitializeComponent();
            user = userName;
            _service = service;
        }

        public CreateLogin()
        {
            InitializeComponent();
        }

        //Exits the environment on the exiting of a window to avoid unfinished processes
        //from stayong open after the end of a execution
        private void OnWindowclose(object sender, EventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        //Creates a user and adds the password and username to the XML file
        //Also asks the host to create a root HTML page
        private void CreateUser_Click(object sender, RoutedEventArgs e)
        {
            server = new ServiceControl.Service();
            string userName = username.Text;
            string password = userPassword.Text;
            server.AddToXML(userName, password);
            server.CreateRootHTML(userName);
            MessageBox.Show("Username Created!");
            _service.Close();
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
