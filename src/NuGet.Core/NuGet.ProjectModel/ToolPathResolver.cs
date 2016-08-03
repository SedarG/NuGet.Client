// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.Versioning;

namespace NuGet.ProjectModel
{
    public class ToolPathResolver
    {
        private readonly VersionPackageFolder _folder;

        public ToolPathResolver(VersionPackageFolder folder)
        {
            _folder = folder;
        }

        public string GetLockFilePath(string packageId, NuGetVersion version, NuGetFramework framework)
        {
            var versionString = version.ToNormalizedString();
            var frameworkString = framework.GetShortFolderName();

            if (_folder.Lowercase)
            {
                packageId = packageId.ToLowerInvariant();
                versionString = versionString.ToLowerInvariant();
                frameworkString = frameworkString.ToLowerInvariant();
            }

            return Path.Combine(
                _folder.Path,
                ".tools",
                packageId,
                versionString,
                frameworkString,
                LockFileFormat.LockFileName);
        }
    }
}
