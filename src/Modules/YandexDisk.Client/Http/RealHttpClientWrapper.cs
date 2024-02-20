﻿namespace YaDiskBackup.YandexDisk.Client.Http;

internal class RealHttpClientWrapper : IHttpClient
{
    public RealHttpClientWrapper(HttpMessageInvoker httpMessageInvoker)
    {
        HttpMessageInvoker = httpMessageInvoker;
    }

    private HttpMessageInvoker HttpMessageInvoker { get; }


    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default(CancellationToken))
    {
        return HttpMessageInvoker.SendAsync(request, cancellationToken);
    }

    public void Dispose()
    {
        HttpMessageInvoker.Dispose();
    }
}