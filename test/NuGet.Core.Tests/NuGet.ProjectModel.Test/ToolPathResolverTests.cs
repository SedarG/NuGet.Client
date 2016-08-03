using System.IO;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.Versioning;
using Xunit;

namespace NuGet.ProjectModel.Test
{
    public class ToolPathResolverTests
    {
        [Fact]
        public void ToolPathResolver_BuildsLockFilePath_Lowercase()
        {
            // Arrange
            var folder = new VersionPackageFolder("packages", lowercase: true);
            var target = new ToolPathResolver(folder);
            var expected = Path.Combine(
                "packages",
                ".tools",
                "packagea",
                "3.1.4-beta",
                "netstandard1.3",
                "project.lock.json");

            // Act
            var actual = target.GetLockFilePath(
                "packageA",
                NuGetVersion.Parse("3.1.4-BETA"),
                FrameworkConstants.CommonFrameworks.NetStandard13);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToolPathResolver_BuildsLockFilePath_OriginalCase()
        {
            // Arrange
            var folder = new VersionPackageFolder("packages", lowercase: false);
            var target = new ToolPathResolver(folder);
            var expected = Path.Combine(
                "packages",
                ".tools",
                "packageA",
                "3.1.4-BETA",
                "netstandard1.3",
                "project.lock.json");

            // Act
            var actual = target.GetLockFilePath(
                "packageA",
                NuGetVersion.Parse("3.1.4-BETA"),
                FrameworkConstants.CommonFrameworks.NetStandard13);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
