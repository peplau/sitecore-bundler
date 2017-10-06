using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace SitecoreBundler.Bundling
{
    public class BundleRepository
    {
        public static List<CdnBundle.Bundle> GetJavascriptBundle(string bundleBlock, string localPath, bool minify)
        {
            var bundles = new List<CdnBundle.Bundle>();

            var jsFooterLines = bundleBlock.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            foreach (var line in jsFooterLines)
            {
                var path = GetJsBundlePath(line);
                var jsFile = GetBundleFile(path);
                jsFile = !jsFile.EndsWith(".js") ? $"{jsFile}.js" : jsFile;
                if (path == string.Empty)
                    continue;

                if (path.StartsWith("~"))
                    bundles.Add(new CdnBundle.Bundle(path, CdnBundle.Bundle.BundleType.JavaScript, minify));
                else
                    bundles.Add(new CdnBundle.Bundle(path, $"{localPath}/{jsFile}",
                        CdnBundle.Bundle.BundleType.JavaScript, minify));
            }

            return bundles;
        }

        public static List<CdnBundle.Bundle> GetCssBundle(string bundleBlock, string localPath, bool minify)
        {
            var bundles = new List<CdnBundle.Bundle>();

            var cssFooterLines = bundleBlock.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            foreach (var line in cssFooterLines)
            {
                var path = GetCssBundlePath(line);
                var cssFile = GetBundleFile(path);
                cssFile = !cssFile.EndsWith(".css") ? $"{cssFile}.css" : cssFile;
                if (path == string.Empty)
                    continue;

                if (path.StartsWith("~"))
                    bundles.Add(new CdnBundle.Bundle(path, CdnBundle.Bundle.BundleType.CSS, minify));
                else
                    bundles.Add(new CdnBundle.Bundle(path, $"{localPath}/{cssFile}",
                        CdnBundle.Bundle.BundleType.CSS, minify));
            }

            return bundles;
        }

        private static string GetBundleFile(string path)
        {
            var parts = path.Split(new[] { "/" }, StringSplitOptions.None);
            return GetValidFilename(parts.Last());
        }

        private static string GetValidFilename(string rawName)
        {
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
                rawName = rawName.Replace(c.ToString(), "");
            if (rawName.Length > 50)
                rawName = rawName.Substring(0, 50);
            return rawName;
        }

        private static XElement GetXmlFromString(string line)
        {
            XDocument document = null;
            try
            {
                document = XDocument.Parse(line);
            }
            catch
            {
                if (line.EndsWith(">"))
                {
                    try
                    {
                        line = line.Insert(line.Length - 1, "/");
                        document = XDocument.Parse(line);
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
            return document?.Root;
        }

        private static string GetCssBundlePath(string line)
        {
            try
            {
                var lineNode = GetXmlFromString(line);
                if (lineNode == null)
                    return string.Empty;

                if (lineNode.NodeType == XmlNodeType.Element)
                {
                    var attrHref = lineNode.Attribute("href");
                    if (attrHref == null)
                        return string.Empty;
                    var src = attrHref.Value;
                    if (!src.StartsWith("http"))
                        src = $"~/{src}";
                    return src;
                }
            }
            catch (Exception e)
            {
                // ignored
            }

            return string.Empty;
        }

        private static string GetJsBundlePath(string line)
        {
            try
            {
                var lineNode = GetXmlFromString(line);
                if (lineNode == null)
                    return string.Empty;

                if (lineNode.NodeType == XmlNodeType.Element)
                {
                    var attrSrc = lineNode.Attribute("src");
                    if (attrSrc == null)
                        return string.Empty;
                    var src = attrSrc.Value;
                    if (!src.StartsWith("http"))
                        src = $"~/{src}";
                    return src;
                }
            }
            catch (Exception e)
            {
                // ignored
            }

            return string.Empty;
        }
    }
}