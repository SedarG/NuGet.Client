// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using NuGet.Common;

namespace NuGet.Protocol.Core.Types
{
    public class OfflineFeedAddContext
    {
        public string PackagePath { get; }
        public VersionPackageFolder Folder { get; }
        public bool ThrowIfSourcePackageIsInvalid { get; }
        public bool ThrowIfPackageExistsAndInvalid { get; }
        public bool ThrowIfPackageExists { get; }
        public bool Expand { get; }
        public ILogger Logger { get; }

        public OfflineFeedAddContext(
            string packagePath,
            VersionPackageFolder folder,
            bool throwIfSourcePackageIsInvalid,
            bool throwIfPackageExistsAndInvalid,
            bool throwIfPackageExists,
            bool expand,
            ILogger logger)
        {
            if (string.IsNullOrEmpty(packagePath))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                    Strings.Argument_Cannot_Be_Null_Or_Empty,
                    nameof(packagePath)));
            }

            if (folder == null)
            {
                throw new ArgumentNullException(nameof(folder));
            }

            if (string.IsNullOrEmpty(folder.Path))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                    Strings.Argument_Cannot_Be_Null_Or_Empty, 
                    nameof(folder.Path)));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            PackagePath = packagePath;
            Folder = folder;
            Logger = logger;
            ThrowIfSourcePackageIsInvalid = throwIfSourcePackageIsInvalid;
            ThrowIfPackageExists = throwIfPackageExists;
            ThrowIfPackageExistsAndInvalid = throwIfPackageExistsAndInvalid;
            Expand = expand;
        }
    }
}
