using Newtonsoft.Json;

namespace OAuth2.Domain.Request
{
    public class GoogleLoginRequest
    {
        [JsonProperty("externalAccessToken")]
        public string? ExternalAccessToken { get; set; }
    }
}
