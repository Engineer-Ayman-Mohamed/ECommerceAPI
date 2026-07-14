namespace ECommerceAPI.Domain.Interfaces.Services;

public interface IPictureService
{
    Task<string> UploadPictureAsync(Stream fileStream, string fileName);
    Task DeletePictureAsync(string publicId);
}
