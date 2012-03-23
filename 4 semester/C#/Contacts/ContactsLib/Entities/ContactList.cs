using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using ContactsLib.Entities;

namespace ContactsLib
{
    [DataContract]
    public class ContactList : INotifyPropertyChanged, IDeserializationCallback
    {
        internal static ContactList Instance;
        private ObservableCollection<ContactGroup> _Groups = new ObservableCollection<ContactGroup>();

        public ContactList()
        {
            Instance = this;
        }

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
            get { return GetGroup("Ungrouped"); }
        }

        public IEnumerable<Contact> Contacts
        {
            get { return Groups.SelectMany(g => g.Contacts); }
        }

        #region IDeserializationCallback Members

        public void OnDeserialization(object sender)
        {
            Instance = this;
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #endregion

        public void InvalidateContactList()
        {
            PropertyChanged(this, new PropertyChangedEventArgs("Contacts"));
        }

        public ContactGroup GetGroup(string name)
        {
            ContactGroup g = Groups.FirstOrDefault(x => x.Name == name);
            if (g == null)
            {
                g = new ContactGroup(name);
                Groups.Add(g);
            }
            return g;
        }

        public ContactGroup GetGroupOf(Contact c)
        {
            return Groups.FirstOrDefault(x => x.Contains(c));
        }

        public void Remove(Contact c)
        {
            GetGroupOf(c).Contacts.Remove(c);
            var deadGroups = new List<ContactGroup>();
            foreach (ContactGroup g in Groups)
                if (g.Contacts.Count == 0)
                    deadGroups.Add(g);
            foreach (ContactGroup g in deadGroups)
                Groups.Remove(g);
        }

        public void Add(Contact contact, string g)
        {
            GetGroup(g).Contacts.Add(contact);
            InvalidateContactList();
        }
    }
}