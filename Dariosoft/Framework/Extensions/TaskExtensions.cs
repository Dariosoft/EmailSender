using System;
using System.Threading.Tasks;

namespace Dariosoft.Framework
{
    public static class TaskExtensions
    {
        public static Task<T> Done<TInput, T>(this Task<TInput> task, Func<TInput, T> onSuccess, Func<Exception, T>? onFailed = null)
            => task.ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                    return onSuccess(task.Result);
                else
                    return onFailed is null ? default! : onFailed(task.Exception!);
            });

        public static Task<T> Done<T>(this Task task, Func<T> onSuccess, Func<Exception, T>? onFailed = null)
            => task.ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                    return onSuccess();
                else
                    return onFailed is null ? default! : onFailed(task.Exception!);
            });

        public static Task Done<TInput>(this Task<TInput> task, Action<TInput> onSuccess, Action<Exception>? onFailed = null)
            => task.ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                    onSuccess(task.Result);
                else if (onFailed is not null)
                    onFailed(task.Exception!);
            });

        public static Task Done(this Task task, Action onSuccess, Action<Exception>? onFailed = null)
        => task.ContinueWith(task =>
        {
            if (task.IsCompletedSuccessfully)
                onSuccess();
            else if (onFailed is not null)
                onFailed(task.Exception!);
        });



    }
}
