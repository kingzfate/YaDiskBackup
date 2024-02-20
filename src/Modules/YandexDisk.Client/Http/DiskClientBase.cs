using System.Net;
using System.Net.Http.Json;
using System.Runtime.Serialization;
using YaDiskBackup.YandexDisk.Client.Http.Serialization;
using YaDiskBackup.YandexDisk.Client.Protocol;

namespace YaDiskBackup.YandexDisk.Client.Http;

internal abstract class DiskClientBase
{
    private static readonly QueryParamsSerializer MvcSerializer = new QueryParamsSerializer();

    //private readonly MediaTypeFormatter[] _defaultFormatters =
    //{
    //        new JsonMediaTypeFormatter
    //        {
    //            SerializerSettings =
    //            {
    //                ContractResolver = new SnakeCasePropertyNamesContractResolver(),
    //                Converters = { new SnakeCaseEnumConverter() },
    //                DateFormatHandling = DateFormatHandling.IsoDateFormat,
    //                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
    //                DateParseHandling = DateParseHandling.DateTime
    //            }
    //        }
    //};

    private readonly IHttpClient _httpClient;
    private readonly ILogSaver _logSaver;
    private readonly Uri _baseUrl;

    protected DiskClientBase(ApiContext apiContext)
    {
        if (apiContext == null)
        {
            throw new ArgumentNullException(nameof(apiContext));
        }
        if (apiContext.HttpClient == null)
        {
            throw new ArgumentNullException(nameof(apiContext.HttpClient));
        }
        if (apiContext.BaseUrl == null)
        {
            throw new ArgumentNullException(nameof(apiContext.BaseUrl));
        }

        _httpClient = apiContext.HttpClient;
        _logSaver = apiContext.LogSaver;
        _baseUrl = apiContext.BaseUrl;
    }

    private Uri GetUrl(string relativeUrl, object request = null)
    {
        var uriBuilder = new UriBuilder(_baseUrl);
        uriBuilder.Path += relativeUrl ?? throw new ArgumentNullException(nameof(relativeUrl));

        if (request != null)
        {
            uriBuilder.Query = MvcSerializer.Serialize(request);
        }

        return uriBuilder.Uri;
    }

    private HttpContent GetContent<TRequest>(TRequest request)
        where TRequest : class
    {
        if (request == null)
        {
            return null;
        }

        if (typeof(TRequest) == typeof(string))
        {
            return new StringContent(request as string);
        }
        if (typeof(TRequest) == typeof(byte[]))
        {
            return new ByteArrayContent(request as byte[]);
        }
        if (typeof(Stream).IsAssignableFrom(typeof(TRequest)))
        {
            return new StreamContent(request as Stream);
        }

        return new ObjectContent<TRequest>(request, _defaultFormatters.First());
    }

    private ILogger GetLogger()
    {
        return LoggerFactory.GetLogger(_logSaver);
    }

    private async Task<TResponse> SendAsync<TResponse>(HttpRequestMessage request, CancellationToken cancellationToken)
        where TResponse : class, new()
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        HttpResponseMessage responseMessage = await SendAsync(request, cancellationToken).ConfigureAwait(false);

        TResponse response = await ReadResponse<TResponse>(responseMessage, cancellationToken).ConfigureAwait(false);

        //If response body is null but ProtocolObjectResponse was requested, 
        //create empty object
        if (response == null &&
            typeof(ProtocolObjectResponse).IsAssignableFrom(typeof(TResponse)))
        {
            response = new TResponse();
        }

        //If response is ProtocolObjectResponse, 
        //add HttpStatusCode to response
        if (response is ProtocolObjectResponse protocolObject)
        {
            protocolObject.HttpStatusCode = responseMessage.StatusCode;
        }

