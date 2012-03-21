using System.Windows;

namespace ContactsApp
{
    public partial class App : Application
    {
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = new MainWindow(new Controller());
            mainWindow.Show();
        }
    }
}