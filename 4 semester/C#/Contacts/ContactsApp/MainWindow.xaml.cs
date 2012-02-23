using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ContactsApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public App Controller;

        public MainWindow(App c)
        {
            Controller = c;
            InitializeComponent();
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            Controller.LoadContactList();
        }

        internal void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Controller.SelectContact((int)(((sender as ListBox).SelectedItem as FrameworkElement).Tag));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddContactPanel.Visibility = Visibility.Collapsed;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddContactPanel.Visibility = Visibility.Visible;
            AddContactName.Text = "";
            AddContactName.Focus();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (AddContactName.Text.Length > 0)
            {
                AddContactPanel.Visibility = Visibility.Collapsed;
                Controller.CreateContact(AddContactName.Text);
                AddContactName.Text = "";
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            Controller.RemoveContact();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Controller.SaveContactList();
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            Controller.NewContactList();
            AddDetailPopup.IsOpen = false;
        }

        private void AddDetail_Click(object sender, RoutedEventArgs e)
        {
            AddDetailPanel.Visibility = System.Windows.Visibility.Visible;
            AddDetailTitle.Text = ((FrameworkElement)sender).Tag as string;
            AddDetailValue.Text = "";
            AddDetailValue.Focus();
            AddDetailPopup.IsOpen = false;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Controller.AddSimpleDetail(AddDetailTitle.Text, AddDetailValue.Text);
            AddDetailTitle.Text = "";
            AddDetailValue.Text = "";
            AddDetailPanel.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            AddDetailTitle.Text = "";
            AddDetailValue.Text = "";
            AddDetailPanel.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void AddDetailButton_Click(object sender, RoutedEventArgs e)
        {
            AddDetailPopup.IsOpen = true;
        }
    }
}
