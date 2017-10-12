using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using CdnBundle;
using Sitecore.Data;
using SitecoreBundler.Bundling.Repository;
using SitecoreBundler.Cache;
using SitecoreBundler.Log;
using SitecoreBundler.Models.Templates;

namespace SitecoreBundler.Bundling
{
    public class SitecoreBundler
    {
        private readonly IBundleRepository _bundleRepository;
        public SitecoreBundler(IBundleRepository bundleRepository)
        {
            _bundleRepository = bundleRepository;
        }
        public SitecoreBundler() : this(new BundleRepository()) { }

        private static SitecoreBundler _instance;
        public static SitecoreBundler Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;
                _instance = new SitecoreBundler();
                return _instance;
            }
        }

        internal static readonly Dictionary<ID, BundlerCache> BundlerCache = new Dictionary<ID, BundlerCache>();
        internal static readonly Dictionary<ID, BundleGroupCache> BundleGroupCache = new Dictionary<ID, BundleGroupCache>();

        internal string GetBundleString(Bundler bundler, __BaseBundleGroup bundleGroup, string filename)
        {
            var logger = new Logger();

            if (!BundleGroupCache.ContainsKey(bundleGroup.ID))
                BundleGroupCache.Add(bundleGroup.ID, new BundleGroupCache(bundleGroup));

            // Clean up caches (filesystem and memory) if any change at the setup occured
            SetupBundleCache(bundler, bundleGroup);

            if (!BundlerCache.ContainsKey(bundler.ID))
                BundlerCache.Add(bundler.ID, new BundlerCache(bundler));

            // Return from Cache (if available)
            // Aggressive Cache is enabled if 1) Bundler forces it OR 2) BundleGroup has it enabled
            if ((bundler.IsAgressiveCacheEnabled || bundleGroup.AgressiveCache) && BundleGroupCache.HasAgressiveCacheOnCache(bundleGroup))
                return BundleGroupCache[bundleGroup.ID].AgressiveCacheContent;

            // Return raw value if bundle is disabled
            // Bundle is enabled if 1) Bundler forces it OR 2) BundleGroup has it enabled
            if (!bundler.IsBundleEnabled && !bundleGroup.Bundle)
                return bundleGroup.Bundles;

            // Get bundles (from Sitecore or cache)
            List<Bundle> bundles;
            if (BundleGroupCache.HasBundlesOnCache(bundleGroup))
                bundles = BundleGroupCache[bundleGroup.ID].Bundles;
            else
            {
                logger.Start("Get bundles using GetJavascriptBundle or GetCssBundle");

                var minify = bundler.IsMinifyEnabled || bundleGroup.Minify;
                bundles = bundleGroup.InnerItem.TemplateID == JavascriptBundler.TemplateID
                    ? _bundleRepository.GetJavascriptBundle(bundleGroup.Bundles, bundler.BundleLocalPath, minify)
                    : _bundleRepository.GetCssBundle(bundleGroup.Bundles, bundler.BundleLocalPath, minify);
                BundleGroupCache[bundleGroup.ID] = new BundleGroupCache(bundleGroup)
                {
                    Bundles = bundles,
                    BundlerRegistered = true
                };
                logger.Finish();
            }

            // Process and render
            logger.Start("Process and render bundles");
            var ret = bundles != null && bundles.Any()
                ? bundles.Load($"{bundler.BundleLocalPath}/{filename}")
                : string.Empty;
            BundleGroupCache[bundleGroup.ID].AgressiveCacheContent = ret;
            logger.Finish();

            return ret;
        }

        private static void SetupBundleCache(Bundler bundler, __BaseBundleGroup bundleGroup)
        {
            // Clean up bundle Cache if needed
            if (!BundlerCache.HasChanged(bundler) && !BundleGroupCache.HasChangedAnyGroup(bundler))
                return;

            var logger = new Logger();
            logger.Start("SetupBundleCache");

            try
            {
                // Clean up caches in memory
                BundlerCache.Remove(bundler.ID);
                foreach (var baseBundleGroup in bundler.GetBundleGroups())
                    if (BundleGroupCache.ContainsKey(baseBundleGroup.ID))
                        BundleGroupCache.Remove(baseBundleGroup.ID);

                BundleListExtensions.ClearAllRecords();
                CdnBundle.Bundle.ClearAllRecords();

                // Get folder or create if not exists
                var dir = Directory.CreateDirectory(bundler.BundleLocalAbsolutePath);

                // Delete files one by one
                foreach (var file in dir.GetFiles())
                {
                    try
                    {
                        file.Delete();
                    }
                    catch(Exception e)
                    {
                        Sitecore.Diagnostics.Log.Error(
                            $"[SitecoreBundler] Cannot delete file '{file.Name}' while cleaning bundle folder {bundler.BundleLocalAbsolutePath}",
                            e,e.GetType());
                    }
                }
            }
            catch (Exception e)
            {
                Sitecore.Diagnostics.Log.Error($"[SitecoreBundler] Error cleaning up folder {bundler.BundleLocalAbsolutePath}", e, e.GetType());
            }

            logger.Finish();
        }
    }
}