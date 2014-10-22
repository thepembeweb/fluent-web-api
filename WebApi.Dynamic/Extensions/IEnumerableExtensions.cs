// ReSharper disable once CheckNamespace
namespace System.Collections.Generic
{
    // ReSharper disable once InconsistentNaming
    public static class IEnumerableExtensions
    {
        public static IList<TSource> ToListOrNull<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
            {
                return null;
            }

            return new List<TSource>(source);
        }
    }
}