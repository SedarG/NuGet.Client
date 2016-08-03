// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Concurrent;
using System.Collections.Generic;
using NuGet.Common;
using NuGet.DependencyResolver;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Repositories;

namespace NuGet.Commands
{
    /// <summary>
    /// Caches providers for the RestoreCommand. This helper ensures that no resources are duplicated.
    /// </summary>
    public class RestoreCommandProvidersCache
    {
        private readonly ConcurrentDictionary<SourceRepository, IRemoteDependencyProvider> _remoteProviders
            = new ConcurrentDictionary<SourceRepository, IRemoteDependencyProvider>();

        private readonly ConcurrentDictionary<VersionPackageFolder, IRemoteDependencyProvider> _localProvider
            = new ConcurrentDictionary<VersionPackageFolder, IRemoteDependencyProvider>(VersionPackageFolderComparer.Default);

        private readonly ConcurrentDictionary<VersionPackageFolder, NuGetv3LocalRepository> _globalCache
            = new ConcurrentDictionary<VersionPackageFolder, NuGetv3LocalRepository>(VersionPackageFolderComparer.Default);

        public RestoreCommandProviders GetOrCreate(
            VersionPackageFolder globalFolder,
            IReadOnlyList<VersionPackageFolder> fallbackFolders,
            IReadOnlyList<SourceRepository> sources,
            SourceCacheContext cacheContext,
            ILogger log)
        {
            var globalCache = _globalCache.GetOrAdd(globalFolder, folder => new NuGetv3LocalRepository(folder));

            var local = _localProvider.GetOrAdd(globalFolder, folder =>
            {
                // Create a v3 file system source
                var pathSource = Repository.Factory.GetCoreV3(folder);

                // Do not throw or warn for global cache 
                return new SourceRepositoryDependencyProvider(pathSource, log, cacheContext, ignoreFailedSources: true, ignoreWarning: true);
            });

            var localProviders = new List<IRemoteDependencyProvider>() { local };
            var fallbackRepositories = new List<NuGetv3LocalRepository>();

            foreach (var fallbackFolder in fallbackFolders)
            {
                var cache = _globalCache.GetOrAdd(fallbackFolder, (path) => new NuGetv3LocalRepository(path));
                fallbackRepositories.Add(cache);

                var localProvider = _localProvider.GetOrAdd(fallbackFolder, folder =>
                {
                    // Create a v3 file system source
                    var pathSource = Repository.Factory.GetCoreV3(folder);

                    // Throw for fallback package folders
                    return new SourceRepositoryDependencyProvider(pathSource, log, cacheContext, ignoreFailedSources: false, ignoreWarning: false);
                });

                localProviders.Add(localProvider);
            }

            var remoteProviders = new List<IRemoteDependencyProvider>(sources.Count);

            foreach (var source in sources)
            {
                var remoteProvider = _remoteProviders.GetOrAdd(source, (sourceRepo) => new SourceRepositoryDependencyProvider(sourceRepo, log, cacheContext));
                remoteProviders.Add(remoteProvider);
            }

            return new RestoreCommandProviders(globalCache, fallbackRepositories, localProviders, remoteProviders, cacheContext);
        }
    }
}
