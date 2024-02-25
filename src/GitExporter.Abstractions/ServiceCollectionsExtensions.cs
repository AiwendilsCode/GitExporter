using GitExporter.Abstractions.Cmd;
using GitExporter.Abstractions.Git;
using Microsoft.Extensions.DependencyInjection;

namespace GitExporter.Abstractions
{
    public static class ServiceCollectionsExtensions
    {
        public static IServiceCollection AddGitAbstractions(this IServiceCollection services)
        {
            services.AddTransient<ICmdManager, CmdManager>();
            services.AddTransient<IGitManager, GitManager>();

            return services;
        }
    }
}
