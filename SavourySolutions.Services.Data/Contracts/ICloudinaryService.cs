using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;

namespace SavourySolutions.Services.Data.Contracts
{
    public interface ICloudinaryService
    {
        Task<string> UploadAsync(IFormFile file, string fileName);

        Task DeleteImage(Cloudinary cloudinary, string name);
    }
}
