namespace Visum.Models
{
    using Newtonsoft.Json;

    public class Token
    {
        [JsonProperty(PropertyName = "access")]
        public string Access { get; set; }

        [JsonProperty(PropertyName = "token")]
        public string Value { get; set; }
    }
}
