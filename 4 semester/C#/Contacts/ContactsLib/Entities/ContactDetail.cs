using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace ContactsLib.Entities
{
    [DataContract]
    public class ContactDetail : INotifyPropertyChanged
    {
        [DataMember] private string _Content;
        [DataMember] public String _Name;

        public ContactDetail(string name, string value)
        {
            Name = name;
            Content = value;
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
    }
}