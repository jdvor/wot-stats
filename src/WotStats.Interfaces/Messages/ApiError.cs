namespace WotStats.Interfaces.Messages
{
    using System.Text.Json.Serialization;

    public class ApiError
    {
        [JsonPropertyName("code")]
        public string? Code { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("field")]
        public string? Field { get; set; }

        [JsonPropertyName("value")]
        public string? Value { get; set; }
    }
}
