using Cocona;
using GitExporter.Abstractions;
using GitExporter.Abstractions.Git;

var builder = CoconaApp.CreateBuilder();

builder.Services.AddGitAbstractions();

var app = builder.Build();

app.Run((IGitManager gitManager, [Argument(Description = "Directory to export into.")] string outputDirectory,
    [Argument(Description = "Optional, directory where repository is located.")] string repoDirectory = ".",
    [Argument(Description = "Optional, branch from where you want export commits.")] string branchName = "master") =>
{
    gitManager.ExportBranchToDirectory(outputDirectory, branchName, repoDirectory);
}); ;