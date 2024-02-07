using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace OceansInFlame.Library.Helpers.Extensions
{
    public static class SemaphoreSlimExtensions
    {
        readonly record struct SemaphoreHandle(SemaphoreSlim semaphore) : IDisposable
        {
            public void Dispose()
            {
                semaphore.Release();
            }
        }
        public static async Task<IDisposable> AcquireAsync(this SemaphoreSlim semaphore)
        {
            await semaphore.WaitAsync();
            return new SemaphoreHandle(semaphore);
        }
    }
}
