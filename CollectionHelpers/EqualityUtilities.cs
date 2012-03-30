using System.Collections.Generic;
using System.Linq;

namespace CollectionHelpers
{
    public static class EqualityUtilities
    {
        public static bool CollectionEquals<T>(this IEnumerable<T> objA, IEnumerable<T> objB)
        {
            if (objA == objB)
                return true;
            if (objA == null || objB == null) return false;
            else
                return objA.SequenceEqual(objB);
        }

        public static int CollectionGetHashCode<T>(this IEnumerable<T> collection)
        {
            return collection.Sum(i => i.GetHashCode());
        }
    }
}