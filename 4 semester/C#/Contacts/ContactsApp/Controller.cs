using System.Windows;
using ContactsLib;
using ContactsLib.Entities;
using ContactsLib.StorageBackends;
using Microsoft.Win32;

namespace ContactsApp
{
    public class Controller : DependencyObject
    {
        public static DependencyProperty ContactListProperty = DependencyProperty.Register("ContactList",
                                                                                           typeof (ContactList),
                                                                                           typeof (Controller));

        public static DependencyProperty CurrentContactProperty = DependencyProperty.Register("CurrentContact",
                                                                                              typeof (Contact),
                                                                                              typeof (Controller));

        public Controller()
        {
            NewContactList();
        }

        public ContactList ContactList
        {
            get { return (ContactList) GetValue(ContactListProperty); }
            set { SetValue(ContactListProperty, value); }
        }

        public Contact CurrentContact
        {
            get { return (Contact) GetValue(CurrentContactProperty); }
            set { SetValue(CurrentContactProperty, value); }
        }

        private string FileName { get; set; }

        public void NewContactList()
        {
            FileName = null;
            ContactList = new ContactList();
        }

        public bool LoadContactList()
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "XML Files|*.xml";
            if (ofd.ShowDialog().Value)
            {
                try
                {
                    ContactList = new XMLStorageBackend().Load(ofd.FileName);
                    FileName = ofd.FileName;
                    return true;
                }
                catch
                {
                    MessageBox.Show("Failed to load contact list", "Contacts", MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }
            }
            return false;
        }

        public bool SaveContactList()
        {
            if (FileName == null)
            {
                var sfd = new SaveFileDialog();
                sfd.Filter = "XML Files|*.xml";
                if (!sfd.ShowDialog().Value)
                    return false;
                FileName = sfd.FileName;
            }

            new XMLStorageBackend().Store(ContactList, FileName);
            return true;
        }

        public void CreateContact(string name)
        {
            var c = new Contact(name);
            ContactList.DefaultGroup.Contacts.Add(c);
        }

        public void AddSimpleDetail(string title, string value)
        {
            CurrentContact.Details.Add(new ContactDetail(title, value));
        }

        public void RemoveContact()
        {
            ContactList.Remove(CurrentContact);
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