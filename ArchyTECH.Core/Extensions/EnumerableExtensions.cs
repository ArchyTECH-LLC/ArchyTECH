using System.Linq.Expressions;

namespace ArchyTECH.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool None<T>(this IEnumerable<T> items)
        {
            return items.Any() == false;
        }

        public static bool None<T>(this IEnumerable<T> items, Func<T, bool> filterExpression)
        {
            return items.Any(filterExpression) == false;
        }

        public static T Single<T>(this IEnumerable<T> items, Func<T, bool> filter, string errorMessage)
        {
            if (items.Count() == 1) return items.Single(filter);
            throw new InvalidOperationException(errorMessage);
        }

    }
}