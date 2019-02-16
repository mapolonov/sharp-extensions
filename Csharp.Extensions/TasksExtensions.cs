using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Csharp.Extensions
{
    public static class TasksExtensions
    {
        public static async Task ThrottleAsync(this IEnumerable<Task> tasks, int maxConcurrentTasks)
        {
            if (tasks == null) throw new ArgumentNullException(nameof(tasks));
            if (maxConcurrentTasks <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxConcurrentTasks));

            var concurrentTasks = new List<Task>();

            using (var semaphore = new SemaphoreSlim(maxConcurrentTasks))
            {
                await semaphore.WaitAsync();
                foreach (var task in tasks)
                {
                    await semaphore.WaitAsync();
                    concurrentTasks.Add(Execute(semaphore, task));
                }

                await Task.WhenAll(concurrentTasks);
            }
        }

        private static async Task Execute(SemaphoreSlim semaphore, Task task)
        {
            try
            {
                await task;
            }
            finally
            {
                semaphore.Release();
            }
        }

        public static async Task<IEnumerable<T>> ThrottleAsync<T>(this IEnumerable<Task<T>> tasks, int maxConcurrentTasks)
        {
            if (tasks == null) throw new ArgumentNullException(nameof(tasks));
            if (maxConcurrentTasks <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxConcurrentTasks));

            var concurrentTasks = new List<Task<T>>();

            using (var semaphore = new SemaphoreSlim(maxConcurrentTasks))
            {
                await semaphore.WaitAsync();
                foreach (var task in tasks)
                {
                    await semaphore.WaitAsync();
                    concurrentTasks.Add(Execute(semaphore, task));
                }

                return await Task.WhenAll(concurrentTasks);
            }
        }

        private static async Task<T> Execute<T>(SemaphoreSlim semaphore, Task<T> task)
        {
            try
            {
                return await task;
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}