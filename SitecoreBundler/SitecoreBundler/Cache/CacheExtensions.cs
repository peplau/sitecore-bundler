using System.Collections.Generic;
using System.Linq;
using Sitecore.Data;
using SitecoreBundler.Models.Templates;

namespace SitecoreBundler.Cache
{
    public static class CacheExtensions
    {
        public static bool HasChanged(this Dictionary<ID, BundlerSettingsCache> cache, Bundler bundler)
        {
            return !cache.ContainsKey(bundler.ID) 
                || cache.ContainsKey(bundler.ID) && cache[bundler.ID].HasChanged(bundler);
        }

        public static bool HasAgressiveCache(this Dictionary<ID, BundlerCache> cache, Bundler bundler)
        {
            return !cache.ContainsKey(bundler.ID)
                   || cache.ContainsKey(bundler.ID) && !string.IsNullOrEmpty(cache[bundler.ID].AgressiveCache);
        }

        public static bool HasBundlesCache(this Dictionary<ID, BundlerCache> cache, Bundler bundler)
        {
            return !cache.ContainsKey(bundler.ID)
                   || cache.ContainsKey(bundler.ID) && cache[bundler.ID].Bundles.Any();
        }

        public static bool BundlerIsRegistered(this Dictionary<ID, BundlerCache> cache, Bundler bundler)
        {
            return !cache.ContainsKey(bundler.ID)
                   || cache.ContainsKey(bundler.ID) && cache[bundler.ID].BundlerRegistered;
        }

        public static void Add(this Dictionary<ID, BundlerSettingsCache> cache, Bundler bundler)
        {
            cache[bundler.ID] = new BundlerSettingsCache(bundler);
        }
    }
}