using System;
using System.Collections.Generic;
using System.Threading;
using NuGet.Common;
using NuGet.Packaging.Core;

namespace NuGet.Protocol
{
    /// <summary>
    /// Retrieve packages from a local folder or UNC share that uses the V3 folder structure.
    /// </summary>
    public class FindLocalPackagesResourceV3 : FindLocalPackagesResource
    {
        private readonly VersionPackageFolder _folder;

        public FindLocalPackagesResourceV3(VersionPackageFolder folder)
        {
            Root = folder.Path;
            _folder = folder;
        }

        public override IEnumerable<LocalPackageInfo> FindPackagesById(string id, ILogger logger, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            return LocalFolderUtility.GetPackagesV3(_folder, id, logger);
        }

        public override LocalPackageInfo GetPackage(Uri path, ILogger logger, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            return LocalFolderUtility.GetPackage(path, logger);
        }

        public override LocalPackageInfo GetPackage(PackageIdentity identity, ILogger logger, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            return LocalFolderUtility.GetPackageV3(_folder, identity, logger);
        }

        public override IEnumerable<LocalPackageInfo> GetPackages(ILogger logger, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            return LocalFolderUtility.GetPackagesV3(_folder, logger);
        }
    }
}
