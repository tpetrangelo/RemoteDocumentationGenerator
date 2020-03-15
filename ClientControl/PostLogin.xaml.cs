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
using Microsoft.Win32;
using System.Xml;

namespace RemoteDocumentationGenerator
{
    /// <summary>
    /// Interaction logic for PrimaryWindow.xaml
    /// </summary>
    public partial class PostLogin : Window
    {
        ServiceControl.Service server = new ServiceControl.Service();
        string user;
        List<string> userProjects = new List<string>();
        List<string> editFiles = new List<string>();

        public PostLogin(string userName)
        {
            user = userName;
            InitializeComponent();
            userProjects = server.PopulateProjects(userName);
            editFiles = server.populateEditFiles(userName);
            foreach (string project in userProjects)
            {            
                projectOptions.Items.Add(project);
                projectGenerate.Items.Add(project);
                projectEdit.Items.Add(project);

            }
            foreach (string file in editFiles)
            {
                editFilesCB.Items.Add(file);
            }
        }

        public PostLogin()
        {
            InitializeComponent();
        }

        private void OnWindowclose(object sender, EventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        public void Logout_Click(object sender, RoutedEventArgs e)
        {
            Login mainWindow = new Login(user);
            this.Visibility = Visibility.Hidden;
            mainWindow.Show();
        }

        private void CreateProject_Click(object sender, RoutedEventArgs e)
        {
            bool projMade = server.AddProject(projectName.Text,user);
            if (projMade)
            {
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
                string file = openFileDialog.FileName;
                uploadFile.Text = file;
            }
                
        }

        private void UploadFile_Click(object sender, RoutedEventArgs e)
        {
            string destinationPath = server.GetFullDestinationPath(projectOptions.SelectedItem.ToString(),user);
            bool fileUploaded = server.UploadFile(uploadFile.Text, destinationPath, user, projectOptions.SelectedItem.ToString());
            if (fileUploaded)
            {
                editFiles.Add(uploadFile.Text);
                MessageBox.Show("Project Uploaded!");
            }
            else
                MessageBox.Show("Project Could Not Be Uploaded!");
            foreach(string file in editFiles)
            {
                if (!editFilesCB.Items.Contains(file))
                {
                    editFilesCB.Items.Add(file);
                }
            }
        }

        private void EditFiles_Click(object sender, RoutedEventArgs e)
        {
            string editedFile = server.GetFilePath(projectEdit.SelectedItem.ToString(), user, editFilesCB.SelectedItem.ToString());

            EditWindow editFiles = new EditWindow(editedFile);
            this.Visibility = Visibility.Hidden;
            editFiles.Show();
        }

        private void ViewFiles_Click(object sender, RoutedEventArgs e)
        {
            ViewWindow viewWindow = new ViewWindow();
            this.Visibility = Visibility.Hidden;
            viewWindow.Show();
        }

        private void DownloadFiles_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GenerateProject_Click(object sender, RoutedEventArgs e)
        {
            string projectPath = server.GetFullDestinationPath(projectGenerate.SelectedItem.ToString(), user);
            server.DocumentationGenerator(projectPath);
        }
    }
}
