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
    /// Interaction logic for EditableField.xaml
    /// </summary>
    public partial class EditableField : UserControl
    {
        public delegate void OnChanged(EditableField sender, string value);
        public event OnChanged Changed = delegate { };

        public EditableField()
        {
            InitializeComponent();
        }

        private void Label_Click(object sender, RoutedEventArgs e)
        {
            Label.Visibility = System.Windows.Visibility.Collapsed;
            TextBox.Visibility = System.Windows.Visibility.Visible;
            TextBox.Text = Label.Content as string;
        }

        private void Hide()
        {
            Label.Visibility = System.Windows.Visibility.Visible;
            TextBox.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Changed(this, TextBox.Text);
                Label.Content = TextBox.Text;
                Hide();
            }
            if (e.Key == Key.Escape)
                Hide();
        }
    }
}
