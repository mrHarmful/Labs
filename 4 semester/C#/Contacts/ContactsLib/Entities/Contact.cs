using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Runtime.Serialization;

namespace ContactsLib.Entities
{
    [DataContract]
    public class Contact : IComparable<Contact>, INotifyPropertyChanged
    {
        internal long ID = -1;
        [DataMember] private ObservableCollection<ContactDetail> _Details = new ObservableCollection<ContactDetail>();
        [DataMember] private string _Name;

        public Contact(string name)
        {
            Name = name;
            PropertyChanged += delegate
                                   {
                                       if (ContactList.Instance.IsLoading)
                                           return;

                                       Persist();
                                   };
        }

        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                PropertyChanged(this, new PropertyChangedEventArgs("_Name"));
            }
        }

        public ObservableCollection<ContactDetail> Details
        {
            get { return _Details; }
            set
            {
                _Details = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Details"));
            }
        }

        public string Group
        {
            get { return ContactList.Instance.GetGroupOf(this).Name; }
        }

        #region IComparable<Contact> Members

        public int CompareTo(Contact other)
        {
            return Name.CompareTo(other.Name);
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #endregion

        public void Persist()
        {
            ContactGroup g = ContactList.Instance.GetGroupOf(this);
            if (ID == -1)
            {
                if (g == null) return;
                ID = (int) (new SqlCommand(
                               String.Format("INSERT INTO Contacts (Name, ContactGroup_id) OUTPUT INSERTED.id VALUES ('{0}', {1})", Name, g.ID)
                               , ContactList.Instance.Connection).ExecuteScalar());
            }
            else
            {
                new SqlCommand(
                    String.Format("UPDATE Contacts SET Name = '{0}', ContactGroup_id={1} WHERE id = {2}",
                                  Name, g.ID, ID), ContactList.Instance.Connection).
                    ExecuteNonQuery();
            }
        }

        public void Destroy()
        {
            if (ContactList.Instance.IsLoading)
                return;

            new SqlCommand(
                String.Format("DELETE FROM Contacts WHERE id = {0}",
                              ID), ContactList.Instance.Connection).
                ExecuteNonQuery();
        }
    }
}