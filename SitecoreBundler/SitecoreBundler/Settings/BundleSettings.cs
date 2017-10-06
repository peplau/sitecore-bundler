using Sitecore.Data;

namespace SitecoreBundler.Settings
{
    public static class BundleSettings
    {
        public static ID GlobalBundlerItem
        {
            get
            {
                var settingVal = Sitecore.Configuration.Settings.GetSetting(Constants.SettingsKeys.GlobalBundlerItem);
                if (string.IsNullOrEmpty(settingVal))
                    return ID.Null;
                ID globalBundlerId;
                return ID.TryParse(settingVal, out globalBundlerId) ? globalBundlerId : ID.Null;
            }
        }

        public static bool DebugMode =>
            Sitecore.Configuration.Settings.GetBoolSetting(Constants.SettingsKeys.DebugMode, false);
    }
}