using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceansInFlame.Library.Helpers.Extensions
{
    public static class TaskExtensions
    {
        public static T Await<T>(this Task<T> task)
        {
            //Task.WaitAll(task);
            var awaiter = task.GetAwaiter();
            var result = awaiter.GetResult();
            return result;
        }
        public static T Await<T>(this ValueTask<T> task)
        {
            return task.AsTask().Await();
        }
    }
}
