using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContactsLib.Entities;
using System.Runtime.Serialization;
using ContactsLib.StorageBackends;
using System.Collections;

namespace ContactsLib
{
    [DataContract]
    public class ContactList
    {
        [DataMember]
        public List<ContactGroup> Groups = new List<ContactGroup>();

        public ContactGroup DefaultGroup
        {
            get
            {
                ContactGroup g = GetGroup("Ungrouped");
                if (g == null)
                {
                    g = new ContactGroup("Ungrouped");
                    Groups.Add(g);
                }
                return g;
            }
        }

        public ContactGroup GetGroup(string name)
        {
            try
            {
                return Groups.First<ContactGroup>(x => x.Name == name);
            }
            catch { return null; }
        }

        public ContactGroup GetGroupOf(Contact c)
        {
            try
            {
                return Groups.First<ContactGroup>(x => x.Contains(c));
            }
            catch { return null; }
        }

        public void Remove(Contact c)
        {
            GetGroupOf(c).Contacts.Remove(c);
            List<ContactGroup> deadGroups = new List<ContactGroup>();
            foreach (ContactGroup g in Groups)
                if (g.Contacts.Count == 0)
                    deadGroups.Add(g);
            foreach (ContactGroup g in deadGroups)
                Groups.Remove(g);
        }
    }
}
