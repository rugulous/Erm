using System;
using System.Linq.Expressions;

namespace Erm
{
    public class GenericQuery<T> : Query
    {
        private Type _type { get; }

        public GenericQuery() : base(typeof(T).Name)
        {
            _type = typeof(T);
        }

        public GenericQuery Where(Expression<Func<T, bool>> predicate)
        {
            return Where<_type>(predicate);
        }
    }
}
