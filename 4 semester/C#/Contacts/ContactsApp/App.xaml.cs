﻿using System;
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

            mainWindow.CurrentContactName.Content = (CurrentContact != null) ? CurrentContact.Name : "";

            if (CurrentContact.Details != null)
                foreach (SimpleContactDetail scd in CurrentContact.Details)
                {
                    Label l = new Label();
                    l.Content = scd.Name; ;
                    l.FontWeight = FontWeights.Bold;
                    mainWindow.ContactDetails.Children.Add(l);
                    EditableField ef = new EditableField();
                    ef.Label.Content = scd.Content;
                    ef.Changed += new EditableField.OnChanged(ef_Changed);
                    ef.Tag = scd;
                    mainWindow.ContactDetails.Children.Add(ef);
                }
        }

        public void ef_Changed(EditableField sender, string value)
        {
            (sender.Tag as SimpleContactDetail).Content = value;
            FillDetails();
        }

        private void FillContactListbox()
        {
            mainWindow.listBox.Items.Clear();
            foreach (Contact c in ContactList.Sorted)
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
                mainWindow.listBox.Items.Add(i);
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
