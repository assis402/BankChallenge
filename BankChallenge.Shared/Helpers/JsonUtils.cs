using Newtonsoft.Json;

namespace BankChallenge.Shared.Helpers;

public static class JsonUtils
{
    public static string ToJson(this object @object, Formatting formatting = Formatting.Indented)
        => JsonConvert.SerializeObject(@object, formatting);
    
    public static TObject ToObject<TObject>(this string json)
        => JsonConvert.DeserializeObject<TObject>(json);
}