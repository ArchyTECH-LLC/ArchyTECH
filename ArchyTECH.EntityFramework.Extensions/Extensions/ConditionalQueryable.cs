using System.Linq.Expressions;

namespace ArchyTECH.EntityFramework.Extensions.Extensions
{

    public class ConditionalQueryable<T>
    {
        private readonly IQueryable<T> _query;
        private readonly bool _condition;

        public ConditionalQueryable(IQueryable<T> query, bool condition)
        {
            _query = query;
            _condition = condition;
        }

        public IQueryable<T> ThenWhere(Expression<Func<T, bool>> filterExpression)
        {
            if (_condition)
            {
                return _query.Where(filterExpression);
            }
            return _query;
        }
    }
}