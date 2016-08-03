using NuGet.Common;
using Xunit;

namespace NuGet.Commands.Test
{
    public class VersionPackageFolderComparerTests
    {
        [Theory]
        [InlineData("pathA", true, "pathB", true, false)]  // Different path is always considered not equal.
        [InlineData("pathA", true, "pathB", false, false)] // Different path is always considered not equal.
        [InlineData("pathA", true, "pathA", true, true)]   // Same path and same lowercase flag is always considered equal.
        [InlineData("pathA", true, "pathA", false, false)] // Same path with different lowercase flag is considered not equal on case-sensitive systems.
        [InlineData("pathA", true, "PATHA", true, false)]  // Different case with same lowercase flag is considered not equal on case-sensitive systems.
        [InlineData("pathA", true, "PATHA", false, false)] // Different case with different lowercase flag is considered not equal on case-sensitive systems.
        public void VersionPackageFolderComparer_CaseSensitive_Equal(string pathA, bool lowercaseA, string pathB, bool lowercaseB, bool expected)
        {
            // Arrange
            var folderA = new VersionPackageFolder(pathA, lowercaseA);
            var folderB = new VersionPackageFolder(pathB, lowercaseB);
            var target = new VersionPackageFolderComparer(isFileSystemCaseSensitive: true);

            // Act
            var actual = target.Equals(folderA, folderB);

            // Assert
            Assert.Equal(actual, expected);
        }

        [Theory]
        [InlineData("pathA", true, "pathB", true, false)]  // Different path is always considered not equal.
        [InlineData("pathA", true, "pathB", false, false)] // Different path is always considered not equal.
        [InlineData("pathA", true, "pathA", true, true)]   // Same path and same lowercase flag is always considered equal.
        [InlineData("pathA", true, "pathA", false, true)]  // Same path with different lowercase flag is considered equal on case-sensitive systems.
        [InlineData("pathA", true, "PATHA", true, true)]   // Different case with same lowercase flag is considered equal on case-insensitive systems.
        [InlineData("pathA", true, "PATHA", false, true)]  // Different case with different lowercase flag is considered equal on case-insensitive systems.
        public void VersionPackageFolderComparer_CaseInsensitive_Equal(string pathA, bool lowercaseA, string pathB, bool lowercaseB, bool expected)
        {
            // Arrange
            var folderA = new VersionPackageFolder(pathA, lowercaseA);
            var folderB = new VersionPackageFolder(pathB, lowercaseB);
            var target = new VersionPackageFolderComparer(isFileSystemCaseSensitive: false);

            // Act
            var actual = target.Equals(folderA, folderB);

            // Assert
            Assert.Equal(actual, expected);
        }
    }
}
