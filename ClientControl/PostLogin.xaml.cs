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
        List<string> userProjects = new List<string>();
        List<string> editFiles = new List<string>();
        List<string> allFiles = new List<string>();

        public PostLogin(string userName, ServiceHost serviceHost)
        {
            user = userName;
            _serverHost = serviceHost;
            InitializeComponent();
            userProjects = server.PopulateProjects(userName);
            editFiles = server.populateEditFiles(userName);
            allFiles = server.PopulateFiles();
            foreach (string project in userProjects)
            {            
                projectOptions.Items.Add(project);
                projectGenerate.Items.Add(project);
                projectEdit.Items.Add(project);

            }
            foreach (string file in editFiles)
            {
                if (!editFilesCB.Items.Contains(file))
                {
                    editFilesCB.Items.Add(file);
                }
            }
            foreach(string allFile in allFiles)
            {
                    FileToDownload.Items.Add(allFile);
                    FileToView.Items.Add(allFile);
            }
        }

        public PostLogin(string userName)
        {
            user = userName;
            InitializeComponent();
            userProjects = server.PopulateProjects(userName);
            editFiles = server.populateEditFiles(userName);
            allFiles = server.PopulateFiles();

            foreach (string project in userProjects)
            {
                projectOptions.Items.Add(project);
                projectGenerate.Items.Add(project);
                projectEdit.Items.Add(project);

            }
            foreach (string file in editFiles)
            {
                if (!editFilesCB.Items.Contains(file))
                {
                    editFilesCB.Items.Add(file);
                }
            }
            foreach (string allFile in allFiles)
            {
                    FileToDownload.Items.Add(allFile);
                    FileToView.Items.Add(allFile);

            }
        }

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

        private void CreateProject_Click(object sender, RoutedEventArgs e)
        {
            bool projMade = server.AddProject(projectName.Text,user);
            if (projMade)
            {
                server.CreateProjectHTML(projectName.Text, user);
                server.AddProjectToRoot(projectName.Text, user);
                userProjects.Add(projectName.Text);
                MessageBox.Show("Project Created!");
            }
            else
            {
                MessageBox.Show("Project Could Not Be Created!");
            }
            foreach (string project in userProjects)
            {
                if (!projectOptions.Items.Contains(project))
                {
                    projectOptions.Items.Add(project);
                    projectGenerate.Items.Add(project);
                    projectEdit.Items.Add(project);
                }
            }
        }

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

        private void UploadFile_Click(object sender, RoutedEventArgs e)
        {
            string destinationPath = server.GetFullDestinationPath(projectOptions.SelectedItem.ToString(),user);
            bool fileUploaded = server.UploadFile(uploadFilePath, destinationPath, user, projectOptions.SelectedItem.ToString());
            if (fileUploaded)
            {
                server.AddFileToProject(System.IO.Path.GetFileName(uploadFilePath), projectOptions.SelectedItem.ToString(), user);
                editFiles.Add(uploadFile.Text);
                MessageBox.Show("File Uploaded!");
            }
            else
                MessageBox.Show("File Could Not Be Uploaded!");
            foreach(string file in editFiles)
            {
                if (!editFilesCB.Items.Contains(file))
                {
                    editFilesCB.Items.Add(file);
                }
            }

            foreach(string allFile in allFiles)
            {
                if (!FileToView.Items.Contains(allFile))
                {
                    FileToView.Items.Add(allFile);
                    FileToDownload.Items.Add(allFile);
                }
            }
        }

        private void EditFiles_Click(object sender, RoutedEventArgs e)
        {
            string editedFile = server.GetFilePath(projectEdit.SelectedItem.ToString(), user, editFilesCB.SelectedItem.ToString());

            EditWindow editFiles = new EditWindow(editedFile, user);
            this.Visibility = Visibility.Hidden;
            editFiles.Show();
        }

        private void ViewFiles_Click(object sender, RoutedEventArgs e)
        {
            string project = server.GetProject(FileToView.SelectedItem.ToString());
            string fileLoc = server.GetFilePath(project, user, FileToView.SelectedItem.ToString());
            ViewWindow viewWindow = new ViewWindow(fileLoc, user);
            this.Visibility = Visibility.Hidden;
            viewWindow.Show();
        }

        private void DownloadFiles_Click(object sender, RoutedEventArgs e)
        {
            string project = server.GetProject(FileToDownload.SelectedItem.ToString());
            string userProject = server.GetUser(FileToDownload.SelectedItem.ToString());
            string fileLoc = server.GetFilePath(project, userProject, FileToDownload.SelectedItem.ToString());
            server.DownloadFile(fileLoc, user);
            MessageBox.Show("File Downloaded!");
        }

        private void GenerateProject_Click(object sender, RoutedEventArgs e)
        {
            string projectPath = server.GetFullDestinationPath(projectGenerate.SelectedItem.ToString(), user);
            server.DocumentationGenerator(projectPath, user, projectGenerate.SelectedItem.ToString());
            MessageBox.Show("Project Generated!");
        }


    }
}
