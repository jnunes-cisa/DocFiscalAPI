using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.Extensions
{

    public static class Extensions
    {

        public static List<T> Except<T, TKey>(this List<T> items, List<T> other, Func<T, TKey> getKeyFunc)
        {

            return (items
                .GroupJoin(other, getKeyFunc, getKeyFunc, (item, tempItems) => new { item, tempItems })
                .SelectMany(t => t.tempItems.DefaultIfEmpty(), (t, temp) => new { t, temp })
                .Where(t => ReferenceEquals(null, t.temp) || t.temp.Equals(default(T)))
                .Select(t => t.t.item)).ToList();

        }

    }

}