namespace YaDiskBackup.YandexDisk.Client.Http;

/// <summary>
/// Abstract request sender for testing purpose
/// </summary>
public interface IHttpClient : IDisposable
{
    /// <summary>
    /// Send http-request to API
    /// </summary>
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default);
}