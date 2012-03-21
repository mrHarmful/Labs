using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace ContactsLib.Entities
{
    [DataContract]
    public class Contact : IComparable<Contact>, INotifyPropertyChanged
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

        public string Group
        {
            get { 
                return ContactList.Instance.GetGroupOf(this).Name; 
            }
        }

        [DataMember]
        public List<ContactDetail> Details = new List<ContactDetail>();

        public Contact(string name)
        {
            Name = name;
        }

        public int CompareTo(Contact other)
        {
            return Name.CompareTo(other.Name);
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
