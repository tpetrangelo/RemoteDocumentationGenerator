﻿///////////////////////////////////////////////////////////////////////
// PostLogin.xaml.cs - Logic for the window for using the service    //
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
 * PostLogin is used for front end logic for the WPF window
 * PostLogin will handle and button clicks on the PostLogin window
 * PostLogin is the main window for all usages in the program
 * 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Xml;
using System.ServiceModel;


namespace RemoteDocumentationGenerator
{
    /// <summary>
    /// Interaction logic for PrimaryWindow.xaml
    /// </summary>
    public partial class PostLogin : Window
    {
        ServiceControl.Service server = new ServiceControl.Service();
        string user;
        static string uploadFilePath;
        static ServiceHost _serverHost;
        Dictionary<string, List<string>> userProjects = new Dictionary<string, List<string>>();
        Dictionary<string, List<string>> editFiles = new Dictionary<string, List<string>>();
        List<string> allFiles = new List<string>();

        //populates all possible drop down boxes with valid projects or files
        //Takes in a ServiceHost to continue streamlined execution
        public PostLogin(string userName, ServiceHost serviceHost)
        {
            user = userName;
            _serverHost = serviceHost;
            InitializeComponent();
            projectEdit.SelectionChanged += new SelectionChangedEventHandler(projectEdit_SelectionChanged);
            userProjects.Add(userName, server.PopulateProjects(userName));
            allFiles = server.PopulateFiles();
            foreach (string project in userProjects[userName])
            {   
                if(!(projectOptions.Items.Contains(project) && projectGenerate.Items.Contains(project) && projectEdit.Items.Contains(project))){
                    projectOptions.Items.Add(project);
                    projectGenerate.Items.Add(project);
                    projectEdit.Items.Add(project);
                }
            }

            foreach(string allFile in allFiles)
            {
                if(!(FileToView.Items.Contains(allFile) && FileToDownload.Items.Contains(allFile)))
                {
                    FileToDownload.Items.Add(allFile);
                    FileToView.Items.Add(allFile);
                }
            }
        }

        //populates all possible drop down boxes with valid projects or files
        public PostLogin(string userName)
        {
            user = userName;
            InitializeComponent();
            projectEdit.SelectionChanged += new SelectionChangedEventHandler(projectEdit_SelectionChanged);
            userProjects.Add(userName, server.PopulateProjects(userName));
            allFiles = server.PopulateFiles();

            foreach (string project in userProjects[userName])
            {
                if (!(projectOptions.Items.Contains(project) && projectGenerate.Items.Contains(project) && projectEdit.Items.Contains(project)))
                {
                    projectOptions.Items.Add(project);
                    projectGenerate.Items.Add(project);
                    projectEdit.Items.Add(project);
                }
            }
            foreach (string allFile in allFiles)
            {
                if (!(FileToView.Items.Contains(allFile) && FileToDownload.Items.Contains(allFile)))
                {
                    FileToDownload.Items.Add(allFile);
                    FileToView.Items.Add(allFile);
                }
            }
        }

        //Exits the environment on the exiting of a window to avoid unfinished processes
        //from stayong open after the end of a execution
        private void OnWindowclose(object sender, EventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        public void Logout_Click(object sender, RoutedEventArgs e)
        {
            _serverHost.Close();
            Login mainWindow = new Login();
            this.Visibility = Visibility.Hidden;
            mainWindow.Show();
        }

        //Creates a project in the user's directory
        private void CreateProject_Click(object sender, RoutedEventArgs e)
        {
            bool projMade = server.AddProject(projectName.Text,user);
            if (projMade)
            {
                server.CreateProjectHTML(projectName.Text, user);
                server.AddProjectToRoot(projectName.Text, user);
                userProjects[user].Add(projectName.Text);
                MessageBox.Show("Project Created!");
            }
            else
            {
                MessageBox.Show("Project Could Not Be Created!");
            }
            foreach (string project in userProjects[user])
            {
                if (!(projectOptions.Items.Contains(project) && projectGenerate.Items.Contains(project) && projectEdit.Items.Contains(project)))
                {
                    projectOptions.Items.Add(project);
                    projectGenerate.Items.Add(project);
                    projectEdit.Items.Add(project);
                }
            }
        }

        //Browses files to upload
        private void BrowseFiles_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.DefaultExt = "*.cs";

            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == true)
            {
                string file = openFileDialog.SafeFileName;
                uploadFilePath = openFileDialog.FileName;
                uploadFile.Text = file;
            }
                
        }

