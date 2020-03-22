///////////////////////////////////////////////////////////////////////
// ViewWindow.xaml.cs - Logic for the window for view a file         //
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
 * ViewWindow is used for front end logic for the WPF window
 * ViewWindow will handle and button clicks on the ViewWindow window
 * 
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

namespace RemoteDocumentationGenerator
{
    /// <summary>
    /// Interaction logic for ViewWindow.xaml
    /// </summary>
    public partial class ViewWindow : Window
    {
        string user;
        string file;

        //Writes a file to a read only window for viewing only
        public ViewWindow(string fileLoc, string username)
        {
            InitializeComponent();
            this.Dispatcher.Invoke(() =>
            {
                viewFile.Text = System.IO.File.ReadAllText(fileLoc);
            });
            user = username;
            file = fileLoc;
        }

        public ViewWindow()
        {
            InitializeComponent();
        }

        //Exits the environment on the exiting of a window to avoid unfinished processes
        //from stayong open after the end of a execution
        private void OnWindowclose(object sender, EventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            PostLogin postLogin = new PostLogin(user);
            this.Visibility = Visibility.Hidden;
            postLogin.Show();
        }
    }
}
