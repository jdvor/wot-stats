namespace WotStats.Interfaces.Messages
{
    using System;
    using System.Net;

    public sealed class Response<T>
        where T : class
    {
        public Response(bool success)
        {
            Success = success;
        }

        public bool Success { get; }

        public HttpStatusCode? HttpStatusCode { get; init; }

        public Exception? Exception { get; init; }

        public string? Error { get; init; }

        public ApiMessage<T>? Api { get; init; }
    }
}
