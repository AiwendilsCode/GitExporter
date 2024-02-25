
using GitExporter.Abstractions.Cmd;
using System.Diagnostics;
using System.IO.Compression;

namespace GitExporter.Abstractions.Git
{
    internal sealed class GitManager : IGitManager
    {
        private readonly ICmdManager _cmdManager;

        public GitManager(ICmdManager cmdManager)
        {
            _cmdManager = cmdManager;
        }

        public void ExportBranchToDirectory(string directory, string branchName = "master", string repoDirectory = ".")
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                Console.WriteLine($"creating directory {directory}");
            }

            foreach(var commit in GetCommitsInfos(GetAllCommitsHashesInBranch(branchName, repoDirectory), repoDirectory))
            {
                ExportCommitToDirectory(commit, directory, repoDirectory);
            }
        }

        private void ExportCommitToDirectory(CommitInfo commitInfo, string directory, string repoDirectory)
        {
            Console.WriteLine($"Exporting commit {commitInfo.Hash}");
            _cmdManager.Start();

            _cmdManager.ExecuteCommand($"git --git-dir \"{repoDirectory}\\.git\" archive --format=zip \"{commitInfo.Hash}\" -o \"{commitInfo.Hash}temp.zip\"");

            ZipFile.ExtractToDirectory($"{commitInfo.Hash}temp.zip", $"{directory}/{commitInfo.CommitDateTime:dd-MM-yyyy--HH-mm-ss}");

            File.Delete($"{commitInfo.Hash}temp.zip");
        }

        private IEnumerable<string> GetAllCommitsHashesInBranch(string branchName, string repoDirectory)
        {
            Process git = new Process();
            git.StartInfo.FileName = "git.exe";
            git.StartInfo.RedirectStandardInput = true;
            git.StartInfo.RedirectStandardOutput = true;
            git.StartInfo.Arguments = $"--git-dir=\"{repoDirectory}\\.git\" --work-tree=\"{repoDirectory}\" log {branchName} --oneline --abbrev=40";
            git.StartInfo.CreateNoWindow = true;
            git.StartInfo.UseShellExecute = false;
            git.Start();

            string commits = git.StandardOutput.ReadToEnd().ReplaceLineEndings();
            git.WaitForExit();

            return commits.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Split(' ')[0]);
        }

        private IEnumerable<CommitInfo> GetCommitsInfos(IEnumerable<string> hashes, string repoDirectory)
        {
            foreach (string hash in hashes)
            {
                Process git = new Process();
                git.StartInfo.FileName = "git.exe";
                git.StartInfo.RedirectStandardInput = true;
                git.StartInfo.RedirectStandardOutput = true;
                git.StartInfo.Arguments = $"--git-dir=\"{repoDirectory}\\.git\" --work-tree=\"{repoDirectory}\" show --no-patch {hash}";
                git.StartInfo.CreateNoWindow = true;
                git.StartInfo.UseShellExecute = false;
                git.Start();

                string commitInfo = git.StandardOutput.ReadToEnd();
                git.WaitForExit();

                yield return CommitInfo.Parse(commitInfo);
            }
        }
    }
}
