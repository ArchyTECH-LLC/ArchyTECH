using ArchyTECH.Core.Exceptions;

namespace ArchyTECH.Core.Extensions
{
    public static class EnumerableExtensions
    {


        /// <summary>
        /// Specifies a conditional filter for an enumerable collection that will only be executed when true
        /// </summary>
        /// <param name="enumerable">The collection to filter</param>
        /// <param name="condition">Whether or not to execute the predicate</param>
        /// <param name="predicate">The expression whether the item should be returned in the results</param>
        /// <returns>A FilteredEnumerable that has not be enumerated yet</returns>
        public static FilteredEnumerable<T> WhereIf<T>(
            this IEnumerable<T> enumerable,
            bool condition,
            Func<T, bool> predicate)
        {

            var delayed = enumerable as FilteredEnumerable<T> ?? new FilteredEnumerable<T>(enumerable);

            return delayed.WhereIf(condition, predicate);
        }

        /// <summary>
        /// Specifies a conditional filter for an enumerable collection that will only be executed when true
        /// </summary>
        /// <param name="enumerable">The collection to filter</param>
        /// <param name="condition">Whether or not to execute the predicate</param>
        /// <param name="predicate">The expression whether the item should be returned in the results</param>
        /// <returns>A FilteredEnumerable that has not be enumerated yet</returns>
        public static FilteredEnumerable<T> WhereIf<T>(
            this FilteredEnumerable<T> enumerable,
            bool condition,
            Func<T, bool> predicate)
        {

            if (condition) 
                enumerable.Predicates.Add(predicate);

            return enumerable;
        }
        
        public static bool None<T>(this IEnumerable<T> items)
        {
            return items.Any() == false;
        }

        public static bool None<T>(this IEnumerable<T> items, Func<T, bool> filterExpression)
        {
            return items.Any(filterExpression) == false;
        }

        public static T Single<T>(this IEnumerable<T> items, Func<T, bool> filter, 
            string moreThanOneErrorMessage,
            string notFoundErrorMessage)
        {

            T? first = default;
            foreach (var item in items)
            {
                if (first == null) first = item;
                else throw ValidationException.MultipleFound(moreThanOneErrorMessage);
            }

            if (first != null) return first;

            throw ValidationException.NotFound(notFoundErrorMessage);
        }

    }
}