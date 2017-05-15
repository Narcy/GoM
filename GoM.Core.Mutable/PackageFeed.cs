﻿using System;
using System.Collections.Generic;

namespace GoM.Core.Mutable
{
    public class PackageFeed : IPackageFeed
    {
        public Uri Url { get; set; }

        public List<PackageInstance> Packages { get; } = new List<PackageInstance>();

        IReadOnlyCollection<IPackageInstance> IPackageFeed.Packages => Packages;
    }
}