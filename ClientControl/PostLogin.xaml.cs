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

namespace RemoteDocumentationGenerator
{
    /// <summary>
    /// Interaction logic for PrimaryWindow.xaml
    /// </summary>
    public partial class PostLogin : Window
    {
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
            Login mainWindow = new Login();
            this.Visibility = Visibility.Hidden;
            mainWindow.Show();
        }

        private void CreateProject_Click(object sender, RoutedEventArgs e)
        {

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
    }
}
