using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LCSSaveEditor.GUI.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task DownloadAsync(this HttpClient client, string requestUri, Stream destination,
            CancellationToken cancellationToken = default,
            IProgress<double> progress = null)
        {
            using (var resp = await client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead))
            {
                var contentLength = resp.Content.Headers.ContentLength;
                using (var download = await resp.Content.ReadAsStreamAsync())
                {
                    if (progress == null || !contentLength.HasValue)
                    {
                        await download.CopyToAsync(destination);
                        return;
                    }

                    var relativeProgress = new Progress<int>(totalBytes => progress.Report((double) totalBytes / contentLength.Value));
                    await download.CopyToAsync(destination, 8192, cancellationToken, relativeProgress);
                    progress.Report(1);
                }
            }
        }
    }
}
