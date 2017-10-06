using System;
using System.Linq;
using System.Web;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Presentation;

namespace SitecoreBundler.Models.Templates
{
    public partial class Bundler
    {
        public static Bundler GetBundler()
        {
            // Get bundler from GlobalBundlerItem
            var globalBundler =
                Sitecore.Configuration.Settings.GetSetting(Constants.SettingsKeys.GlobalBundlerItem);
            try
            {
                if (!string.IsNullOrEmpty(globalBundler))
                {
                    ID globalBundlerId;
                    if (ID.TryParse(globalBundler, out globalBundlerId))
                    {
                        var globalBundlerItem = Sitecore.Context.Database.GetItem(globalBundlerId);
                        if (globalBundlerItem != null)
                            return new Bundler(globalBundlerItem);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error($"Error getting Bundler from GlobalBundlerItem - Item ID: {globalBundler}", e, e.GetType());
                return null;
            }

            // Get Bundler relative to the current website
            try
            {
                var contentStartPath = Sitecore.Context.Site.ContentStartPath;
                contentStartPath = contentStartPath.Replace("/","#/#");
                if (contentStartPath.StartsWith("#/"))
                    contentStartPath = contentStartPath.Substring(1, contentStartPath.Length-1);
                if (contentStartPath.EndsWith("/#"))
                    contentStartPath = contentStartPath.Substring(0, contentStartPath.Length-2);
                else if (!contentStartPath.EndsWith("#"))
                    contentStartPath += "#";

                var query = $"fast:{contentStartPath}//*[@@templateid='{TemplateID}' and (@Layout='{new ID(RenderingContext.Current.Rendering.LayoutId)}' or @Layout='')]";
                var bundlerItems = Sitecore.Context.Database.SelectItems(query).Select(p => new Bundler(p)).ToArray();

                Item bundlerItem = null;
                if (bundlerItems.Any(p => p.Layout.TargetID.ToGuid() == RenderingContext.Current.Rendering.LayoutId))
                    bundlerItem = bundlerItems.First(p => p.Layout.TargetID.ToGuid() == RenderingContext.Current.Rendering.LayoutId);
                else if (bundlerItems.Any(p => p.Layout == null))
                    bundlerItem = bundlerItems.First(p => p.Layout == null);

                return bundlerItem == null ? null : new Bundler(bundlerItem);
            }
            catch (Exception e)
            {
                Log.Error(
                    $"Error getting Bundler relative to current website - Website name: {Sitecore.Context.Site.Name}",
                    e, e.GetType());
                return null;
            }
        }

        public __BaseBundleGroup GetBundleGroup(string filename)
        {
            var query = InnerItem.Children.Where(p =>
                    p.TemplateID == JavascriptBundler.TemplateID || p.TemplateID == CssBundler.TemplateID)
                .Select(p => new __BaseBundleGroup(p)).Where(p => p.BundledFilename == filename).ToList();
            return query.Any() ? new __BaseBundleGroup(query.First()) : null;
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