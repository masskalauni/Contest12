using System;
using System.Collections.Generic;
using System.Linq;

namespace Kolpi.Utilities
{
    public static class ComplexTypeComparer<T>
    {
        public static ComplexTypeComparer<T, TK> Create<TK>(params Func<T, TK>[] projections)
        {
            return new ComplexTypeComparer<T, TK>(projections);
        }

        public static ComplexTypeComparer<T, TK> Create<TK>(IEqualityComparer<TK> comparer, params Func<T, TK>[] projections)
        {
            return new ComplexTypeComparer<T, TK>(projections, comparer);
        }
    }
    public class ComplexTypeComparer<T, TK> : IEqualityComparer<T>
    {
        readonly Func<T, TK>[] _projections;
        private readonly IEqualityComparer<TK> _comparer;

        public ComplexTypeComparer(Func<T, TK>[] projections) : this(projections, null)
        {
        }
        public ComplexTypeComparer(Func<T, TK>[] projections, IEqualityComparer<TK> comparer)
        {
            _projections = projections;
            _comparer = comparer ?? EqualityComparer<TK>.Default;
        }

        public bool Equals(T x, T y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            if (x == null || y == null)
            {
                return false;
            }

            return _projections.All(projection => _comparer.Equals(projection(x), projection(y)));
        }

        public int GetHashCode(T obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            return _comparer.GetHashCode(_projections[0](obj));
        }
    }
}