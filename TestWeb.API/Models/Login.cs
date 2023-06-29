using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TestWeb.API.Models
{
    public class Login
    {
        [Key]
        public string Username { get; set; }
        public string Password { get; set; }
        [JsonIgnore]
        public string? AccessToken { get; set; }
        [JsonIgnore]
        public string? RefreshToken { get; set; }
    }
}
