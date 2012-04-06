using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using ContactsLib.Entities;

namespace ContactsLib
{
    [DataContract]
    public class ContactList : INotifyPropertyChanged, IDeserializationCallback
    {
        public static ContactList Instance;
        private ObservableCollection<ContactGroup> _Groups = new ObservableCollection<ContactGroup>();
        private SqlConnection _connection;
        internal bool isLoading;

        public ContactList(string conn)
        {
            Instance = this;
            Connection = new SqlConnection(conn);
        }

        internal SqlConnection Connection
        {
            get
            {
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }
                return _connection;
            }
            set { _connection = value; }
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

        public Dictionary<string, int> GetGroupStats()
        {
            var r = new Dictionary<string, int>();
            SqlDataReader reader = new SqlCommand("EXEC GroupStats", Connection).ExecuteReader();
            while (reader.Read())
            {
                string gname = new SqlCommand("SELECT Name FROM ContactGroups WHERE id=" + reader.GetInt32(0), Connection).ExecuteScalar() as string;
                r[gname] = reader.GetInt32(1);
            }
            return r;
        }

        public void Reload()
        {
            isLoading = true;

            Groups.Clear();
            SqlDataReader reader = new SqlCommand("SELECT * FROM ContactGroups", Connection).ExecuteReader();
            Console.WriteLine("Loading database");

            while (reader.Read())
            {
                string name = reader.GetString(1);
                var group = new ContactGroup(name);
                Console.WriteLine("Loading group {0}", name);
                Groups.Add(group);
                group.ID = reader.GetInt32(0);

                SqlDataReader greader =
                    new SqlCommand("SELECT * FROM Contacts WHERE ContactGroup_id=" + reader.GetInt32(0), Connection).
                        ExecuteReader();
                while (greader.Read())
                {
                    var contact = new Contact(greader.GetString(1));
                    contact.ID = greader.GetInt32(0);
                    Console.WriteLine("Loading contact {0}", contact.Name);

                    SqlDataReader dreader =
                        new SqlCommand("SELECT * FROM ContactDetails WHERE Contact_id=" + greader.GetInt32(0),
                                       Connection).ExecuteReader();
                    while (dreader.Read())
                    {
                        var detail = new ContactDetail(dreader.GetString(1), dreader.GetString(2));
                        detail.ID = dreader.GetInt32(0);
                        Console.WriteLine("Loading detail {0}", detail.Name);
                        contact.Details.Add(detail);
                    }

                    group.Contacts.Add(contact);
                }
            }
            isLoading = false;
        }

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
                g.Persist();
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
            {
                g.Destroy();
                Groups.Remove(g);
            }
        }

        public void Add(Contact contact, string g)
        {
            GetGroup(g).Contacts.Add(contact);
            contact.Persist();
            InvalidateContactList();
        }
    }
}