using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ContactsLib.Entities
{
    [DataContract]
    public class ContactGroup : IEnumerable<Contact>
    {
        [DataMember]
        public string Name;
        
        [DataMember]
        public List<Contact> Contacts = new List<Contact>();

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
    }
}
