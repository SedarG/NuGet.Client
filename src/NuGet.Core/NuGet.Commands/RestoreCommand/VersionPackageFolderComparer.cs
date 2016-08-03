// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Common;
using NuGet.Shared;

namespace NuGet.Commands
{
    /// <summary>
    /// Compares two <see cref="VersionPackageFolder"/>. The logic differs based on whether the
    /// file system is case-sensitive or not.
    /// </summary>
    public class VersionPackageFolderComparer : IEqualityComparer<VersionPackageFolder>
    {
        public static VersionPackageFolderComparer _default = new VersionPackageFolderComparer(
            isFileSystemCaseSensitive: !RuntimeEnvironmentHelper.IsWindows);

        public static VersionPackageFolderComparer Default => _default;

        private readonly bool _isFileSystemCaseSensitive;
        private readonly StringComparer _pathComparer;

        public VersionPackageFolderComparer(bool isFileSystemCaseSensitive)
        {
            _isFileSystemCaseSensitive = isFileSystemCaseSensitive;
            _pathComparer = isFileSystemCaseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase;
        }

        public bool Equals(VersionPackageFolder x, VersionPackageFolder y)
        {
            if (!_isFileSystemCaseSensitive)
            {
                return _pathComparer.Equals(x.Path, y.Path);
            }

            return _pathComparer.Equals(x.Path, y.Path) &&
                   x.Lowercase == y.Lowercase;
        }

        public int GetHashCode(VersionPackageFolder obj)
        {
            var combiner = new HashCodeCombiner();
            
            if (!_isFileSystemCaseSensitive)
            {
                // If the file system is case-insensitive, then we only need to include the path in
                // the hashcode. The lowercase flag is irrelevant.
                combiner.AddObject(obj.Path, _pathComparer);
            }
            else
            {
                // If the file system is case-sensitive, then we need to both compare the path in a
                // case-sensitive manner as well as treating two folders with the same path but
                // different lowercase flag as different.
                combiner.AddObject(obj.Path, _pathComparer);
                combiner.AddObject(obj.Lowercase);
            }

            return combiner.CombinedHash;
        }
    }
}
