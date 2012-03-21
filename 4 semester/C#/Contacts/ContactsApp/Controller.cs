using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using ContactsLib;
using ContactsLib.Entities;
using Microsoft.Win32;
using ContactsLib.StorageBackends;

namespace ContactsApp
{
    public class Controller : DependencyObject
    {
        private MainWindow mainWindow;

        public Controller()
        {
            NewContactList();
        }

        public static DependencyProperty ContactListProperty = DependencyProperty.Register("ContactList", typeof(ContactList), typeof(Controller));
        public ContactList ContactList
        {
            get { return (ContactList)GetValue(ContactListProperty); }
            set { SetValue(ContactListProperty, value); }
        }

        public static DependencyProperty CurrentContactProperty = DependencyProperty.Register("CurrentContact", typeof(Contact), typeof(Controller));
        public Contact CurrentContact
        {
            get { return (Contact)GetValue(CurrentContactProperty); }
            set { SetValue(CurrentContactProperty, value); }
        }

        private string FileName = null;

        public void NewContactList()
        {
            FileName = null;
            ContactList = new ContactList();
            FillContactListbox();
            SelectContact(null);
        }

        public bool LoadContactList()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "XML Files|*.xml";
            if (ofd.ShowDialog().Value)
            {
                try
                {
                    ContactList = new XMLStorageBackend().Load(ofd.FileName);
                    FillContactListbox();
                    SelectContact(null);
                    FileName = ofd.FileName;
                    return true;
                }
                catch
                {
                    MessageBox.Show("Failed to load contact list", "Contacts", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return false;
        }

        public bool SaveContactList()
        {
            if (FileName == null)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "XML Files|*.xml";
                if (!sfd.ShowDialog().Value)
                    return false;
                FileName = sfd.FileName;
            }

            new XMLStorageBackend().Store(ContactList, FileName);
            return true;
        }

        public void SelectContact(Contact c)
        {
            CurrentContact = c;
        }

        public void FillDetails()
        {
            /*
            mainWindow.ContactDetails.Children.Clear();
            if (CurrentContact == null)
                return;

            EditableField efn = new EditableField();
            efn.Changed += ContactName_Changed;
            efn.Label.Content = CurrentContact.Name;
            efn.Label.FontSize = 32;
            efn.Tag = CurrentContact;
            efn.Deletable = false;
            mainWindow.ContactDetails.Children.Add(efn);

            Label ll = new Label();
            ll.Content = "Group";
            ll.FontWeight = FontWeights.Bold;
            mainWindow.ContactDetails.Children.Add(ll);

            ComboBox cbx = new ComboBox();
            cbx.IsEditable = true;
            cbx.KeyUp += ContactGroup_Changed;
            cbx.Text = ContactList.GetGroupOf(CurrentContact).Name;
            cbx.ItemsSource = ContactList.Groups;
            mainWindow.ContactDetails.Children.Add(cbx);


            if (CurrentContact.Details == null)
                CurrentContact.Details = new List<ContactDetail>();

            foreach (SimpleContactDetail scd in CurrentContact.Details)
            {
                Label l = new Label();
                l.Content = scd.Name; ;
                l.FontWeight = FontWeights.Bold;
                mainWindow.ContactDetails.Children.Add(l);
                EditableField ef = new EditableField();
                ef.Label.Content = scd.Content;
                ef.Changed += ef_Changed;
                ef.Deleted += ef_Deleted;
                ef.Tag = scd;
                mainWindow.ContactDetails.Children.Add(ef);
            }
            */
        }

        void ef_Deleted(EditableField obj)
        {
            CurrentContact.Details.Remove(obj.Tag as SimpleContactDetail);
            FillDetails();
        }

        void ContactName_Changed(EditableField sender, string value)
        {
            Contact c = sender.Tag as Contact;
            c.Name = value;
            FillContactListbox();
            FillDetails();
        }

        public void ef_Changed(EditableField sender, string value)
        {
            (sender.Tag as SimpleContactDetail).Content = value;
            FillContactListbox();
            FillDetails();
        }

        private void FillContactListbox()
        {
        }

        public void CreateContact(string name)
        {
            Contact c = new Contact(name);
            ContactList.DefaultGroup.Contacts.Add(c);
            FillContactListbox();
        }

        public void AddSimpleDetail(string title, string value)
        {
            CurrentContact.Details.Add(new SimpleContactDetail(title, value));
            FillDetails();
        }

        public void RemoveContact()
        {
            ContactList.Remove(CurrentContact);
            SelectContact(null);
            FillContactListbox();
        }


        public void ChangeContactGroup(Contact contact, string g)
        {
            if (contact.Group != g)
            {
                ContactList.Remove(contact);
                ContactList.Add(contact, g);
                CurrentContact = contact;
            }
        }
    }
}
