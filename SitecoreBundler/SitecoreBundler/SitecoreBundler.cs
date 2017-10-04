using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using CdnBundle;
using Sitecore.Data;
using Sitecore.Diagnostics;
using SitecoreBundler.Cache;
using SitecoreBundler.Models.Templates;
using SitecoreBundler.Repository;

namespace SitecoreBundler
{
    public static class SitecoreBundler
    {
        private static readonly Dictionary<ID, BundlerSettingsCache> BundlerSettingsCache =
            new Dictionary<ID, BundlerSettingsCache>();
        private static readonly Dictionary<ID, BundlerCache> BundlerCache = new Dictionary<ID, BundlerCache>();

        public static HtmlString Bundle(string filename)
        {
            var ret = "";

            // Get current bundler
            var bundler = Bundler.GetBundler();
            if (bundler==null)
                return new HtmlString(ret);

            // Get Bundle Group from filename
            var bundleGroup = bundler.GetBundleGroup(filename);
            if (bundleGroup == null)
                return new HtmlString(ret);

            ret = GetBundleString(bundler, bundleGroup, filename);
            return new HtmlString(ret);
        }

        private static string GetBundleString(Bundler bundler, __BaseBundleGroup bundleGroup, string filename)
        {
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
                bundles = bundleGroup.InnerItem.TemplateID == JavascriptBundler.TemplateID
                    ? BundleRepository.GetJavascriptBundle(bundleGroup.Bundles, bundler.BundleLocalPath, bundler.Minify)
                    : BundleRepository.GetCssBundle(bundleGroup.Bundles, bundler.BundleLocalPath, bundler.Minify);
                BundlerCache[bundler.ID] = new BundlerCache
                {
                    Bundles = bundles,
                    BundlerRegistered = true
                };
            }

            // Process and render
            var ret = bundles != null && bundles.Any()
                ? bundles.Load($"{bundler.BundleLocalPath}/{filename}")
                : string.Empty;
            BundlerCache[bundler.ID].AgressiveCache = ret;
            return ret;
        }

        private static void SetupBundleCache(Bundler bundler)
        {
            // Clean up bundle Cache if needed
            if (BundlerSettingsCache.HasChanged(bundler))
            {
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
                            Log.Error(
                                $"Cannot delete file '{file.Name}' while cleaning bundle folder {bundler.BundleLocalAbsolutePath}",
                                e,e.GetType());
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Error($"Error cleaning up folder {bundler.BundleLocalAbsolutePath}", e, e.GetType());
                }
            }
        }
    }
}