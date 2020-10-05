using System;
using Newtonsoft.Json;

namespace LCSSaveEditor.GUI.Utils
{
    public class GitHubRelease
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("tag_name")]
        public string Tag { get; set; }

        [JsonProperty("published_at")]
        public DateTime Date { get; set; }

        [JsonProperty("html_url")]
        public string Url { get; set; }

        [JsonProperty("body")]
        public string Notes { get; set; }

        [JsonProperty("prerelease")]
        public bool IsPreRelease { get; set; }

        [JsonProperty("assets")]
        public GitHubReleaseAsset[] Assets { get; set; }

        public GitHubRelease()
        {
            Assets = new GitHubReleaseAsset[0];
        }
    }
}
