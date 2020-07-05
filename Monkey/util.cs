namespace util
{
    class hash
    {
        // Taken from CLOXCS
        public static uint hashString(string key)
        {
            uint hash = 2166136261u;
            for (int i = 0; i < key.Length; i++)
            {
                hash ^= key[i];
                hash *= 16777619;
            }
            return hash;
        }
    }
}
