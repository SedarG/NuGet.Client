using System;
using System.Globalization;
using NuGet.Common;
using NuGet.Packaging.Core;

namespace NuGet.Packaging
{
    public class VersionFolderPathContext
    {
        public PackageIdentity Package { get; }
        public VersionPackageFolder Folder { get; }
        public ILogger Logger { get; }
        public PackageSaveMode PackageSaveMode { get; }
        public XmlDocFileSaveMode XmlDocFileSaveMode { get; set; }

        public VersionFolderPathContext(
            PackageIdentity package,
            VersionPackageFolder folder,
            ILogger logger,
            PackageSaveMode packageSaveMode,
            XmlDocFileSaveMode xmlDocFileSaveMode)
        {
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            if (folder == null)
            {
                throw new ArgumentNullException(nameof(folder));
            }

            if (string.IsNullOrEmpty(folder.Path))
            {
                throw new ArgumentException(string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.StringCannotBeNullOrEmpty,
                    nameof(folder.Path)));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            Package = package;
            Folder = folder;
            Logger = logger;
            PackageSaveMode = packageSaveMode;
            XmlDocFileSaveMode = xmlDocFileSaveMode;
        }
    }
}
