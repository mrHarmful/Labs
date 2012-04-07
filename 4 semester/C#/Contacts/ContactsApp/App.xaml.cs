using System.Windows;

namespace ContactsApp
{
    public partial class App : Application
    {
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = new MainWindow(new Controller("Data Source=hv02.linology.info;Initial Catalog=Contacts;User Id=admin;Password=admin;MultipleActiveResultSets=yes"));
            //var mainWindow = new MainWindow(new Controller("Driver={sql native client};Provider=SQLNCLI; MARS_Connection=yes; Server=hv02.linology.info; Database=Contacts; Uid=admin;Pwd=admin;"));
            
            mainWindow.Show();
        }
    }
}