        return response;
    }

    protected async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        using (ILogger logger = GetLogger())
        {
            await logger.SetRequestAsync(request).ConfigureAwait(false);

            try
            {
                HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

                await logger.SetResponseAsync(response).ConfigureAwait(false);

                await EnsureSuccessStatusCode(response).ConfigureAwait(false);

                logger.EndWithSuccess();

                return response;
            }
            catch (Exception e)
            {
                logger.EndWithError(e);

                throw;
            }
        }
    }

    protected async Task<TResponse> ReadResponse<TResponse>(HttpResponseMessage responseMessage, CancellationToken cancellationToken)
        where TResponse : class
    {
        if (responseMessage == null)
        {
            throw new ArgumentNullException(nameof(responseMessage));
        }

        if (responseMessage.StatusCode == HttpStatusCode.NoContent)
        {
            return null;
        }
        if (typeof(TResponse) == typeof(string))
        {
            return await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false) as TResponse;
        }
        if (typeof(TResponse) == typeof(byte[]))
        {
            return await responseMessage.Content.ReadAsByteArrayAsync().ConfigureAwait(false) as TResponse;
        }
        if (typeof(Stream).IsAssignableFrom(typeof(TResponse)))
        {
            return await responseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false) as TResponse;
        }

        return await responseMessage.Content.ReadFromJsonAsync<TResponse>(cancellationToken).ConfigureAwait(false);
    }

    protected Task<TResponse> GetAsync<TParams, TResponse>(string relativeUrl, TParams parameters, CancellationToken cancellationToken)
        where TParams : class
        where TResponse : class, new()
    {
        if (relativeUrl == null)
        {
            throw new ArgumentNullException(nameof(relativeUrl));
        }

        Uri url = GetUrl(relativeUrl, parameters);

        var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

        return SendAsync<TResponse>(requestMessage, cancellationToken);
    }

    private Task<TResponse> RequestAsync<TRequest, TParams, TResponse>(string relativeUrl, TParams parameters, TRequest request, HttpMethod httpMethod, CancellationToken cancellationToken)
        where TRequest : class
        where TResponse : class, new()
        where TParams : class
    {
        Uri url = GetUrl(relativeUrl, parameters);

        HttpContent content = GetContent(request);

        var requestMessage = new HttpRequestMessage(httpMethod, url) { Content = content };

        return SendAsync<TResponse>(requestMessage, cancellationToken);
    }

    protected Task<TResponse> PostAsync<TRequest, TParams, TResponse>(string relativeUrl, TParams parameters, TRequest request, CancellationToken cancellationToken)
        where TRequest : class
        where TResponse : class, new()
        where TParams : class
    {
        return RequestAsync<TRequest, TParams, TResponse>(relativeUrl, parameters, request, HttpMethod.Post, cancellationToken);
    }

    protected Task<TResponse> PutAsync<TRequest, TParams, TResponse>(string relativeUrl, TParams parameters, TRequest request, CancellationToken cancellationToken)
        where TRequest : class
        where TResponse : class, new()
        where TParams : class
    {
        return RequestAsync<TRequest, TParams, TResponse>(relativeUrl, parameters, request, HttpMethod.Put, cancellationToken);
    }

    protected Task<TResponse> DeleteAsync<TRequest, TParams, TResponse>(string relativeUrl, TParams parameters, TRequest request, CancellationToken cancellationToken)
        where TRequest : class
        where TResponse : class, new()
        where TParams : class
    {
        return RequestAsync<TRequest, TParams, TResponse>(relativeUrl, parameters, request, HttpMethod.Delete, cancellationToken);
    }

    protected Task<TResponse> PatchAsync<TRequest, TParams, TResponse>(string relativeUrl, TParams parameters, TRequest request, CancellationToken cancellationToken)
        where TRequest : class
        where TResponse : class, new()
        where TParams : class
    {
        return RequestAsync<TRequest, TParams, TResponse>(relativeUrl, parameters, request, new HttpMethod("PATCH"), cancellationToken);
    }

    private async Task EnsureSuccessStatusCode(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var error = await TryGetErrorDescriptionAsync(response).ConfigureAwait(false);

            response.Content?.Dispose();

            if (response.StatusCode == HttpStatusCode.Unauthorized ||
                response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new NotAuthorizedException(response.ReasonPhrase, error);
            }

            throw new YandexApiException(response.StatusCode, response.ReasonPhrase, error);
        }
    }

    private async Task<ErrorDescription> TryGetErrorDescriptionAsync(HttpResponseMessage response)
    {
        try
        {
            return response.Content != null
                ? await response.Content.ReadAsAsync<ErrorDescription>().ConfigureAwait(false)
                : null;
        }
        catch (SerializationException) //unexpected data in content
        {
            return null;
        }
    }
}