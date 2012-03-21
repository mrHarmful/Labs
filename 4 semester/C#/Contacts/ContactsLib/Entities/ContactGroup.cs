using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace ContactsLib.Entities
{
    [DataContract]
    public class ContactGroup : IEnumerable<Contact>, INotifyPropertyChanged, IDeserializationCallback
    {
        private string _Name;
        [DataMember]
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Name"));
            }
        }

        private ObservableCollection<Contact> _Contacts = new ObservableCollection<Contact>();
        [DataMember]
        public ObservableCollection<Contact> Contacts
        {
            get { return _Contacts; }
            set
            {
                _Contacts = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Contacts"));
            }
        }

        public IEnumerable<Contact> Sorted
        {
            get
            {
                List<Contact> ll = new List<Contact>();
                ll.AddRange(Contacts);
                ll.Sort();
                return ll;
            }
        }

        public ContactGroup(string name)
        {
            Name = name;
            Init();
        }

        #region IEnumerable
        public IEnumerator<Contact> GetEnumerator()
        {
            return Contacts.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Contacts.GetEnumerator();
        }
        #endregion

        public override string ToString()
        {
            return Name;
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void OnDeserialization(object sender)
        {
            Init();
        }

        private void Init()
        {
            Contacts.CollectionChanged += delegate
            {
                ContactList.Instance.InvalidateContactList();
            };
        }
    }
}
