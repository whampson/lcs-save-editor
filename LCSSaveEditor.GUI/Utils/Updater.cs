using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LCSSaveEditor.Core;
using LCSSaveEditor.Core.Helpers;
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

        public static bool DownloadUpdatePackage(GitHubRelease update,
            DownloadProgressChangedEventHandler downloadProgressChangedHandler = null,
            AsyncCompletedEventHandler downloadCompletedHandler = null)
        {
            if (IsDownloadInProgress)
            {
                return false;
            }

            Log.Info("Downloading update...");

            string downloadDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(downloadDir);
            var pkg = update.Assets[0];

            if (UpdaterSettings.StandaloneRing)
            {
                var standalonepkg = update.Assets.FirstOrDefault(x => x.Name.Contains("standalone"));
                if (standalonepkg != null)
                {
                    pkg = standalonepkg;
                }
                else
                {
                    Log.Error("Standalone update package not found in this release. Falling back to framework-dependent package.");
                }
            }

            float sizeKB = pkg.Size / 1024;
            float sizeMB = sizeKB / 1024;
            string size = (sizeMB < 1) ? $"{sizeKB:N} KB" : $"{sizeMB:N} MB";
            Log.Info($"   URL: {pkg.Url}");
            Log.Info($"  Size: {size}");
            Log.Info($"  Dest: {downloadDir}\\");
            Log.Info($"Download progress: 0%");

            string destPath = Path.Combine(downloadDir, pkg.Name);
            
            using WebClient wc = new WebClient();
            wc.DownloadFileCompleted += DownloadFileCompleted;
            if (downloadCompletedHandler != null)
            {
                wc.DownloadFileCompleted += downloadCompletedHandler;
            }
            if (downloadProgressChangedHandler != null)
            {
                wc.DownloadProgressChanged += downloadProgressChangedHandler;
            }

            wc.DownloadFileAsync(new Uri(pkg.Url), destPath, destPath);
            
            IsDownloadInProgress = true;
            return true;
        }

        private static void DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            IsDownloadInProgress = false;
        }

        public static string InstallUpdatePackage(string pkgPath)
        {
            Log.Info("Extracting update...");
            string extractDir = Path.Combine(Path.GetDirectoryName(pkgPath), "extract");
            ZipFile.ExtractToDirectory(pkgPath, extractDir);

            string newExeDownloadPath = null;
            string newExeName = null;

            foreach (string filePath in Directory.GetFiles(extractDir))
            {
                if (Path.GetExtension(filePath) == ".exe")
                {
                    newExeDownloadPath = filePath;
                    newExeName = Path.GetFileName(newExeDownloadPath);
                    Log.Info($"Found executable '{newExeName}'");
                    break;
                }
            }

            if (newExeDownloadPath == null)
            {
                Log.Error("Package does not contain an executable!");
                return null;
            }

            Log.Info("Installing update...");
            string oldExe = Process.GetCurrentProcess().MainModule.FileName;

            string oldExeBak = oldExe + ".bak";
            string newExe = Path.Combine(Path.GetDirectoryName(oldExe), newExeName);

            File.Move(oldExe, oldExeBak, true);
            File.Copy(newExeDownloadPath, newExe);

            UpdaterSettings.CleanupAfterUpdate = true;
            UpdaterSettings.CleanupList.Add(oldExeBak);
            if (oldExe != newExe)
            {
                UpdaterSettings.CleanupList.Add(oldExe);
            }

            return newExe;
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
                if (resp.StatusCode != HttpStatusCode.OK)
                {
                    Log.Error($"HTTP {req.Method} returned {(int) resp.StatusCode} ({resp.StatusDescription}).");
                }

                return resp;
            }
        }
    }
}
