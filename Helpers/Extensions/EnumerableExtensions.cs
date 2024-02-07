using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace OceansInFlame.Library.Helpers.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> series, Action<T> action)
        {
            foreach (var item in series)
            {
                action(item);
                yield return item;
            }
        }

        public static Queue<T> ToQueue<T>(this IEnumerable<T> series)
        {
            var queue = new Queue<T>();
            foreach (var item in series)
            {
                queue.Enqueue(item);
            }
            return queue;
        }

        public static IEnumerable<T> Count<T>(this IEnumerable<T> series, Counter<long> counter, Func<T, long> measurement)
        {
            foreach (var item in series)
            {
                var amount = measurement(item);
                counter.Add(amount);
                yield return item;
            }
        }

        public static IEnumerable<R> SelectAwait<T, R>(this IEnumerable<T> series, Func<T, Task<R>> func)
        {
            foreach (var item in series)
            {
                yield return func(item).GetAwaiter().GetResult();
            }
        }
    }
}
