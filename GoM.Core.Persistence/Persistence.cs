﻿using GoM.Core;
using System;
using GoM.Core.Persistence;
using System.IO;
using System.Xml.Linq;

namespace GoM.Core.Persistence
{
    public static class Helper
    {
        // XELEMENT (XNAME, OBJECT[])

        public static XElement ToXML ( this IPackageInstance _this )
        {
            XElement element = new XElement(typeof(IPackageInstance).Name);
            element.SetAttributeValue( nameof( _this.Version ), _this.Version );
            element.SetAttributeValue( nameof( _this.Name ), _this.Name );
            return element;
        }

        public static XElement ToXML(this IPackageFeed _this)
        {
            XElement element = new XElement(typeof(IPackageFeed).Name);
            element.SetAttributeValue( nameof( _this.Url ), _this.Url );
            foreach ( IPackageInstance package in _this.Packages )
            {
                element.Add( package.ToXML() );
            }
            return element;
        }

        public static XElement ToXML ( this IGoMContext _this )
        {
            XElement element = new XElement(typeof(IGoMContext).Name);
            element.SetAttributeValue( nameof( _this.RootPath ), _this.RootPath );
            foreach ( var t in _this.Repositories ) element.Add( t.ToXML() );
            foreach ( var t in _this.Feeds ) element.Add( t.ToXML() );
            return element;
        }

        public static XElement ToXML ( this IProject _this )
        {
            XElement element = new XElement(typeof(IGoMContext).Name);
            element.SetAttributeValue( nameof( _this.Path ), _this.Path );
            foreach ( var t in _this.Targets ) element.Add( t.ToXML() );
            return element;
        }

        public static XElement ToXML ( this ITargetDependency _this )
        {
            XElement element = new XElement(typeof(IGoMContext).Name);
            element.SetAttributeValue( nameof( _this.Name ), _this.Name );
            element.SetAttributeValue( nameof( _this.Version ), _this.Version );
            return element;
        }

        public static XElement ToXML ( this IVersionTag _this )
        {
            XElement element = new XElement(typeof(IGoMContext).Name);
            element.SetAttributeValue( nameof( _this.FullName ), _this.FullName );
            
            return element;
        }

        public static XElement ToXML ( this ITarget _this )
        {
            XElement element = new XElement(typeof(IGoMContext).Name);
            element.SetAttributeValue( nameof( _this.Name ), _this.Name );
            foreach ( var t in _this.Dependencies ) element.Add( t.ToXML() );

            return element;
        }

        public static XElement ToXML(this IBasicGitBranch _this)
        {
            XElement element = new XElement(typeof(IBasicGitBranch).Name);
            element.SetAttributeValue(nameof(_this.Name), _this.Name);
            element.Add( _this.Details.ToXML() );
            return element;
        }

        public static XElement ToXML(this IBranchVersionInfo _this)
        {
            XElement element = new XElement(typeof(IBasicGitBranch).Name);
            element.SetAttributeValue(nameof(_this.LastTagDepth), _this.LastTagDepth);
            element.Add( _this.LastTag.ToXML() );
            return element;
        }

        public static XElement ToXML(this IBasicGitRepository _this)
        {
            XElement element = new XElement(typeof(IBasicGitRepository).Name);
            element.SetAttributeValue(nameof(_this.Path), _this.Path);
            element.SetAttributeValue(nameof(_this.Url), _this.Url);

            element.Add( _this.Details.ToXML() );
            return element;
        }

        public static XElement ToXML(this IGitBranch _this)
        {
            XElement element = new XElement(typeof(IGitBranch).Name);
            element.Add(_this.Version.ToXML());
            foreach (var t in _this.Projects) element.Add(t.ToXML());

            element.SetAttributeValue( nameof( _this.Name ), _this.Name );
            element.Add( _this.Details.ToXML() );
            return element;
        }

        public static XElement ToXML(this IGitRepository _this)
        {
            XElement element = new XElement(typeof(IGitRepository).Name);

            element.SetAttributeValue( nameof( _this.Path ), _this.Path );
            element.SetAttributeValue( nameof( _this.Url ), _this.Url );
            foreach (var t in _this.Branches) element.Add(t.ToXML());

            element.Add( _this.Details.ToXML() );
            return element;
        }

    }

    class Persistence : IPersistence
    {
        string FolderName { get; }
        string FileName { get; }

        public Persistence(string folderName=".gom", string fileName="context")
        {
            FolderName = folderName;
            FileName = fileName;
        }


        public IGoMContext Load (string rootPath)
        {
            var data = File.ReadAllText( Path.Combine( rootPath, FolderName, FileName ) );
            XDocument doc = XDocument.Parse( data );
            IGoMContext context = new GoMContext();





            return context;
        }

        public void Save ( IGoMContext context)
        {
            if ( context == null ) throw new ArgumentNullException();

            using ( var stream = File.Create( Path.Combine( context.RootPath, FolderName, FileName ) ) )
            {
                XDocument doc = new XDocument();
                doc.Add( context.ToXML() );
                doc.Root.SetAttributeValue( "GOM_Document_Version", "1" );
                doc.Save(stream);
            }

        }



    }
}