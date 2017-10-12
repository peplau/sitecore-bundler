using System.Collections.Generic;
using System.Linq;
using Sitecore.Data;
using SitecoreBundler.Models.Templates;

namespace SitecoreBundler.Cache
{
    public static class CacheExtensions
    {
        public static bool HasChanged(this Dictionary<ID, BundlerCache> cache, Bundler bundler)
        {
            return !cache.ContainsKey(bundler.ID) || cache[bundler.ID].HasChanged(bundler);
        }

        public static bool HasChanged(this Dictionary<ID, BundleGroupCache> cache, __BaseBundleGroup bundleGroup)
        {
            return !cache.ContainsKey(bundleGroup.ID) || cache[bundleGroup.ID].HasChanged(bundleGroup);
        }

        public static bool HasChangedAnyGroup(this Dictionary<ID, BundleGroupCache> cache, Bundler bundler)
        {
            foreach (var bundleGroup in bundler.GetBundleGroups())
            {
                if (!cache.ContainsKey(bundleGroup.ID))
                    return true;
                if (cache[bundleGroup.ID].HasChanged(bundleGroup))
                    return true;
            }
            return false;
        }

        public static bool HasAgressiveCacheOnCache(this Dictionary<ID, BundleGroupCache> cache, __BaseBundleGroup bundleGroup)
        {
            return cache.ContainsKey(bundleGroup.ID) && !string.IsNullOrEmpty(cache[bundleGroup.ID].AgressiveCacheContent); 
        }

        public static bool HasBundlesOnCache(this Dictionary<ID, BundleGroupCache> cache, __BaseBundleGroup bundleGroup)
        {
            return cache.ContainsKey(bundleGroup.ID) && cache[bundleGroup.ID].Bundles.Any();
        }

        public static bool BundlerIsRegistered(this Dictionary<ID, BundleGroupCache> cache, __BaseBundleGroup bundleGroup)
        {
            return cache.ContainsKey(bundleGroup.ID) && cache[bundleGroup.ID].BundlerRegistered;
        }

        public static void Add(this Dictionary<ID, BundlerCache> cache, Bundler bundler)
        {
            cache[bundler.ID] = new BundlerCache(bundler);
        }
    }
}