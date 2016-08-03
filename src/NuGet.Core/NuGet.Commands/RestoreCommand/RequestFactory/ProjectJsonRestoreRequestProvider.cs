// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.ProjectModel;

namespace NuGet.Commands
{
    public class ProjectJsonRestoreRequestProvider : IRestoreRequestProvider
    {
        private readonly RestoreCommandProvidersCache _providerCache;

        public ProjectJsonRestoreRequestProvider(RestoreCommandProvidersCache providerCache)
        {
            _providerCache = providerCache;
        }

        public Task<IReadOnlyList<RestoreSummaryRequest>> CreateRequests(
            string inputPath,
            RestoreArgs restoreArgs)
        {
            var paths = new List<string>();

            if (Directory.Exists(inputPath))
            {
                paths.AddRange(GetProjectJsonFilesInDirectory(inputPath));
            }
            else
            {
                paths.Add(inputPath);
            }

            var requests = new List<RestoreSummaryRequest>(paths.Count);

            foreach (var path in paths)
            {
                requests.Add(Create(path, restoreArgs));
            }

            return Task.FromResult<IReadOnlyList<RestoreSummaryRequest>>(requests);
        }

        public Task<bool> Supports(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            // True if dir or project.json file
            var result = Directory.Exists(path)
                || (File.Exists(path) && ProjectJsonPathUtilities.IsProjectConfig(path));

            return Task.FromResult(result);
        }

        private RestoreSummaryRequest Create(
            string inputPath,
            RestoreArgs restoreArgs)
        {
            var file = new FileInfo(inputPath);

            // Get settings relative to the input file
            var settings = restoreArgs.GetSettings(file.DirectoryName);

            var sources = restoreArgs.GetEffectiveSources(settings);
            var FallbackPackageFolders = restoreArgs.GetEffectiveFallbackPackageFolders(settings);

            var globalPath = restoreArgs.GetEffectiveGlobalPackagesFolder(
                settings,
                lowercase: restoreArgs.LowercaseGlobalPackagesFolder);

            var sharedCache = _providerCache.GetOrCreate(
                globalPath,
                FallbackPackageFolders,
                sources,
                restoreArgs.CacheContext,
                restoreArgs.Log);

            var project = JsonPackageSpecReader.GetPackageSpec(file.Directory.Name, file.FullName);

            var request = new RestoreRequest(
                project,
                sharedCache,
                restoreArgs.Log,
                disposeProviders: false);

            restoreArgs.ApplyStandardProperties(request);

            var summaryRequest = new RestoreSummaryRequest(request, inputPath, settings, sources);

            return summaryRequest;
        }

        private static List<string> GetProjectJsonFilesInDirectory(string path)
        {
            try
            {
                return Directory.GetFiles(
                    path,
                    $"*{ProjectJsonPathUtilities.ProjectConfigFileName}",
                    SearchOption.AllDirectories)
                        .Where(file => ProjectJsonPathUtilities.IsProjectConfig(file))
                        .ToList();
            }
            catch (UnauthorizedAccessException e)
            {
                // Access to a subpath of the directory is denied.
                var resourceMessage = Strings.Error_UnableToLocateRestoreTarget_Because;
                var message = string.Format(CultureInfo.CurrentCulture, resourceMessage, path);

                throw new InvalidOperationException(message, e);
            }
        }
    }
}
