using Newtonsoft.Json;

namespace CrossPuzzleClient.Infrastructure
{
    public static class JsonUtility
    {
        public static string ToJson(object obj)
        {
            if (obj != null)
            {
                return JsonConvert.SerializeObject(obj, GetJsonSerializerSettings());
            }
            else
            {
                return string.Empty;
            }
        }

        public static TOutput FromJson<TOutput>(string text)
        {
            if (text != null)
            {
                return JsonConvert.DeserializeObject<TOutput>(text, GetJsonSerializerSettings());
            }
            else
            {
                return default(TOutput);
            }
        }

        private static JsonSerializerSettings GetJsonSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                Formatting = Formatting.None
            };
        }
    }

}