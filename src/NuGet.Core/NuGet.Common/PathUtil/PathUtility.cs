using System;
using System.Collections.Generic;
using System.IO;

namespace NuGet.Common
{
    public static class PathUtility
    {
        /// <summary>
        /// Returns OrdinalIgnoreCase windows and mac. Ordinal for linux.
        /// </summary>
        /// <returns></returns>
        public static StringComparer GetStringComparerBasedOnOS()
        {
            if (RuntimeEnvironmentHelper.IsWindows)
            {
                return StringComparer.OrdinalIgnoreCase;
            }

            return StringComparer.Ordinal;
        }

        /// <summary>
        /// Returns distinct ordered <see cref="VersionPackageFolder"/> instances based on the file system case
        /// sensitivity.
        /// </summary>
        public static IEnumerable<VersionPackageFolder> GetUniqueFoldersBasedOnOS(IEnumerable<VersionPackageFolder> folders)
        {
            if (folders == null)
            {
                throw new ArgumentNullException(nameof(folders));
            }

            var unique = new HashSet<string>(GetStringComparerBasedOnOS());

            foreach (var folder in folders)
            {
                if (unique.Add(folder.Path))
                {
                    yield return folder;
                }
            }

            yield break;
        }

        public static string GetPathWithForwardSlashes(string path)
        {
            return path.Replace('\\', '/');
        }

        public static string EnsureTrailingSlash(string path)
        {
            return EnsureTrailingCharacter(path, Path.DirectorySeparatorChar);
        }

        private static string EnsureTrailingCharacter(string path, char trailingCharacter)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            // if the path is empty, we want to return the original string instead of a single trailing character.
            if (path.Length == 0
                || path[path.Length - 1] == trailingCharacter)
            {
                return path;
            }

            return path + trailingCharacter;
        }
    }
}