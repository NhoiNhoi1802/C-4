using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Ếch_ăn_chay.Models;
using Microsoft.Extensions.Options;

namespace Ếch_ăn_chay.Services
{
    public class CloudinaryService
    {
        public readonly Cloudinary _cloudinary;
        public CloudinaryService(IOptions<CloudinarySettings> config)
        {
            var settings = config.Value;

            var account = new Account(
                settings.CloudName,
                settings.ApiKey,
                settings.ApiSecret
            );

            _cloudinary = new Cloudinary(account);
        }

        public Cloudinary GetInstance() => _cloudinary;


        public async Task<string> UploadImageAsync(IFormFile file, string oldImageUrl = null)
        {
            if (file == null || file.Length == 0)
                return null;

            // Xóa ảnh cũ nếu có
            if (!string.IsNullOrEmpty(oldImageUrl))
                await DeleteImageAsync(oldImageUrl);

            try
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "products"
                };
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                    return uploadResult.SecureUrl.ToString();
                else
                {
                    Console.WriteLine($"Upload failed: {uploadResult.Error?.Message}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception when uploading: {ex.Message}");
                return null;
            }
        }


        public async Task<bool> DeleteImageAsync(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
                return false;

            try
            {
                // Lấy public_id từ URL
                var uri = new Uri(imageUrl);
                var segments = uri.AbsolutePath.Split('/');
                var fileName = segments.Last(); // ví dụ "codoanna.jpg"
                var publicId = $"products/{Path.GetFileNameWithoutExtension(fileName)}";

                var deletionParams = new DeletionParams(publicId);
                var result = await _cloudinary.DestroyAsync(deletionParams);

                return result.Result == "ok" || result.Result == "not found";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception when deleting: {ex.Message}");
                return false;
            }
        }
    }
}