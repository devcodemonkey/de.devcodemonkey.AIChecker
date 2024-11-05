using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static de.devcodemonkey.AIChecker.UseCases.CreatePromptRatingUseCase;

namespace de.devcodemonkey.AIChecker.UseCases.Global
{
    public class Status
    {
        public static async Task<T> HandleStatus<T>(StatusHandler? statusHandler, string statusMessage, Func<Task<T>> action)
        {
            if (statusHandler != null)
            {
                T result = default!;
                statusHandler(statusMessage, () =>
                {
                    result = action().Result;
                });
                return result;
            }
            return await action();
        }

        public static async Task HandleStatus(StatusHandler? statusHandler, string statusMessage, Func<Task> action)
        {
            if (statusHandler != null)
            {
                statusHandler(statusMessage, () =>
                {
                    action().Wait();
                });
            }
            else
            {
                await action();
            }
        }
    }
}
