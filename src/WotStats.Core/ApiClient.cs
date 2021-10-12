namespace WotStats.Core
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text.Json;
    using System.Threading.Tasks;
    using WotStats.Interfaces;
    using WotStats.Interfaces.Messages;

    public sealed class ApiClient : IDisposable, IApiClient
    {
        private readonly string applicationId;
        private readonly ApiClientOptions options;
        private readonly HttpClient http;
        private readonly JsonSerializerOptions jsonOptions;
        private bool disposed;

        public ApiClient(string applicationId, Realm realm = Realm.EU, ApiClientOptions? options = null)
        {
            this.applicationId = applicationId ?? throw new ArgumentNullException(nameof(applicationId));
            Realm = realm;
            this.options = options ?? ApiClientOptions.Default();
            http = CreateHttpClient(Realm, this.options);
            jsonOptions = new JsonSerializerOptions
            {
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true,
            };
        }

        public Realm Realm { get; }

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            http.Dispose();
            disposed = true;
        }

        public async Task<Response<Player[]>> FindPlayersAsync(params string[] nicknames)
        {
            var query = new (string key, object value)[]
            {
                ("search", string.Join(',', nicknames)),
                ("type", "exact"),
                ("language", "en"),
            };
            var response = await GetAsync<Player[]>("account/list", query).ConfigureAwait(false);

            return response;
        }

        private async Task<Response<T>> GetAsync<T>(string path, params (string key, object value)[] queryParams)
            where T : class
        {
            var endpoint = new EndpointBuilder(path)
                    .QueryParams(queryParams)
                    .QueryParam("application_id", options.ApplicationId)
                    .ToString();
            using var req = new HttpRequestMessage(HttpMethod.Get, endpoint);
            HttpResponseMessage? httpResp = null;
            try
            {
                httpResp = await http.SendAsync(req).ConfigureAwait(false);
                if (httpResp.IsSuccessStatusCode)
                {
                    using var stream = await httpResp.Content.ReadAsStreamAsync().ConfigureAwait(false);
                    var msg = await JsonSerializer.DeserializeAsync<ApiMessage<T>>(stream, jsonOptions).ConfigureAwait(false);
                    return new Response<T>(true) { HttpStatusCode = httpResp.StatusCode, Api = msg };
                }

                var (code, body) = await GetCodeAndBodyAsync(httpResp).ConfigureAwait(false);

                return new Response<T>(false) { HttpStatusCode = code, Error = body };
            }
            catch (Exception ex)
            {
                var (code, body) = await GetCodeAndBodyAsync(httpResp).ConfigureAwait(false);
                return new Response<T>(false) { Exception = ex, Error = body };
            }
            finally
            {
                if (httpResp != null)
                {
                    httpResp.Dispose();
                }
            }
        }

        private static async Task<(HttpStatusCode? code, string? body)> GetCodeAndBodyAsync(HttpResponseMessage? httpResp)
        {
            if (httpResp == null)
            {
                return (null, null);
            }

            try
            {
                var body = await httpResp.Content.ReadAsStringAsync().ConfigureAwait(false);
                return (httpResp.StatusCode, body);
            }
            catch
            {
                return (httpResp.StatusCode, null);
            }
        }

        private static Uri GetBaseUri(Realm realm)
        {
            return realm switch
            {
                Realm.EU => new Uri(@"https://api.worldoftanks.eu/wot/"),
                Realm.RU => new Uri(@"https://api.worldoftanks.ru/wot/"),
                Realm.NA => new Uri(@"https://api.worldoftanks.com/wot/"),
                Realm.Asia => new Uri(@"https://api.worldoftanks.asia/wot/"),
                _ => new Uri(@"https://api.worldoftanks.eu/wot/"),
            };
        }

        private static HttpClient CreateHttpClient(Realm realm, ApiClientOptions options)
        {
            var baseUri = GetBaseUri(realm);

#pragma warning disable CA2000 // (Dispose objects before losing scope); disposed by HttpClient
            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = false,
                AutomaticDecompression = DecompressionMethods.All,
                MaxRequestContentBufferSize = 61_440, // 64KB
                UseCookies = false,
            };
#pragma warning restore CA2000

            var client = new HttpClient(handler, disposeHandler: true)
            {
                BaseAddress = baseUri,
                MaxResponseContentBufferSize = 52_428_800, // 50MB
                Timeout = options.RequestTimeout,
            };

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
    }
}
