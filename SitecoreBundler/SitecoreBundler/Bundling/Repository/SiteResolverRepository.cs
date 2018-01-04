using System;
using Sitecore.Sites;

namespace SitecoreBundler.Bundling.Repository
{
    public class SiteResolverRepository : ISiteResolverRepository
    {
        public SiteContext GetSiteFromAssembly(string assemblyName)
        {
            try
            {
                var thisType = Type.GetType(assemblyName);
                if (thisType == null)
                    return null;

                // Instance of Resolver
                var obj = Activator.CreateInstance(thisType);

                // Call Run method
                var method = thisType.GetMethod("Run");
                if (method == null)
                    return null;

                var site = (SiteContext)method.Invoke(obj, null);
                return site;
            }
            catch (Exception e)
            {
                Sitecore.Diagnostics.Log.Error($"Cannot instantiate class from assembly '{assemblyName}'", e, this);
                return null;
            }
        }
    }
}