
using System.Globalization;
using System.Text.RegularExpressions;

namespace GitExporter.Abstractions.Git
{
    public sealed class CommitInfo
    {
        public required string Hash { get; set; }
        public required DateTime CommitDateTime { get; set; }
        public required string Message { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }

        public static CommitInfo Parse(string commitInfoInString)
        {
            commitInfoInString = commitInfoInString.ReplaceLineEndings("\n\r").Trim();

            // Regex pattern to match the commit information
            Regex regex = new Regex(@"^commit (.+)\n\r(?:\bMerge: .+\n\r\b)?Author: (.+) <(.+)>\n\rDate: (.+)\n\r\n\r(.+)", RegexOptions.Multiline);
            Match match = regex.Match(commitInfoInString);

            CommitInfo commitInfo;

            if (match.Success)
            {
                commitInfo = new()
                {
                    Hash = match.Groups[1].Value.Trim(),
                    Username = match.Groups[2].Value.Trim(),
                    Email = match.Groups[3].Value.Trim(),
                    CommitDateTime = DateTime.ParseExact(match.Groups[4].Value.Trim(), "ddd MMM d HH:mm:ss yyyy K", CultureInfo.InvariantCulture),
                    Message = match.Groups[5].Value.Trim()
                };
            }
            else
            {
                throw new ArgumentException($"Input string was not in correct format. Input String:\n {commitInfoInString}");
            }

            return commitInfo;
        }
    }
}
