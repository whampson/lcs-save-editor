using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LCSSaveEditor.Core;
using Newtonsoft.Json;

namespace LCSSaveEditor.GUI.Utils
{
    public class Updater
    {
        public static UpdaterSettings UpdaterSettings => Settings.TheSettings.Updater;

        public static async Task<GitHubRelease[]> GetReleaseInfo()
        {
            using var resp = await GitHubApiGet("https://api.github.com/repos/whampson/pigeon-locator/releases");
            JsonSerializer jsonSerializer = new JsonSerializer();

            await using var contentStream = resp.GetResponseStream();
            using StreamReader contentReader = new StreamReader(contentStream);
            return (GitHubRelease[]) jsonSerializer.Deserialize(contentReader, typeof(GitHubRelease[]));
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

            bool preReleaseRing = UpdaterSettings.PreReleaseRing;
            GitHubRelease latest = releases[0];
            string newVersionFull = latest.Tag;

            if (newVersionFull.StartsWith("v"))
            {
                newVersionFull = newVersionFull.Substring(1);
            }

            string newVersion = newVersionFull;
            if (newVersion.Contains("-"))
            {
                int dash = newVersion.IndexOf("-");
                newVersion = newVersion.Substring(0, dash);
            }

            if (!Version.TryParse(newVersion, out Version newAssemblyVersion))
            {
                Log.Error($"Release version '{newVersion}' is not of the correct format.");
                return null;
            }

            if (latest.Assets.Length == 0)
            {
                Log.Error("Release does not include any assets!");
                return null;
            }

            if (newAssemblyVersion <= App.AssemblyVersion || latest.IsPreRelease && !preReleaseRing)
            {
                Log.Info("No updates available.");
                return null;
            }

            Log.Info($"Version {newVersionFull} available!");
            return latest;
        }

        public static void DownloadUpdatePackage(GitHubRelease update,
            DownloadProgressChangedEventHandler downloadProgressChangedHandler = null,
            AsyncCompletedEventHandler downloadCompletedHandler = null)
        {
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
            if (downloadProgressChangedHandler != null) wc.DownloadProgressChanged += downloadProgressChangedHandler;
            if (downloadProgressChangedHandler != null) wc.DownloadFileCompleted += downloadCompletedHandler;
            wc.DownloadFileAsync(new Uri(pkg.Url), destPath, destPath);
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

            if (File.Exists(oldExeBak))
            {
                File.Delete(oldExeBak);
            }
            if (File.Exists(newExe))
            {
                File.Delete(newExe);
            }

            File.Move(oldExe, oldExeBak);
            File.Copy(newExeDownloadPath, newExe);

            Settings.TheSettings.Updater.CleanupAfterUpdate = true;
            Settings.TheSettings.Updater.CleanupList.Add(oldExeBak);

            return newExeDownloadPath;
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
            HttpWebResponse resp = (HttpWebResponse) await req.GetResponseAsync();
            if (resp.StatusCode != HttpStatusCode.OK)
            {
                Log.Info($"HTTP {req.Method} returned {resp.StatusCode}");
            }

            return resp;
        }
    }
}
