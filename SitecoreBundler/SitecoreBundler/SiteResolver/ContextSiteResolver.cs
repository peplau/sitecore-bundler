using Sitecore.Sites;

namespace SitecoreBundler.SiteResolver
{
    public class ContextSiteResolver : ISiteResolver
    {
        public SiteContext Run()
        {
            return Sitecore.Context.Site;
        }
    }
}