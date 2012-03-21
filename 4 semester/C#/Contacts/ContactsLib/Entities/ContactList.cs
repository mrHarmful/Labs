using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContactsLib.Entities;
using System.Runtime.Serialization;
using ContactsLib.StorageBackends;
using System.Collections;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace ContactsLib
{
    [DataContract]
    public class ContactList : INotifyPropertyChanged, IDeserializationCallback
    {
        private ObservableCollection<ContactGroup> _Groups = new ObservableCollection<ContactGroup>();
        [DataMember]
        public ObservableCollection<ContactGroup> Groups
        {
            get { return _Groups; }
            set
            {
                _Groups = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Groups"));
            }
        }

        public ContactGroup DefaultGroup
        {
            get
            {
                return GetGroup("Ungrouped");
            }
        }

        public IEnumerable<Contact> Contacts
        {
            get
            {
                return Groups.SelectMany<ContactGroup, Contact>(g => g.Contacts);
            }
        }

        internal static ContactList Instance;
        public ContactList()
        {
            Instance = this;
        }

        public void InvalidateContactList()
        {
            PropertyChanged(this, new PropertyChangedEventArgs("Contacts"));
        }

        public ContactGroup GetGroup(string name)
        {
            ContactGroup g = Groups.FirstOrDefault<ContactGroup>(x => x.Name == name);
            if (g == null)
            {
                g = new ContactGroup(name);
                Groups.Add(g);
            }
            return g;
        }

        public ContactGroup GetGroupOf(Contact c)
        {
            return Groups.FirstOrDefault<ContactGroup>(x => x.Contains(c));
        }

        public void Remove(Contact c)
        {
            GetGroupOf(c).Contacts.Remove(c);
            List<ContactGroup> deadGroups = new List<ContactGroup>();
            foreach (ContactGroup g in Groups)
                if (g.Contacts.Count == 0)
                    deadGroups.Add(g);
            foreach (ContactGroup g in deadGroups)
                Groups.Remove(g);
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void OnDeserialization(object sender)
        {
            Instance = this;
        }

        public void Add(Contact contact, string g)
        {
            GetGroup(g).Contacts.Add(contact);
            InvalidateContactList();
        }
    }
}
