using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Runtime.Serialization;

namespace ContactsLib.Entities
{
    [DataContract]
    public class ContactGroup : IEnumerable<Contact>, INotifyPropertyChanged, IDeserializationCallback
    {
        internal long ID = -1;
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

        public void Persist()
        {
            if (ID == -1)
            {
                ID = (int) (new SqlCommand(
                               String.Format("INSERT INTO ContactGroups (Name) OUTPUT INSERTED.id VALUES ('{0}')", Name)
                               , ContactList.Instance.Connection).ExecuteScalar());
            }
            else
            {
                new SqlCommand(
                    String.Format("UPDATE ContactGroups SET Name = '{0}' WHERE id = {1}",
                                  Name, ID), ContactList.Instance.Connection).
                    ExecuteNonQuery();
            }
        }

        public void Destroy()
        {
            if (ContactList.Instance.isLoading)
                return;

            new SqlCommand(
                String.Format("DELETE FROM ContactGroups WHERE id = {0}",
                              ID), ContactList.Instance.Connection).
                ExecuteNonQuery();
        }

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