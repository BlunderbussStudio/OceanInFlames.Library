using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceansInFlame.Library.Helpers.Extensions
{
    public static class AsyncEnumerableExtensions
    {
        public class AsyncEnumerable<T> : IAsyncEnumerable<T>
        {
            readonly SemaphoreSlim canSet = new SemaphoreSlim(1);
            readonly SemaphoreSlim canGet = new SemaphoreSlim(0);
            bool closed = false;
            T current = default;

            public async Task Close()
            {
                await canSet.WaitAsync();
                closed = true;
                canGet.Release();
            }

            public async Task Enqueue(T value)
            {
                await canSet.WaitAsync();
                current = value;
                canGet.Release();
            }

            public async IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            {
                while (true)
                {
                    await canGet.WaitAsync();
                    if (closed) break;
                    yield return current;
                    canSet.Release();
                }
            }
        }

        public static IAsyncEnumerable<T>[] OrderedSplit<T>(this IAsyncEnumerable<T> series, int count)
        {
            var channels = Enumerable
                .Range(0, count)
                .Select(_ => new AsyncEnumerable<T>())
                .ToArray();

            Task.Run(async () =>
            {
                var channelIndex = 0;
                await foreach (var item in series)
                {
                    var channel = channels[channelIndex];
                    await channel.Enqueue(item);
                    channelIndex = (channelIndex + 1) % count;
                }
                for (var i = 0; i < count; i++)
                {
                    var channel = channels[channelIndex];
                    await channel.Close();
                    channelIndex = (channelIndex + 1) % count;
                }
            });

            return channels;
        }

        public static async IAsyncEnumerable<T> OrderedMerge<T>(IAsyncEnumerable<T>[] channels)
        {
            var enumerators = channels
                .Select(c => c.GetAsyncEnumerator())
                .ToQueue();

            while (enumerators.TryDequeue(out IAsyncEnumerator<T> enumerator))
            {
                if (!await enumerator.MoveNextAsync()) continue;
                yield return enumerator.Current;
                enumerators.Enqueue(enumerator);
            }
        }

        public static IAsyncEnumerable<T> OrderedBuffer<T>(this IAsyncEnumerable<T> input, int capacity)
        {
            var inSemaphore = new SemaphoreSlim(capacity);
            var outSemaphore = new SemaphoreSlim(0);
            var output = new AsyncEnumerable<T>();
            var queue = new Queue<T>();

            Task.Run(async () =>
            {
                await foreach (var item in input)
                {
                    await inSemaphore.WaitAsync();
                    queue.Enqueue(item);
                    outSemaphore.Release();
                }
                outSemaphore.Release();
            });

            Task.Run(async () =>
            {
                while (true)
                {
                    await outSemaphore.WaitAsync();
                    if (!queue.TryDequeue(out T item)) break;
                    await output.Enqueue(item);
                    inSemaphore.Release();
                }
                await output.Close();
            });

            return output;
        }

        public static IAsyncEnumerable<R> InParallel<T, R>(this IAsyncEnumerable<T> source, int concurrency, Func<IAsyncEnumerable<T>, IAsyncEnumerable<R>> func)
        {
            var channels = source.OrderedSplit(concurrency);
            var processors = channels.Select(c => func(c).OrderedBuffer(1)).ToArray();
            var output = OrderedMerge(processors);
            return output;
        }

        public static async IAsyncEnumerable<T[]> ChunkWhile<T, A>(this IAsyncEnumerable<T> source, Func<A> init, Func<A, T, A> aggregator, Func<A, bool> test)
        {
            List<T> buffer = new List<T>();
            A aggregate = init();

            await foreach (var item in source)
            {
                buffer.Add(item);
                aggregate = aggregator(aggregate, item);
                if (!test(aggregate))
                {
                    var batch = buffer.Take(buffer.Count - 1).ToArray();
                    yield return batch;
                    buffer.Clear();
                    buffer.Add(item);
                    aggregate = aggregator(init(), item);
                }
            }

            if (buffer.Count > 0)
            {
                yield return buffer.ToArray();
            }
        }

        public static async IAsyncEnumerable<A> Reduce<T, A>(this IAsyncEnumerable<T> series, A init, Func<A, T, A> reduce)
        {
            var aggregate = init;
            await foreach (var item in series)
            {
                aggregate = reduce(aggregate, item);
            }
            yield return aggregate;
        }

        public static async IAsyncEnumerable<(A, B)> AnnotateAsync<A, B>(this IAsyncEnumerable<A> series, Func<A, Task<B>> func)
        {
            await foreach (var a in series)
            {
                var b = await func(a);
                yield return (a, b);
            }
        }

        public static async IAsyncEnumerable<T> Count<T>(this IAsyncEnumerable<T> series, Counter<long> counter, Func<T, long> measurement)
        {
            await foreach (var item in series)
            {
                var amount = measurement(item);
                counter.Add(amount);
                yield return item;
            }
        }
    }
}
