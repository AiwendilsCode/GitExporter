using GitExporter.Abstractions.Git;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using System.Globalization;
using System.Text.RegularExpressions;

namespace GitExporter.Abstractions.Tests.ParsingTests
{
    public class CommitInfoParsingTests
    {
        [Fact]
        public void ParseCommitInfo_shouldParse()
        {
            // Arrange
            string inputString = @"commit 3d6d8a206f62d8a0eb3691b8ccf46d2c421c4f18
Author: John Smith <john.smith@example.com>
Date:   Thu Feb 15 12:00:00 2024 -0000

    Updated README.md and added new feature";

            // Act
            var parsedCommitInfo = CommitInfo.Parse(inputString);

            // Assert
            parsedCommitInfo.CommitDateTime.ToUniversalTime().Should().Be(new DateTime(2024, 2, 15, 12, 0, 0, DateTimeKind.Utc));
            parsedCommitInfo.Hash.Should().Be("3d6d8a206f62d8a0eb3691b8ccf46d2c421c4f18");
            parsedCommitInfo.Message.Should().Be("Updated README.md and added new feature");
            parsedCommitInfo.Username.Should().Be("John Smith");
            parsedCommitInfo.Email.Should().Be("john.smith@example.com");
        }
    }
}
