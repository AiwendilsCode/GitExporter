
namespace GitExporter.Abstractions.Git
{
    public interface IGitManager
    {
        void ExportBranchToDirectory(string directory, string branchName = "master", string repoDirectory = ".");
    }
}
