using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceansInFlame.Library.Helpers.Extensions
{
    public static class ParallelQueryExtensions
    {
        public static ParallelQuery<T> ForEach<T>(ParallelQuery<T> items, Action<T> action)
        {
            return items.Select(item =>
            {
                action(item);
                return item;
            });
        }


        public static ParallelQuery<R> SelectAwait<T, R>(this ParallelQuery<T> series, Func<T, Task<R>> func)
        {
            return series.Select(item => func(item).GetAwaiter().GetResult());
        }

        public static ParallelQuery<T> Count<T>(this ParallelQuery<T> series, Counter<long> counter, Func<T, long> measurement)
        {
            return series.Select(item =>
            {
                var amount = measurement(item);
                counter.Add(amount);
                return item;
            });
        }
    }
}
