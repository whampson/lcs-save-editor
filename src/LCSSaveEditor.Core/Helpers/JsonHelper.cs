using Newtonsoft.Json;

namespace LCSSaveEditor.Core.Helpers
{
    public static class JsonHelper
    {
        public static bool TryParseJson<T>(string json, out T result)
        {
            bool success = true;
            var settings = new JsonSerializerSettings
            {
                Error = (o, e) => { success = false; e.ErrorContext.Handled = true; }
            };

            result = JsonConvert.DeserializeObject<T>(json, settings);
            return success;
        }
    }
}
