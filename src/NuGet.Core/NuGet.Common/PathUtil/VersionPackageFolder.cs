// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Shared;

namespace NuGet.Common
{
    /// <summary>
    /// This represents a V3 package folder on disk.
    /// </summary>
    public class VersionPackageFolder : IEquatable<VersionPackageFolder>
    {
        public VersionPackageFolder(string path, bool lowercase)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            Path = path;
            Lowercase = lowercase;
        }

        /// <summary>
        /// The path to the root of the folder.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Whether or not ID and version folder names should be lowercased.
        /// </summary>
        public bool Lowercase { get; }

        public override int GetHashCode()
        {
            var combiner = new HashCodeCombiner();

            combiner.AddObject(Path);
            combiner.AddObject(Lowercase);

            return combiner.CombinedHash;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as VersionPackageFolder);
        }

        public bool Equals(VersionPackageFolder other)
        {
            if (other == null)
            {
                return false;
            }

            // Note that this equality can be more clever on case-insensitive file systems. However
            // that is handled at higher levels.
            return Path == other.Path && Lowercase == other.Lowercase;
        }
    }
}
