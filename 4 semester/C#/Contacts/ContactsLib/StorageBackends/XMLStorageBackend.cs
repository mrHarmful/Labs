using System;
using System.IO;
using System.Runtime.Serialization;

namespace ContactsLib.StorageBackends
{
    public class XMLStorageBackend : StorageBackend
    {
        public override void Store(ContactList list, object descriptor)
        {
            FileStream fs = null;
            try
            {
                fs = File.Open(descriptor as string, FileMode.Create);
            }
            catch
            {
                throw new ArgumentException("Invalid file name");
            }

            try
            {
                var dcs = new DataContractSerializer(typeof (ContactList));
                dcs.WriteObject(fs, list);
            }
            finally
            {
                fs.Close();
            }
        }

        public override ContactList Load(object descriptor)
        {
            FileStream fs = null;
            try
            {
                fs = File.Open(descriptor as string, FileMode.Open, FileAccess.Read);
            }
            catch
            {
                throw new ArgumentException("Invalid file name");
            }

            try
            {
                var dcs = new DataContractSerializer(typeof (ContactList));
                var result = (ContactList) dcs.ReadObject(fs);
                return result;
            }
            finally
            {
                fs.Close();
            }
        }
    }
}