namespace Visum.Models
{
    using Newtonsoft.Json;

    public class LoginRequest
    {
        [JsonProperty(PropertyName = "correo")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "clave")]
        public string Password { get; set; }
    }
}
