using System.Collections.Generic;

namespace Library.Core
{
    public static class CollectionExtensions
    {
        public static void Reset<T>(this ICollection<T> collection, IEnumerable<T> newContents)
        {
            collection.Clear();
            foreach (var newContent in newContents)
            {
                collection.Add(newContent);
            }
        }
    }
}
