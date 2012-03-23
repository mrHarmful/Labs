namespace ContactsLib.StorageBackends
{
    public abstract class StorageBackend
    {
        public abstract void Store(ContactList list, object descriptor);
        public abstract ContactList Load(object descriptor);
    }
}