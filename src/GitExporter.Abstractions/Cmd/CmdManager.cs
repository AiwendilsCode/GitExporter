

using System.Diagnostics;

namespace GitExporter.Abstractions.Cmd
{
    internal sealed class CmdManager : ICmdManager
    {
        private readonly Process _commandLine;

        public bool IsStarted { get; private set; } = false;

        public CmdManager()
        {
            _commandLine = new Process();
        }

        public ValueTask<string> ExecuteCommand(string command)
        {
            _commandLine.StandardInput.WriteLine(command);
            _commandLine.StandardInput.Flush();
            _commandLine.StandardInput.Close();
            _commandLine.WaitForExit();
            var output = _commandLine.StandardOutput.ReadToEnd();

            return ValueTask.FromResult(output);
        }

        public void Start()
        {
            _commandLine.StartInfo.FileName = "cmd.exe";
            _commandLine.StartInfo.RedirectStandardInput = true;
            _commandLine.StartInfo.RedirectStandardOutput = true;
            _commandLine.StartInfo.CreateNoWindow = true;
            _commandLine.StartInfo.UseShellExecute = false;
            _commandLine.Start();

            IsStarted = true;
        }
    }
}
