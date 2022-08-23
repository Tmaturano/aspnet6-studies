using Azure.Storage.Blobs;
using Blog.Attributes;
using Blog.Data;
using Blog.Dtos;
using Blog.Dtos.Images;
using Blog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace Blog.Controllers
{
    [ApiController]
    [Route("api")]
    [Tags("Image")]
    public class ImageController : ControllerBase
    {
        private readonly BlogDataContext _context;

        public ImageController(BlogDataContext context) => _context = context;

        [Authorize]
        [HttpPost("v1/images/upload-local")]
        public async Task<IActionResult> UploadImageLocal([FromBody] UploadImageDto uploadImageDto)
        {
            var fileName = $"{Guid.NewGuid().ToString()}.jpg";
            var data = new Regex(@"^data:image\/[a-z]+;base64,").Replace(uploadImageDto.Base64Image, "");

            var bytes = Convert.FromBase64String(data);

            try
            {
                //save in any storage, such as S3, Azure Storage, etc
                await System.IO.File.WriteAllBytesAsync($"wwwroot/images/{fileName}", bytes);
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultDto<string>("05X04 - Internal Server Error"));
            }

            var email = User.Identity?.Name;
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user is null)
                return NotFound(new ResultDto<User>("User not found"));

            user.Image = $"https://localhost:0000/images/{fileName}";

            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultDto<string>("05X04 - Internal Server Error"));
            }

            return Ok(new ResultDto<string>("Image updated successfully"));
        }

        public record UploadImage(string Image);

        [ApiKey]
        [HttpPost("v1/images/upload-azure")]
        public async Task<IActionResult> UploadImageAzure([FromServices]IConfiguration configuration, [FromBody]UploadImage base64Image)
        {
            var fileName = $"{Guid.NewGuid().ToString()}.jpg";
            var data = new Regex(@"^data:image\/[a-z]+;base64,").Replace(base64Image.Image, "");

            var imageBytes = Convert.FromBase64String(data);

            try
            {
                var azureStorageConnectionString = configuration.GetValue<string>("AzureStorageConnectionString");
                var containerName = configuration.GetValue<string>("AzureStorageContainerName");
                var blobClient = new BlobClient(azureStorageConnectionString, containerName, fileName);
                                
                using var stream = new MemoryStream(imageBytes);
                await blobClient.UploadAsync(stream);                              

                return StatusCode(200, new ResultDto<string>($"Image uploades successfully in {blobClient.Uri.AbsoluteUri}"));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultDto<string>("05X05 - Internal Server Error"));
            }            
        }
    }
}
