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

namespace ContactsApp
{
    public partial class App : Application
    {
        private ContactList ContactList;
        private Contact CurrentContact;
        private MainWindow mainWindow;
        private string FileName = null;

        public void NewContactList()
        {
            FileName = null;
            ContactList = new ContactList();
            FillContactListbox();
            SelectContact(-1);
        }

        public bool LoadContactList()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "XML Files|*.xml";
            if (ofd.ShowDialog().Value)
            {
                try
                {
                    ContactList = ContactList.Load<XMLStorageBackend>(ofd.FileName);
                    FillContactListbox();
                    SelectContact(-1);
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

            ContactList.Store<XMLStorageBackend>(FileName);
            return true;
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            mainWindow = new MainWindow(this);
            mainWindow.Show();
            NewContactList();
        }

        public void SelectContact(int id)
        {
            if (id == -1)
                CurrentContact = null;
            else
                CurrentContact = ContactList[id];

            FillDetails();
            mainWindow.AddDetailButton.Visibility = (id == -1) ? Visibility.Collapsed : Visibility.Visible;
        }

        public void FillDetails()
        {
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
            EditableField eff = new EditableField();
            eff.Changed += ContactGroup_Changed;
            eff.Label.Content = CurrentContact.Group;
            eff.Tag = CurrentContact;
            eff.Deletable = false;
            mainWindow.ContactDetails.Children.Add(eff);

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

        }

        void ef_Deleted(EditableField obj)
        {
            CurrentContact.Details.Remove(obj.Tag as SimpleContactDetail);
            FillDetails();
        }

        void ContactGroup_Changed(EditableField sender, string value)
        {
            Contact c = sender.Tag as Contact;
            c.Group = value;
            FillContactListbox();
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
            mainWindow.listBox.Children.Clear();
            foreach (ContactGroup g in ContactList.Groups)
            {
                Expander exp = new Expander();
                exp.Header = (g.Name != null) ? g.Name : "Ungrouped";
                exp.IsExpanded = true;
                mainWindow.listBox.Children.Add(exp);

                ListBox lbx = new ListBox();
                lbx.SelectionChanged += mainWindow.listBox_SelectionChanged;
                lbx.BorderThickness = new Thickness(0);
                exp.Content = lbx;

                foreach (Contact c in ContactList.Sorted)
                    if (c.Group == g.Name)
                    {
                        ContactListItem i = new ContactListItem();
                        i.PersonName.Content = c.Name;
                        i.PersonName.FontWeight = FontWeights.Bold;
                        try
                        {
                            i.Description.Content = c.Details[0].Content;
                        }
                        catch { }
                        i.Tag = ContactList.Contacts.IndexOf(c);
                        lbx.Items.Add(i);
                    }
            }
        }

        public void CreateContact(string name)
        {
            Contact c = new Contact(name);
            ContactList.Add(c);
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
            SelectContact(-1);
            FillContactListbox();
        }
    }
}
