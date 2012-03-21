using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace ContactsLib.Entities
{
    [DataContract]
    public class ContactGroup : IEnumerable<Contact>, INotifyPropertyChanged, IDeserializationCallback
    {
        private ObservableCollection<Contact> _Contacts = new ObservableCollection<Contact>();
        private string _Name;

        public ContactGroup(string name)
        {
            Name = name;
            Init();
        }

        [DataMember]
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("_Name"));
            }
        }

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

        #region IDeserializationCallback Members

        public void OnDeserialization(object sender)
        {
            Init();
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #endregion

        private void Init()
        {
            Contacts.CollectionChanged += delegate { ContactList.Instance.InvalidateContactList(); };
        }

        public override string ToString()
        {
            return Name;
        }

        #region IEnumerable

        public IEnumerator<Contact> GetEnumerator()
        {
            return Contacts.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Contacts.GetEnumerator();
        }

        #endregion
    }
}