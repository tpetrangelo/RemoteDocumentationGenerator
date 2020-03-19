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
        public EditWindow(string fileLoc, string username)
        {
            
            InitializeComponent();
            this.Dispatcher.Invoke(() =>
            {
                editText.Text = System.IO.File.ReadAllText(fileLoc);
            });
            _username = username;
            file = fileLoc;
        }

        public EditWindow()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            StringCollection lines = new StringCollection();
            int lineCount = editText.LineCount;
            for(int line = 0; line < lineCount; line++)
            {
                lines.Add(editText.GetLineText(line));
            }
            
            server.SaveFile(file, lines);
            PostLogin postLogin = new PostLogin(_username);
            this.Visibility = Visibility.Hidden;
            postLogin.Show();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            PostLogin postLogin = new PostLogin(_username);
            this.Visibility = Visibility.Hidden;
            postLogin.Show();
        }
    }
}
