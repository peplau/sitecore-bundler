using SitecoreBundler.Models.Templates;

namespace SitecoreBundler.Cache
{
    public class BundlerCache
    {
        private readonly string _bundleLocalPath;
        private readonly bool? _bundleEnabled;
        private readonly bool? _minifyEnabled;
        private readonly bool? _agressiveCacheEnabled;

        public BundlerCache(Bundler bundler)
        {
            _bundleLocalPath = bundler.BundleLocalPath;
            _bundleEnabled = bundler.BundleEnabled;
            _minifyEnabled = bundler.MinifyEnabled;
            _agressiveCacheEnabled = bundler.AgressiveCacheEnabled;
        }

        public bool HasChanged(Bundler bundler)
        {
            return bundler.BundleLocalPath != _bundleLocalPath
                || bundler.BundleEnabled != _bundleEnabled
                || bundler.MinifyEnabled != _minifyEnabled
                || bundler.AgressiveCacheEnabled != _agressiveCacheEnabled;
        }
    }
}