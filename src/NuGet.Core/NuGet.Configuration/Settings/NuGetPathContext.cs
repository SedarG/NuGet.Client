using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Common;

namespace NuGet.Configuration
{
    public class NuGetPathContext : INuGetPathContext
    {
        /// <summary>
        /// Fallback package folders. There many be zero or more of these.
        /// </summary>
        public IReadOnlyList<VersionPackageFolder> FallbackPackageFolders { get; internal set; }

        /// <summary>
        /// User global packages folder.
        /// </summary>
        public VersionPackageFolder UserPackageFolder { get; internal set; }

        /// <summary>
        /// User level http cache.
        /// </summary>
        public string HttpCacheFolder { get; internal set; }

        /// <summary>
        /// Load paths from already loaded settings.
        /// </summary>
        /// <param name="settings">NuGet.Config settings.</param>
        /// <param name="lowercase">
        /// Whether or not the user packages folder has lowercase ID and version folder names.
        /// </param>
        /// <returns>The NuGet path context.</returns>
        public static NuGetPathContext Create(ISettings settings, bool lowercase)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var userPackagesFolder = SettingsUtility.GetGlobalPackagesFolder(settings, lowercase);
            var fallbackPackageFolders = SettingsUtility.GetFallbackPackageFolders(settings);

            // Create paths using SettingsUtility
            return new NuGetPathContext()
            {
                FallbackPackageFolders = fallbackPackageFolders,
                UserPackageFolder = userPackagesFolder,
                HttpCacheFolder = SettingsUtility.GetHttpCacheFolder()
            };
        }

        /// <summary>
        /// Load settings based on the solution or project root directory. NuGet.Config files will 
        /// be discovered based on this path. The machine wide config will also be loaded.
        /// </summary>
        /// <param name="settingsRoot">Root directory of the solution or project.</param>
        /// <param name="lowercase">
        /// Whether or not the user packages folder has lowercase ID and version folder names.
        /// </param>
        /// <returns>The NuGet path context.</returns>
        public static NuGetPathContext Create(string settingsRoot, bool lowercase)
        {
            if (settingsRoot == null)
            {
                throw new ArgumentNullException(nameof(settingsRoot));
            }

            var settings = Settings.LoadDefaultSettings(settingsRoot);
            return Create(settings, lowercase);
        }
    }
}
