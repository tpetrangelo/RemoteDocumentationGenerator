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

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            PostLogin postLogin = new PostLogin(user);
            this.Visibility = Visibility.Hidden;
            postLogin.Show();
        }
    }
}
