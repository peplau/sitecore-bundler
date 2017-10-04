using SitecoreBundler.Models.Templates;

namespace SitecoreBundler.Cache
{
    public class BundlerSettingsCache
    {
        private readonly Bundler _bundler;

        public BundlerSettingsCache(Bundler bundler)
        {
            _bundler = bundler;
        }

        public bool HasChanged(Bundler bundler)
        {
            return bundler.BundleLocalPath != _bundler.BundleLocalPath 
                || bundler.Bundle != _bundler.Bundle 
                || bundler.Minify != _bundler.Minify 
                || bundler.AgressiveCache != _bundler.AgressiveCache;
        }
    }
}