///////////////////////////////////////////////////////////////////////
// EditWindow.xaml.cs - Logic for the window for editing a user file //
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
 * EditWindow is used for front end logic for the WPF window
 * EditWindow will handle and button clicks on the EditWindow window
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
using System.IO;
using System.Collections.Specialized;

namespace RemoteDocumentationGenerator
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        string file;
        ServiceControl.Service server = new ServiceControl.Service();
        string _username;

        //Writes a file's text to the edit window
        public EditWindow(string fileLoc, string username)
        {
            
            InitializeComponent();
            this.Dispatcher.Invoke(() =>
            {
                editText.Text = System.IO.File.ReadAllText(fileLoc.Replace(Environment.NewLine,""));
            });
            _username = username;
            file = fileLoc;
        }

        public EditWindow()
        {
            InitializeComponent();
        }

        //Exits the environment on the exiting of a window to avoid unfinished processes
        //from stayong open after the end of a execution
        private void OnWindowclose(object sender, EventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        //Saves whatever the user typed in
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            StringCollection lines = new StringCollection();
            int lineCount = editText.LineCount;
            for(int line = 0; line < lineCount; line++)
            {
                lines.Add(editText.GetLineText(line).Replace(Environment.NewLine, ""));
            }
            server.SaveFile(file, lines);
            PostLogin postLogin = new PostLogin(_username);
            this.Visibility = Visibility.Hidden;
            postLogin.Show();
        }

        //Cancels any changes
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            PostLogin postLogin = new PostLogin(_username);
            this.Visibility = Visibility.Hidden;
            postLogin.Show();
        }
    }
}
