namespace SitecoreBundler.Models.Templates
{
    public partial class __BaseBundleGroup
    {
        public Bundler GetBundler()
        {
            return new Bundler(InnerItem.Parent);
        }
    }
}