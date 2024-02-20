﻿using YaDiskBackup.YandexDisk.Client.Protocol;

namespace YaDiskBackup.YandexDisk.Client.Clients;

/// <summary>
/// Files and folder metadata client
/// </summary>
public interface IMetaInfoClient
{
    /// <summary>
    /// Returns information about disk
    /// </summary>
    Task<Disk> GetDiskInfoAsync(CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Return files or folder metadata
    /// </summary>
    Task<Resource> GetInfoAsync(ResourceRequest request, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Return files or folder metadata in the trash
    /// </summary>
    Task<Resource> GetTrashInfoAsync(ResourceRequest request, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Flat file list on Disk
    /// </summary>
    /// <returns></returns>
    Task<FilesResourceList> GetFilesInfoAsync(FilesResourceRequest request, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Last uploaded file list on Disk
    /// </summary>
    Task<LastUploadedResourceList> GetLastUploadedInfoAsync(LastUploadedResourceRequest request, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Append custom properties to resource
    /// </summary>
    Task<Resource> AppendCustomProperties(string path, IDictionary<string, string> properties, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Publish resource
    /// </summary>
    Task<Link> PublishFolderAsync(string path, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Unpublish resource
    /// </summary>
    Task<Link> UnpublishFolderAsync(string path, CancellationToken cancellationToken = default(CancellationToken));
}