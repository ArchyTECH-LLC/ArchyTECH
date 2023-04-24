using System.Linq.Expressions;
using System.Transactions;
using ArchyTECH.Core.Results;
using ArchyTECH.EntityFramework.Extensions.DbContext;

namespace ArchyTECH.EntityFramework.Extensions.Extensions
{
    public static class QueryableExtensions
    {
        public static bool None<T>(this IQueryable<T> query)
        {
            return query.Any() == false;
        }
        public static bool None<T>(this IQueryable<T> query, Expression<Func<T, bool>> filterExpression)
        {
            return query.Any(filterExpression) == false;
        }
        
        public static T Single<T>(this IQueryable<T> items, Expression<Func<T, bool>> filter,  string errorMessage)
        {
            var matches = items
                .Where(filter)
                .ToList();

            if (matches.Count == 1) return matches.First();
            throw new InvalidOperationException($"{errorMessage} (Found: {matches.Count})");
        }

        public static PagedDataResult<T> ToPagedResult<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        {
            var pagedResults = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var totalResults = query.Count();

            return new PagedDataResult<T>
            {
                Data = pagedResults,
                Page = pageNumber,
                PageSize = pageSize,
                Total = totalResults
            };
        }
        
        public static PagedDataResult<T> ToPagedResultWithNoLock<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        {

            using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() {IsolationLevel = IsolationLevel.ReadUncommitted}))
            {
                var list =  ToPagedResult(query, pageNumber, pageSize);
                scope.Complete();
                return list;
            }
        }

        public static int CountWithNoLocks<T>(this IQueryable<T> query)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
            {
                var toReturn = query.Count();
                scope.Complete();
                return toReturn;
            }
        }

        public static List<T> ToListWithNoLocks<T>(this IQueryable<T> query)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
            {
                var toReturn = query.ToList();
                scope.Complete();
                return toReturn;
            }
        }

        public static ConditionalQueryable<T> If<T>(this IQueryable<T> query, bool condition)
        {
            return new ConditionalQueryable<T>(query, condition);
        }

        public static ConditionalQueryable<TEntity> IfIsNotNull<TEntity, TProperty>(this IQueryable<TEntity> query, TProperty property)
        {
            var condition = property != null;
            return new ConditionalQueryable<TEntity>(query, condition);
        }

        public static ConditionalQueryable<TEntity> IfIsNull<TEntity, TProperty>(this IQueryable<TEntity> query, TProperty property)
        {
            var condition = property == null;
            return new ConditionalQueryable<TEntity>(query, condition);
        }
        
        //public static ICollection<T> UpdateTo<T, TKey>(this ICollection<T> collection, IEnumerable<T> items, Func<T, TKey> keySelector)
        //{
        //    var identicalItems = collection.Join(items, keySelector, keySelector, (c, i) => c);

        //    var removedItems = collection.Except(identicalItems, keySelector).ToList();
        //    var addedItems = items.Except(identicalItems, keySelector).ToList();

        //    removedItems.ForEach(item => collection.Remove(item));
        //    addedItems.ForEach(collection.Add);

        //    return collection;
        //}

        /// <summary>
        /// This function can be used to add or remove an EF entity's list of items to match a desired list.
        /// Items found in the original list, but not in the destination list will be marked deleted
        /// Items found in the destination list, but not in the original list will be marked inserted
        /// Items that match by the key func will not be modified
        /// SaveChanges() will still need to be called to commit the changes
        /// </summary>
        /// <typeparam name="T">The type of item</typeparam>
        /// <typeparam name="TKey">The type of key used for matching</typeparam>
        /// <param name="collection">The source set of items</param>
        /// <param name="dbContext">The EF context</param>
        /// <param name="destinationItems">What the list should be updated to</param>
        /// <param name="keySelector">The key to be used when comparing objects from each set</param>
        /// <returns>The updated collection</returns>
        public static ICollection<T> AddOrRemoveDbEntities<T, TKey>(
            this ICollection<T> collection, 
            IDbContext dbContext, 
            IEnumerable<T> destinationItems, 
            Func<T, TKey> keySelector) where T : class
        {
            var identicalItems = collection.Join(destinationItems, keySelector, keySelector, (c, i) => c);

            var removedItems = collection.Except(identicalItems, keySelector).ToList();
            var addedItems = destinationItems.Except(identicalItems, keySelector).ToList();

            removedItems.ForEach(item =>
            {
                dbContext.RemoveEntity(item);
                collection.Remove(item);
            });
            addedItems.ForEach(collection.Add);

            return collection;
        }

        public static IEnumerable<T> Except<T, TKey>(this IEnumerable<T> enumerable, IEnumerable<T> comparables, Func<T, TKey> keySelector)
        {
            return enumerable.Except(comparables, new LambdaComparer<T>((x, y) => Equals(keySelector(x), keySelector(y))));
        }

        private class LambdaComparer<T> : IEqualityComparer<T>
        {
            private readonly Func<T, T, bool> _lambdaComparer;
            private readonly Func<T, int> _lambdaHash;

            public LambdaComparer(Func<T, T, bool> lambdaComparer) :
                this(lambdaComparer, o => 0)
            {
            }

            public LambdaComparer(Func<T, T, bool> lambdaComparer, Func<T, int> lambdaHash)
            {
                if (lambdaComparer == null)
                    throw new ArgumentNullException("lambdaComparer");
                if (lambdaHash == null)
                    throw new ArgumentNullException("lambdaHash");

                _lambdaComparer = lambdaComparer;
                _lambdaHash = lambdaHash;
            }

            public bool Equals(T x, T y)
            {
                return _lambdaComparer(x, y);
            }

            public int GetHashCode(T obj)
            {
                return _lambdaHash(obj);
            }


        }
    }
}
