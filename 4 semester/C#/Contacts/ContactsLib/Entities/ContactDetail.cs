using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Runtime.Serialization;

namespace ContactsLib.Entities
{
    [DataContract]
    public class ContactDetail : INotifyPropertyChanged
    {
        internal long ID = -1;
        [DataMember] private string _Content;
        [DataMember] public String _Name;

        public ContactDetail(string name, string value)
        {
            Name = name;
            Content = value;
            PropertyChanged += delegate
                                   {
                                       if (ContactList.Instance.isLoading)
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
                PropertyChanged(this, new PropertyChangedEventArgs("Name"));
            }
        }

        public string Content
        {
            get { return _Content; }
            set
            {
                _Content = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Content"));
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #endregion

        public void Persist()
        {
            Contact c = null;
            foreach (Contact contact in ContactList.Instance.Contacts)
            {
                if (contact.Details.Contains(this))
                {
                    c = contact;
                    break;
                }
            }
            if (c == null) return;

            if (ID == -1)
            {
                ID = (int) (new SqlCommand(
                               String.Format(
                                   "INSERT INTO ContactDetails (Title, Value, Contact_id) OUTPUT INSERTED.id VALUES ('{0}', '{1}', {2})",
                                   Name, Content, c.ID)
                               , ContactList.Instance.Connection).ExecuteScalar());
            }
            else
            {
                new SqlCommand(
                    String.Format(
                        "UPDATE ContactDetails SET Title = '{0}', Value = '{2}', Contact_id = {3} WHERE id = {1}",
                        Name, ID, Content, c.ID), ContactList.Instance.Connection).
                    ExecuteNonQuery();
            }
        }

        public void Destroy()
        {
            if (ContactList.Instance.isLoading)
                return;

            new SqlCommand(
                String.Format("DELETE FROM ContactDetails WHERE id = {0}",
                              ID), ContactList.Instance.Connection).
                ExecuteNonQuery();
        }
    }
}