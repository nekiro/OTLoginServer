using Newtonsoft.Json;

namespace OTLoginServer.Models
{
    public class ErrorResponse
    {
        [JsonProperty("errorCode")]
        public int ErrorCode { get; set; }

        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }
    }
}
