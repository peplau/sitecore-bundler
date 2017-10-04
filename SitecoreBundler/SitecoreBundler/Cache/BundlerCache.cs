using System.Collections.Generic;
using CdnBundle;

namespace SitecoreBundler.Cache
{
    public class BundlerCache
    {
        public BundlerCache()
        {
            Bundles = new List<Bundle>();
        }

        public string AgressiveCache { get; set; }
        public bool BundlerRegistered { get; set; }
        public List<Bundle> Bundles { get; set; }
    }
}