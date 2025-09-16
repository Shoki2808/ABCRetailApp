using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace ABCRetailApp.Data;

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _serviceClient;
    private readonly string _containerName;

    public BlobStorageService(BlobServiceClient serviceClient, string containerName)
    {
        _serviceClient = serviceClient;
        _containerName = containerName;
    }

    public async Task<string> UploadAsync(IBrowserFile file, string fileName, string contentType)
    {
        var container = _serviceClient.GetBlobContainerClient(_containerName);
        await container.CreateIfNotExistsAsync(PublicAccessType.Blob);
        var blob = container.GetBlobClient(fileName);

        await using var stream = file.OpenReadStream(50 * 1024 * 1024); // 50 MB limit
        await blob.UploadAsync(stream, new BlobHttpHeaders { ContentType = contentType });
        return blob.Uri.ToString();
    }
}
