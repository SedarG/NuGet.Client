using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Commands.Test
{
    public class MSBuildRestoreResultTests
    {
        [Fact]
        public void MSBuildRestoreResult_ReplaceWithUserProfileMacro()
        {
            // Arrange
            using (var randomProjectDirectory = TestFileSystemUtility.CreateRandomTestFolder())
            {
                var projectName = "testproject";
                var globalPackagesFolder = SettingsUtility.GetGlobalPackagesFolder(
                    NullSettings.Instance,
                    lowercase: true);

                var msBuildRestoreResult = new MSBuildRestoreResult(
                    projectName,
                    randomProjectDirectory,
                    globalPackagesFolder.Path,
                    Enumerable.Empty<string>(),
                    new[] { "blah" });

                var targetsName = $"{projectName}.nuget.targets";
                var targetsPath = Path.Combine(randomProjectDirectory, targetsName);

                // Assert
                Assert.False(File.Exists(targetsPath));

                // Act
                msBuildRestoreResult.Commit(NullLogger.Instance);

                Assert.True(File.Exists(targetsPath));
                var xml = XDocument.Load(targetsPath);
                var ns = XNamespace.Get("http://schemas.microsoft.com/developer/msbuild/2003");
                var elements = xml.Root.Descendants(ns + "NuGetPackageRoot");
                Assert.Single(elements);

                string expected;
                if (RuntimeEnvironmentHelper.IsWindows)
                {
                    expected = Path.Combine(@"$(UserProfile)", ".nuget", "packages") + Path.DirectorySeparatorChar;
                }
                else
                {
                    expected = Path.Combine(Environment.GetEnvironmentVariable("HOME"), ".nuget", "packages") + Path.DirectorySeparatorChar;
                }

                var element = elements.Single();
                Assert.Equal(expected, element.Value);
            }
        }
    }
}
