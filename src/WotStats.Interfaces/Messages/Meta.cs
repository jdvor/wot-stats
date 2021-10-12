namespace WotStats.Interfaces.Messages
{
    using System.Text.Json.Serialization;

    public class Meta
    {
        [JsonPropertyName("count")]
        public int? Count { get; set; }

        [JsonPropertyName("page_total")]
        public int? PageTotal { get; set; }

        [JsonPropertyName("total")]
        public int? Total { get; set; }

        [JsonPropertyName("limit")]
        public int? Limit { get; set; }

        [JsonPropertyName("page")]
        public int? Page { get; set; }

        public bool IsEmpty
            => !Count.HasValue && !PageTotal.HasValue && !Total.HasValue && !Limit.HasValue && !Page.HasValue;
    }
}
