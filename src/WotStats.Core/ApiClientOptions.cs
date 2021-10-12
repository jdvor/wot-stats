namespace WotStats.Core
{
    using System;

    public sealed class ApiClientOptions
    {
        public string ApplicationId { get; set; } = "ecec8751c86a405f49b7d7ac7c1162cb";

        public TimeSpan RequestTimeout { get; set; } = TimeSpan.FromSeconds(30);

        public static ApiClientOptions Default()
        {
            return new ApiClientOptions();
        }
    }
}
