using System.Collections.Generic;
using CdnBundle;
using SitecoreBundler.Models.Templates;

namespace SitecoreBundler.Cache
{
    public class BundleGroupCache
    {
        private readonly string _bundles;
        private readonly string _bundledFileName;
        private readonly bool _bundle;
        private readonly bool _minify;
        private readonly bool _agressiveCache;

        public BundleGroupCache(__BaseBundleGroup bundleGroup)
        {
            _bundle = bundleGroup.Bundle;
            _minify = bundleGroup.Minify;
            _agressiveCache = bundleGroup.AgressiveCache;
            _bundledFileName = bundleGroup.BundledFilename;
            _bundles = bundleGroup.Bundles;
            Bundles = new List<Bundle>();
        }

        public bool HasChanged(__BaseBundleGroup bundleGroup)
        {
            return bundleGroup.Bundle != _bundle
                   || bundleGroup.Minify != _minify
                   || bundleGroup.AgressiveCache != _agressiveCache
                   || bundleGroup.Bundles != _bundles
                   || bundleGroup.BundledFilename != _bundledFileName;
        }

        public string AgressiveCacheContent { get; set; }
        public bool BundlerRegistered { get; set; }
        public List<Bundle> Bundles { get; set; }
    }
}