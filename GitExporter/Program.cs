using System.Diagnostics;
using System.IO.Compression;

var commandLineVars = Environment.GetCommandLineArgs();
ValidateArguments(commandLineVars);

var sha1Hashes = getHashes();

Console.WriteLine("Creating zip folder.");
Directory.CreateDirectory($"{commandLineVars[2]}/zipFiles");

foreach (var sha1 in sha1Hashes)
{
    Console.WriteLine($"Exporting {sha1} commit.");

    execute($"git --git-dir \"{commandLineVars[1]}\\.git\" archive --format=zip \"{sha1}\" -o \"{commandLineVars[2]}/zipFiles/{sha1}.zip\"");

    Directory.CreateDirectory($"{commandLineVars[2]}/{sha1}");
    ZipFile.ExtractToDirectory($"{commandLineVars[2]}/zipFiles/{sha1}.zip", $"{commandLineVars[2]}/{sha1}");

    execute($"git --git-dir \"{commandLineVars[1]}\\.git\" show --no-patch \"{sha1}\" > \"{commandLineVars[2]}/{sha1}/commitInformations.txt\"");
} 

Console.WriteLine($"{sha1Hashes.Length} commits was exported.");

Console.WriteLine("Removing zip folder.");
Directory.Delete($"{commandLineVars[2]}/zipFiles", true);

string execute(string command)
{
    Process cmd = new Process();
    cmd.StartInfo.FileName = "cmd.exe";
    cmd.StartInfo.RedirectStandardInput = true;
    cmd.StartInfo.RedirectStandardOutput = true;
    cmd.StartInfo.CreateNoWindow = true;
    cmd.StartInfo.UseShellExecute = false;
    cmd.Start();

    cmd.StandardInput.WriteLine(command);
    cmd.StandardInput.Flush();
    cmd.StandardInput.Close();
    cmd.WaitForExit();
    var output = cmd.StandardOutput.ReadToEnd();

    return output;
}

string[] getHashes()
{
    Process git = new Process();
    git.StartInfo.FileName = "git.exe";
    git.StartInfo.RedirectStandardInput = true;
    git.StartInfo.RedirectStandardOutput = true;
    git.StartInfo.Arguments = $"--git-dir \"{commandLineVars[1] ?? ""}\\.git\" rev-list --all";
    git.StartInfo.CreateNoWindow = true;
    git.StartInfo.UseShellExecute = false;
    git.Start();

    git.WaitForExit();
    string stringHashes = git.StandardOutput.ReadToEnd().ReplaceLineEndings();

    return stringHashes.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
}

void ValidateArguments(string[] commandLineVars)
{
    //throw new NotImplementedException();
}