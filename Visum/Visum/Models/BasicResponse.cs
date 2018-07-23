namespace Visum.Models
{
    using Newtonsoft.Json;

    public class BasicResponse
    {
        [JsonProperty(PropertyName = "completada")]
        public bool Complete { get; set; }

        [JsonProperty(PropertyName = "error")]
        public bool Error { get; set; }

        [JsonProperty(PropertyName = "mensaje")]
        public string Message { get; set; }
    }
}
