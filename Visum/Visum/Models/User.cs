
namespace Visum.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class User
    {
        [JsonProperty(PropertyName = "_id")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "estado")]
        public int Status { get; set; }

        [JsonProperty(PropertyName = "tipo")]
        public int Type { get; set; }

        [JsonProperty(PropertyName = "tokens")]
        public List<Token> Tokens { get; set; }

        [JsonProperty(PropertyName = "nombres")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "apellidos")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "correo")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "fechaNacimiento")]
        public string Birthdate { get; set; }

        [JsonProperty(PropertyName = "genero")]
        public string Gender { get; set; }

        [JsonProperty(PropertyName = "celular")]
        public string CellPhone { get; set; }

        [JsonProperty(PropertyName = "fechaRegistro")]
        public string RegistrationDate { get; set; }

        [JsonProperty(PropertyName = "clave")]
        public string Password { get; set; }
    }
}
