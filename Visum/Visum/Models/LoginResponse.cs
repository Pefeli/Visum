namespace Visum.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class LoginResponse
    {
        [JsonProperty(PropertyName = "completada")]
        public bool Complete { get; set; }

        [JsonProperty(PropertyName = "error")]
        public bool Error { get; set; }

        [JsonProperty(PropertyName = "mensaje")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "objeto")]
        public User User { get; set; }
    }
}
