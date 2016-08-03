using System.IO;
using NuGet.Common;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Configuration.Test
{
    public class NuGetPathContextTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void NuGetPathContext_LoadSettings(bool lowercase)
        {
            // Arrange
            var config = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <config>
        <add key=""globalPackagesFolder"" value=""global"" />
    </config>
    <fallbackPackageFolders>
        <add key=""shared"" value=""test"" />
        <add key=""src"" value=""src"" />
    </fallbackPackageFolders>
</configuration>";

            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestFileSystemUtility.CreateRandomTestFolder())
            {
                var globalFolder = new VersionPackageFolder(Path.Combine(mockBaseDirectory, "global"), lowercase);

                // Fallback folders are always lowercase.
                var testFolder = new VersionPackageFolder(Path.Combine(mockBaseDirectory, "test"), lowercase: true);
                var srcFolder = new VersionPackageFolder(Path.Combine(mockBaseDirectory, "src"), lowercase: true);

                ConfigurationFileTestUtility.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);
                Settings settings = new Settings(mockBaseDirectory);

                var http = SettingsUtility.GetHttpCacheFolder();

                // Act
                var pathContext = NuGetPathContext.Create(settings, lowercase);

                // Assert
                Assert.Equal(2, pathContext.FallbackPackageFolders.Count);
                Assert.Equal(testFolder, pathContext.FallbackPackageFolders[0]);
                Assert.Equal(srcFolder, pathContext.FallbackPackageFolders[1]);
                Assert.Equal(globalFolder, pathContext.UserPackageFolder);
                Assert.Equal(http, pathContext.HttpCacheFolder);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void NuGetPathContext_LoadDefaultSettings(bool lowercase)
        {
            // Arrange
            using (var mockBaseDirectory = TestFileSystemUtility.CreateRandomTestFolder())
            {
                var globalFolder = SettingsUtility.GetGlobalPackagesFolder(NullSettings.Instance, lowercase: lowercase);
                var http = SettingsUtility.GetHttpCacheFolder();

                // Act
                var pathContext = NuGetPathContext.Create(NullSettings.Instance, lowercase);

                // Assert
                Assert.Equal(0, pathContext.FallbackPackageFolders.Count);
                Assert.Equal(globalFolder, pathContext.UserPackageFolder);
                Assert.Equal(http, pathContext.HttpCacheFolder);
            }
        }
    }
}
