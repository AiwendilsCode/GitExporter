
namespace GitExporter.Abstractions.Cmd
{
    internal interface ICmdManager
    {
        void Start();
        ValueTask<string> ExecuteCommand(string command);
    }
}
