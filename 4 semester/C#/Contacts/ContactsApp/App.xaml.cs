using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using ContactsLib;
using Microsoft.Win32;
using ContactsLib.StorageBackends;
using ContactsLib.Entities;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel;

namespace ContactsApp
{
    public partial class App : Application
    {
        private void OnStartup(object sender, StartupEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(new Controller());
            mainWindow.Show();
        }
    }
}
