using System;
using System.Linq;
using MarkdownSharp;
using Newtonsoft.Json;

namespace LCSSaveEditor.GUI.Utils
{
    public class GitHubRelease
    {
        private Markdown m_markdown;

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("tag_name")]
        public string Tag { get; set; }

        [JsonProperty("published_at")]
        public DateTime PublishDate { get; set; }

        [JsonProperty("html_url")]
        public string Url { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("prerelease")]
        public bool IsPreRelease { get; set; }

        [JsonProperty("assets")]
        public GitHubReleaseAsset[] Assets { get; set; }

        public string HtmlBody => m_markdown.Transform(Body);

        public GitHubRelease()
        {
            m_markdown = new Markdown();
            Assets = new GitHubReleaseAsset[0];
        }
    }
}
