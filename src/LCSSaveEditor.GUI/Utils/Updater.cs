using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LCSSaveEditor.Core;
using LCSSaveEditor.Core.Helpers;
using LCSSaveEditor.GUI.Extensions;
using Semver;

namespace LCSSaveEditor.GUI.Utils
{
    public class Updater
    {
        public static UpdaterSettings UpdaterSettings => Settings.TheSettings.Updater;
        public static bool IsDownloadInProgress { get; private set; }

        public static async Task<GitHubRelease[]> GetReleaseInfo()
        {
            using var resp = await GitHubApiGet("https://api.github.com/repos/whampson/lcs-save-editor/releases");
            if (resp == null)
            {
                Log.Error("Unable to get update information. Are you connected to the internet?");
                return new GitHubRelease[0];
            }
            if (resp.StatusCode != HttpStatusCode.OK)
            {
                return new GitHubRelease[0];
            }

            await using var contentStream = resp.GetResponseStream();
            using StreamReader contentReader = new StreamReader(contentStream);

            if (!JsonHelper.TryParseJson(contentReader.ReadToEnd(), out GitHubRelease[] releaseInfo))
            {
                Log.Error("Response is not a valid GitHub Release JSON object.");
                return new GitHubRelease[0];
            }

            return releaseInfo;
        }

        public static async Task<GitHubRelease> CheckForUpdate()
        {
            Log.Info("Checking for updates...");

            GitHubRelease[] releases = await GetReleaseInfo();
            if (releases.Length == 0)
            {
                Log.Info("No updates available.");
                return null;
            }

            GitHubRelease latest = releases[0];
            if (latest.Assets.Length == 0)
            {
                Log.Error("Release does not include any assets!");
                return null;
            }

            string newVersionString = latest.Tag;
            if (newVersionString.StartsWith("v"))
            {
                newVersionString = newVersionString.Substring(1);
            }

            SemVersion curVersion = SemVersion.Parse(App.Version);
            if (!SemVersion.TryParse(newVersionString, out SemVersion newVersion))
            {
                Log.Error($"Release version '{newVersionString}' is not of the correct format.");
                return null;
            }

            if (newVersion <= curVersion || latest.IsPreRelease && !UpdaterSettings.PreReleaseRing)
            {
                Log.Info("No updates available.");
                return null;
            }

            Log.Info($"Version {newVersionString} available!");
            return latest;
        }

        public static GitHubReleaseAsset GetUpdatePackageInfo(GitHubRelease releaseInfo)
        {
            var pkg = releaseInfo.Assets.FirstOrDefault();
            if (UpdaterSettings.StandaloneRing)
            {
                var standalonePkg = releaseInfo.Assets.FirstOrDefault(x => x.Name.Contains("standalone"));
                if (standalonePkg == null)
                {
                    Log.Error("Standalone update package not found in this release. Falling back to framework-dependent package.");
                }
                else
                {
                    pkg = standalonePkg;
                }
            }

            return pkg;
        }

        public static async Task DownloadUpdatePackage(GitHubReleaseAsset pkgInfo, string dest,
            CancellationToken cancellationToken = default,
            IProgress<double> progress = null)
        {
            Log.Info("Downloading update...");

            double sizeKB = pkgInfo.Size / 1024.0;
            double sizeMB = sizeKB / 1024.0;
            string size = (sizeMB < 1) ? $"{sizeKB:N} KB" : $"{sizeMB:N} MB";

            Log.Info($"   URL: {pkgInfo.Url}");
            Log.Info($"  Size: {size}");
            Log.Info($"  Dest: {dest}");

            using HttpClient client = new HttpClient
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            using FileStream file = new FileStream(dest, FileMode.Create, FileAccess.Write, FileShare.None);
            await client.DownloadAsync(pkgInfo.Url, file, cancellationToken, progress);
        }

        private static async Task<HttpWebResponse> GitHubApiGet(string url)
        {
            HttpWebRequest req = WebRequest.CreateHttp(url);
            req.Method = WebRequestMethods.Http.Get;
            req.UserAgent = App.AssemblyName;
            req.Accept = "application/vnd.github.v3+json";

            return await GetHttpResponse(req);
        }

        private static async Task<HttpWebResponse> GetHttpResponse(HttpWebRequest req)
        {
            try
            {
                return (HttpWebResponse) await req.GetResponseAsync();
            }
            catch (WebException e)
            {
                var resp = e.Response as HttpWebResponse;
                if (resp == null)
                {
                    Log.Error(e.Message);
                    return null;
                }
                if (resp.StatusCode != HttpStatusCode.OK)
                {
                    Log.Error($"HTTP {req.Method} returned {(int) resp.StatusCode} ({resp.StatusDescription}).");
                }

                return resp;
            }
        }
    }
}
