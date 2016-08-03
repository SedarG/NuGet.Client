using System;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    public class FindLocalPackagesResourceV3Provider : ResourceProvider
    {
        public FindLocalPackagesResourceV3Provider()
            : base(typeof(FindLocalPackagesResource), nameof(FindLocalPackagesResourceV3Provider), nameof(FindLocalPackagesResourceV2Provider))
        {
        }

        public override async Task<Tuple<bool, INuGetResource>> TryCreate(SourceRepository source, CancellationToken token)
        {
            FindLocalPackagesResource curResource = null;

            var feedType = await source.GetFeedType(token);
            if (feedType == FeedType.FileSystemV3 ||
                feedType == FeedType.FileSystemV3OriginalCase)
            {
                var folder = new VersionPackageFolder(
                    source.PackageSource.Source,
                    lowercase: feedType != FeedType.FileSystemV3OriginalCase);

                curResource = new FindLocalPackagesResourceV3(folder);
            }

            return new Tuple<bool, INuGetResource>(curResource != null, curResource);
        }
    }
}
