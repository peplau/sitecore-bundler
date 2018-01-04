using Sitecore.Sites;

namespace SitecoreBundler.SiteResolver
{
    public interface ISiteResolver
    {
        SiteContext Run();
    }
}