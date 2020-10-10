using System;
using Newtonsoft.Json;

namespace LCSSaveEditor.GUI.Utils
{
    public class GitHubReleaseAsset
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("content_type")]
        public string ContentType { get; set; }

        [JsonProperty("browser_download_url")]
        public string Url { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("created_at")]
        public DateTime Date { get; set; }
    }
}
