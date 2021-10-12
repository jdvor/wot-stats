namespace WotStats.Interfaces.Messages
{
    using System.Text.Json.Serialization;

    public class ApiMessage<T>
        where T : class
    {
        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("meta")]
        public Meta? Meta { get; set; }

        [JsonPropertyName("error")]
        public ApiError? Error { get; set; }

        [JsonPropertyName("data")]
        public T? Data { get; set; }

        public bool IsSuccess => Status != null && Status == "ok";

        public bool IsError => Status != null && Status == "error";

        public bool HasMeta => Meta != null && !Meta.IsEmpty;

        public bool HasData => IsSuccess && Data != null;
    }
}
