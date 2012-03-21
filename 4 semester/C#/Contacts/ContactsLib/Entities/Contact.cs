using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace ContactsLib.Entities
{
    [DataContract]
    public class Contact : IComparable<Contact>, INotifyPropertyChanged
    {
        [DataMember] private ObservableCollection<ContactDetail> _Details = new ObservableCollection<ContactDetail>();
        [DataMember] private string _Name;

        public Contact(string name)
        {
            Name = name;
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
    }
}