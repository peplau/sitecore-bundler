using Sitecore.Sites;

namespace SitecoreBundler.Bundling.Repository
{
    public interface ISiteResolverRepository
    {
        SiteContext GetSiteFromAssembly(string assemblyName);
    }
}