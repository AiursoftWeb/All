﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Aiursoft.XelNaga.Services;

public static class AsyncHelper
{
    private static readonly TaskFactory TaskFactory = new(CancellationToken.None,
        TaskCreationOptions.None,
        TaskContinuationOptions.None,
        TaskScheduler.Default);

    public static async Task<T> Try<T>(Func<Task<T>> taskFactory, int times, Action<Exception> onError = null)
    {
        for (var i = 1; i <= times; i++)
        {
            try
            {
                var response = await taskFactory();
                return response;
            }
            catch (Exception e)
            {
                onError?.Invoke(e);
                if (i >= times)
                {
                    throw;
                }

                await Task.Delay(ExponentialBackOffTimeSlot(i) * 1000);
            }
        }

        throw new InvalidOperationException("Code shall not reach here.");
    }

    /// <summary>
    ///     Run with retry. This is only a light-weight method for pool retry. For complicated requirements with dependency,
    ///     please consider RetryEngine.
    /// </summary>
    /// <param name="taskFactory"></param>
    /// <param name="times"></param>
    /// <param name="onError"></param>
    public static void TryAsync(Func<Task> taskFactory, int times, Func<Exception, Task> onError = null)
    {
        for (var i = 1; i <= times; i++)
        {
            try
            {
                RunSync(taskFactory);
                break;
            }
            catch (Exception e)
            {
                if (onError != null)
                {
                    RunSync(() => onError(e));
                }

                if (i >= times)
                {
                    throw;
                }

                Thread.Sleep(ExponentialBackOffTimeSlot(i) * 1000);
            }
        }
    }

    /// <summary>
    ///     <see href="https://en.wikipedia.org/wiki/Exponential_backoff">Exponetial backoff </see> time slot.
    /// </summary>
    /// <param name="time">the time of trial</param>
    /// <returns>Time slot to wait.</returns>
    private static int ExponentialBackOffTimeSlot(int time)
    {
        var max = (int)Math.Pow(2, time);
        var rnd = new Random();
        return rnd.Next(0, max);
    }

    public static TResult RunSync<TResult>(Func<Task<TResult>> func)
    {
        return TaskFactory
            .StartNew(func)
            .Unwrap()
            .GetAwaiter()
            .GetResult();
    }

    public static void RunSync(Func<Task> func)
    {
        TaskFactory
            .StartNew(func)
            .Unwrap()
            .GetAwaiter()
            .GetResult();
    }

    public static Task ForEachParallel<T>(this IEnumerable<T> items, Func<T, Task> function)
    {
        return Task.WhenAll(items
            .Select(function));
    }
}