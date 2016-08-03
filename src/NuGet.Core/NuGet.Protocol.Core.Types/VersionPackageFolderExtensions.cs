// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Common;

namespace NuGet.Protocol.Core.Types
{
    /// <summary>
    /// Extension methods for <see cref="VersionPackageFolder"/>.
    /// </summary>
    public static class VersionPackageFolderExtensions
    {
        /// <summary>
        /// Determines the feed type of a <see cref="VersionPackageFolder"/>. The output will either
        /// be <see cref="FeedType.FileSystemV3"/> or <see cref="FeedType.FileSystemV3OriginalCase"/>.
        /// </summary>
        /// <param name="folder">The version package folder.</param>
        /// <returns>The feed type.</returns>
        public static FeedType GetFeedType(this VersionPackageFolder folder)
        {
            return folder.Lowercase ? FeedType.FileSystemV3 : FeedType.FileSystemV3OriginalCase;
        }
    }
}
