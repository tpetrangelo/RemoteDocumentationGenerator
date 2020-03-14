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
        public ViewWindow(string userName)
        {
            InitializeComponent();
            user = userName;
        }
        
        public ViewWindow()
        {
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            PostLogin postLogin = new PostLogin(null);
            this.Visibility = Visibility.Hidden;
            postLogin.Show();
        }
    }
}
