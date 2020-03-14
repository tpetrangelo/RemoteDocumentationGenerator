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
        bool comboItemExists = false;
        public PostLogin(string userName)
        {
            user = userName;
            InitializeComponent();
            userProjects = server.PopulateProjects(userName);
            foreach (string project in userProjects)
            {            
                projectOptions.Items.Add(project);
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
                }
            }

        }

        private void BrowseFiles_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.DefaultExt = "*.cs";
            openFileDialog.ShowDialog();
        }

        private void UploadFile_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditFiles_Click(object sender, RoutedEventArgs e)
        {
            EditWindow editFiles = new EditWindow();
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

        private void PopulateProjects()
        {

        }
    }
}
