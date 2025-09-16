using Microsoft.AspNetCore.Components.Forms;

namespace ABCRetailApp.Data;

public interface IBlobStorageService
{
    Task<string> UploadAsync(IBrowserFile file, string fileName, string contentType);
}
