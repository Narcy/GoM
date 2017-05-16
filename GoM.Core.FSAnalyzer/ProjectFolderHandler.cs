﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GoM.Core.Abstractions;
using Microsoft.Extensions.FileProviders;

namespace GoM.Core.FSAnalyzer
{
    public class ProjectFolderHandler : BaseProjectFolderHandler
    {
        public ProjectFolderHandler(IFileProvider provider) : base(provider)
        {
        }

        public override IProjectFolderHandler Sniff()
        {
            if (FileExtensions.Contains(".csproj"))
            {
                return new CsharpProjectHandler(FileProvider).Sniff();
            } else if (HasFile("package.json"))
            {
                return new JsProjectHandler(FileProvider).Sniff();
            }
            return null;
        }

        public override IProject Read()
        {
            return null;
        }
    }
}
