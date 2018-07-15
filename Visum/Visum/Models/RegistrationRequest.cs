namespace Visum.Models
{
    using Newtonsoft.Json;

    public class RegistrationRequest
    {
        [JsonProperty(PropertyName = "nombres")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "apellidos")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "correo")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "clave")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "fechaNacimiento")]
        public string Birthdate { get; set; }

        [JsonProperty(PropertyName = "genero")]
        public string Gender { get; set; }

        [JsonProperty(PropertyName = "estado")]
        public int Status { get; set; }

        [JsonProperty(PropertyName = "celular")]
        public string CellPhone { get; set; }

        [JsonProperty(PropertyName = "tipo")]
        public int Type { get; set; }
    }
}
