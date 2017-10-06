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

        internal static readonly Dictionary<ID, BundlerSettingsCache> BundlerSettingsCache =
            new Dictionary<ID, BundlerSettingsCache>();
        internal static readonly Dictionary<ID, BundlerCache> BundlerCache = new Dictionary<ID, BundlerCache>();

        internal string GetBundleString(Bundler bundler, __BaseBundleGroup bundleGroup, string filename)
        {
            var logger = new Logger();

            // Clean up caches (filesystem and memory) if any change at the setup occured
            SetupBundleCache(bundler);

            // Return from Cache (if available)
            if (bundler.AgressiveCache && BundlerCache.HasAgressiveCache(bundler))
                return BundlerCache[bundler.ID].AgressiveCache;

            // Return raw value if bundle is disabled
            if (!bundler.Bundle)
                return bundleGroup.Bundles;

            // Get bundles (from Sitecore or cache)
            List<Bundle> bundles;
            if (BundlerCache.HasBundlesCache(bundler))
                bundles = BundlerCache[bundler.ID].Bundles;
            else
            {
                logger.Start("Get bundles using GetJavascriptBundle or GetCssBundle");
                bundles = bundleGroup.InnerItem.TemplateID == JavascriptBundler.TemplateID
                    ? _bundleRepository.GetJavascriptBundle(bundleGroup.Bundles, bundler.BundleLocalPath, bundler.Minify)
                    : _bundleRepository.GetCssBundle(bundleGroup.Bundles, bundler.BundleLocalPath, bundler.Minify);
                BundlerCache[bundler.ID] = new BundlerCache
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
            BundlerCache[bundler.ID].AgressiveCache = ret;
            logger.Finish();

            return ret;
        }

        private void SetupBundleCache(Bundler bundler)
        {
            // Clean up bundle Cache if needed
            if (BundlerSettingsCache.HasChanged(bundler))
            {
                var logger = new Logger();
                logger.Start("SetupBundleCache");

                BundlerSettingsCache.Add(bundler);
                try
                {
                    // Clean up caches in memory
                    BundlerCache.Remove(bundler.ID);
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
}