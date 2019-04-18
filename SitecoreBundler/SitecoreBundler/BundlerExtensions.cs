using System.Web;
using SitecoreBundler.Log;
using SitecoreBundler.Models.Templates;

namespace SitecoreBundler
{
    public static class BundlerExtensions
    {
        public static HtmlString Bundle(this Sitecore.Mvc.Helpers.SitecoreHelper sitecoreHelper, string filename)
        {
            var ret = "";
            var logger = new Logger();

            // Get current bundler
            logger.Start("Call to Bundler.GetBundler()");
            var bundler = Bundler.GetBundler();
            logger.Finish();
            if (bundler == null)
                return new HtmlString(ret);

            // Get Bundle Group from filename
            logger.Start($"Call bundler.GetBundleGroup(\"{filename}\")");
            var bundleGroup = bundler.GetBundleGroup(filename);
            logger.Finish();
            if (bundleGroup == null)
                return new HtmlString(ret);
            if (bundleGroup.BundledFilename != null && filename != bundleGroup.BundledFilename)
                filename = bundleGroup.BundledFilename;

            logger.Start($"Call GetBundleString({bundler.DisplayName}, {bundleGroup.DisplayName}, \"{filename}\")");
            ret = Bundling.SitecoreBundler.Instance.GetBundleString(bundler, bundleGroup, filename);
            logger.Finish();

            return new HtmlString(ret);
        }
    }
}