        //Initiates the upload of a file to a project
        private void UploadFile_Click(object sender, RoutedEventArgs e)
        {
            string destinationPath = server.GetFullDestinationPath(projectOptions.SelectedItem.ToString(),user);
            bool fileUploaded = server.UploadFile(uploadFilePath, destinationPath, user, projectOptions.SelectedItem.ToString());
            if (fileUploaded)
            {
                server.AddFileToProject(System.IO.Path.GetFileName(uploadFilePath), projectOptions.SelectedItem.ToString(), user);
                if (!editFiles.ContainsKey(user))
                {
                    List<string> edit = new List<string>();
                    editFiles.Add(user, edit);
                }
                editFiles[user].Add(uploadFile.Text);
                allFiles.Add(uploadFile.Text);
                MessageBox.Show("File Uploaded!");
            }
            else
                MessageBox.Show("File Could Not Be Uploaded!");

            foreach (string file in editFiles[user])
            {
                if (!editFilesCB.Items.Contains(file))
                {
                    editFilesCB.Items.Add(file);
                }
            }

            foreach (string allFile in allFiles)
            {
                if (!(FileToView.Items.Contains(allFile) && FileToDownload.Items.Contains(allFile)))
                {
                    FileToDownload.Items.Add(allFile);
                    FileToView.Items.Add(allFile);
                }
            }
        }

        //Opens EditWindow for user editing
        private void EditFiles_Click(object sender, RoutedEventArgs e)
        {
            string editedFile = server.GetFilePath(projectEdit.SelectedItem.ToString(), user, editFilesCB.SelectedItem.ToString());

            EditWindow editFiles = new EditWindow(editedFile, user);
            this.Visibility = Visibility.Hidden;
            editFiles.Show();
        }

        //Opens ViewWindow for user viewing
        private void ViewFiles_Click(object sender, RoutedEventArgs e)
        {
            string project = server.GetProject(FileToView.SelectedItem.ToString());
            string fileUser = server.GetUser(FileToView.SelectedItem.ToString());
            string fileLoc = server.GetFilePath(project, fileUser, FileToView.SelectedItem.ToString());
            ViewWindow viewWindow = new ViewWindow(fileLoc, user);
            this.Visibility = Visibility.Hidden;
            viewWindow.Show();
        }

        //Downloads a file to a user's DownloadedFile folder
        private void DownloadFiles_Click(object sender, RoutedEventArgs e)
        {
            string project = server.GetProject(FileToDownload.SelectedItem.ToString());
            string userProject = server.GetUser(FileToDownload.SelectedItem.ToString());
            string fileLoc = server.GetFilePath(project, userProject, FileToDownload.SelectedItem.ToString());
            server.DownloadFile(fileLoc, user);
            MessageBox.Show("File Downloaded!");
        }

        //Starts the generation of a project of files
        private void GenerateProject_Click(object sender, RoutedEventArgs e)
        {
            string projectPath = server.GetFullDestinationPath(projectGenerate.SelectedItem.ToString(), user);
            server.DocumentationGenerator(projectPath, user, projectGenerate.SelectedItem.ToString());
            MessageBox.Show("Project Generated!");
        }

        //When a project is selected, the below function picks all files in that project to edit
        private void projectEdit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string project = projectEdit.SelectedItem.ToString();
            editFiles.Clear();
            editFiles.Add(user, server.PopulateEditFiles(user, project));
            foreach(KeyValuePair<string,List<string>> kvp in editFiles)
            {
               foreach(string file in kvp.Value)
                {
                    if (!editFilesCB.Items.Contains(file))
                    {
                        editFilesCB.Items.Add(file);
                    }
                }
                
            }
        }
    }
}
