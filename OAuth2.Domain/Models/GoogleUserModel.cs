using Newtonsoft.Json;

namespace OAuth2.Domain.Models
{
    public class GoogleUserModel
    {
        [JsonProperty("email")]
        public string? Email { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }
    }
}
