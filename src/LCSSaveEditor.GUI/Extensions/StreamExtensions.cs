using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LCSSaveEditor.GUI.Extensions
{
    public static class StreamExtensions
    {
        public static async Task CopyToAsync(this Stream source, Stream destination, int bufferSize,
            CancellationToken cancellationToken = default,
            IProgress<int> progress = null)
        {
            if (source == null) throw new ArgumentException(nameof(source));
            if (!source.CanRead) throw new ArgumentException("Stream must be readable.", nameof(source));
            if (destination == null) throw new ArgumentException(nameof(destination));
            if (!destination.CanWrite) throw new ArgumentException("Stream must be writable.", nameof(destination));
            if (bufferSize < 0) throw new ArgumentOutOfRangeException(nameof(bufferSize));

            byte[] buffer = new byte[bufferSize];
            int totalBytesRead = 0;

            int bytesRead;
            while ((bytesRead = await source.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false)) != 0)
            {
                await destination.WriteAsync(buffer, 0, bytesRead, cancellationToken).ConfigureAwait(false);
                totalBytesRead += bytesRead;
                progress?.Report(totalBytesRead);
            }

        }
    }
}
