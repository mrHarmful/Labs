using System.Windows;
using ContactsApp.Properties;

namespace ContactsApp
{
    /// <summary>
    /// Interaction logic for StartupWindow.xaml
    /// </summary>
    public partial class StartupWindow
    {
        public StartupWindow()
        {
            InitializeComponent();
            StringBox.Text = Settings.Default.ConnectionString;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.ConnectionString = StringBox.Text;
            var mainWindow = new MainWindow(new Controller(StringBox.Text));
            mainWindow.Show();
            Hide();
        }
    }
}