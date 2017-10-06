using System.Collections.Generic;

namespace SitecoreBundler.Bundling.Repository
{
    public interface IBundleRepository
    {
        List<CdnBundle.Bundle> GetJavascriptBundle(string bundleBlock, string localPath, bool minify);
        List<CdnBundle.Bundle> GetCssBundle(string bundleBlock, string localPath, bool minify);
    }
}