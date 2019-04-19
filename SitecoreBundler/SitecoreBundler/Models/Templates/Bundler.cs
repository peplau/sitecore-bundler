using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.Sites;
using SitecoreBundler.Bundling.Repository;
using SitecoreBundler.Log;

namespace SitecoreBundler.Models.Templates
{
    public partial class Bundler
    {
        public static SiteContext Site =>
            new SiteResolverRepository().GetSiteFromAssembly(Settings.BundleSettings.SiteResolverAssembly);
        public bool IsBundleEnabled => BundleEnabled.HasValue && BundleEnabled.Value;
        public bool IsMinifyEnabled => MinifyEnabled.HasValue && MinifyEnabled.Value;
        public bool IsAgressiveCacheEnabled => AgressiveCacheEnabled.HasValue && AgressiveCacheEnabled.Value;
        public bool? BundleEnabled => GetBoolFromDictionary(Bundle.TargetItem);
        public bool? MinifyEnabled => GetBoolFromDictionary(Minify.TargetItem);
        public bool? AgressiveCacheEnabled => GetBoolFromDictionary(AgressiveCache.TargetItem);

        private bool? GetBoolFromDictionary(Item dicItem)
        {
            if (dicItem == null || dicItem.TemplateID != DictionaryEntry.TemplateID)
                return null;
            var dicEntry = new DictionaryEntry(dicItem);
            if (string.IsNullOrEmpty(dicEntry.Phrase))
                return null;
            bool result;
            if (!bool.TryParse(dicEntry.Phrase, out result))
                return null;
            return result;
        }

        public static Bundler GetBundler()
        {
            if (Site == null)
                return null;

            var logger = new Logger();

            // Get bundler from GlobalBundlerItem
            var globalBundlerId = Settings.BundleSettings.GlobalBundlerItem;
            try
            {
                if (globalBundlerId != ID.Null)
                {
                    logger.Start($"Get GlobalBundlerItem ID {globalBundlerId} from Sitecore");
                    var globalBundlerItem = Sitecore.Context.Database.GetItem(globalBundlerId);
                    logger.Finish();
                    if (globalBundlerItem != null)
                        return new Bundler(globalBundlerItem);
                }
            }
            catch (Exception e)
            {
                Sitecore.Diagnostics.Log.Error(
                    $"[SitecoreBundler] Error getting Bundler from GlobalBundlerItem - Item ID: {globalBundlerId}", e, e.GetType());
                return null;
            }

            // Get Bundler relative to the current website
            try
            {
                // Format path to safe quering
                var contentStartPath = Site.ContentStartPath;
                contentStartPath = contentStartPath.Replace("/","#/#");
                if (contentStartPath.StartsWith("#/"))
                    contentStartPath = contentStartPath.Substring(1, contentStartPath.Length-1);
                if (contentStartPath.EndsWith("/#"))
                    contentStartPath = contentStartPath.Substring(0, contentStartPath.Length-2);
                else if (!contentStartPath.EndsWith("#"))
                    contentStartPath += "#";

                // Execute query and get all possible options
                var query =
                    $"{contentStartPath}//*[@@templateid='{TemplateID}' and (@Layout='{new ID(RenderingContext.Current.Rendering.LayoutId)}' or @Layout='')]";
                logger.Start($"Get Bundle Item - Step 1: query ({query})");
                var bundlerItems = Sitecore.Context.Database.SelectItems(query).Select(p => new Bundler(p)).ToArray();
                logger.Finish();

                // Get proper bundlerItem using LINQ
                logger.Start($"Get Bundle Item - Step 2: LINQ filter ({query})");
                Item bundlerItem = null;
                if (bundlerItems.Any(p => p.Layout.TargetID.ToGuid() == RenderingContext.Current.Rendering.LayoutId))
                    bundlerItem = bundlerItems.First(p =>
                        p.Layout.TargetID.ToGuid() == RenderingContext.Current.Rendering.LayoutId);
                else if (bundlerItems.Any(p => p.Layout == null))
                    bundlerItem = bundlerItems.First(p => p.Layout == null);
                logger.Finish();

                return bundlerItem == null ? null : new Bundler(bundlerItem);
            }
            catch (Exception e)
            {
                Sitecore.Diagnostics.Log.Error(
                    $"[SitecoreBundler] Error getting Bundler relative to current website - Website name: {Site.Name}",
                    e, e.GetType());
                return null;
            }
        }

        public List<__BaseBundleGroup> GetBundleGroups()
        {
            return this.InnerItem.Children
                .Where(p => p.TemplateID == JavascriptBundler.TemplateID || p.TemplateID == CssBundler.TemplateID)
                .Select(p => new __BaseBundleGroup(p)).ToList();
        }

        public __BaseBundleGroup GetBundleGroup(string filename)
        {
            var query = InnerItem.Children.Where(p =>
                    p.TemplateID == JavascriptBundler.TemplateID || p.TemplateID == CssBundler.TemplateID)
                .Select(p => new __BaseBundleGroup(p)).Where(p => p.BundledFilename == filename).ToList();

            var bundleGroup = query.Any() ? new __BaseBundleGroup(query.First()) : null;
            if (bundleGroup == null)
                return null;
            if (Sitecore.Context.PageMode.IsExperienceEditorEditing && bundleGroup.ExperienceEditor.TargetItem!=null)
                return new __BaseBundleGroup(bundleGroup.ExperienceEditor.TargetItem);
            return bundleGroup;
        }

        public string BundleLocalAbsolutePath
        {
            get
            {
                var localUrlPath = BundleLocalPath;
                if (HttpContext.Current != null && BundleLocalPath.StartsWith("~"))
                    localUrlPath = HttpContext.Current.Server.MapPath(BundleLocalPath);
                else if (!string.IsNullOrEmpty(localUrlPath))
                    return localUrlPath.Replace("~", Environment.CurrentDirectory.Replace(@"\", "/"));
                return localUrlPath ?? "";
            }
        }
    }
}