using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace Jera
{

    public class JSONConverter
    {
        public static Dictionary<string, Dictionary<string, string>> FromJson(string json) => JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Dictionary<string, Dictionary<string, string>> self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter

    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}