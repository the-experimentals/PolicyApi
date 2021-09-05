using System;
using Microsoft.Extensions.DependencyInjection;

namespace PolicyApi.StartupTasks
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStartupTask<T>(this IServiceCollection services)
        where T : class, IStartupTask
        => services.AddTransient<IStartupTask, T>();
    }
}
