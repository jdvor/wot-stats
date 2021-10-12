namespace WotStats.Interfaces.Messages
{
    using System.Text.Json.Serialization;

    public class Player
    {
        [JsonPropertyName("nickname")]
        public string? Nickname { get; set; }

        [JsonPropertyName("account_id")]
        public int? AccountId { get; set; }
    }
}
