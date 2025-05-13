using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProgBlogAPI.Models.Domain;
using ProgBlogAPI.Models.DTO;
using ProgBlogAPI.Repositories.Interface;

namespace ProgBlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        //GET: {apibaseurl}/api/images
        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            var images = await imageRepository.GetAll();
            if (images == null || !images.Any())
            {
                return NotFound("No images found");
            }
            //convert domain model to DTO
            var response = images.Select(image => new BlogImageDto
            {
                Id = image.Id,
                Title = image.Title,
                DateCreated = image.DateCreated,
                FileExtension = image.FileExtension,
                FileName = image.FileName,
                Url = image.Url,
            });
            return Ok(response);

        }

        //POST: {apibaseurl}/api/images
        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm]IFormFile file, [FromForm] string fileName, [FromForm] string title)
        {
            ValidateFileUpload(file);
            if(ModelState.IsValid)
            {
                var blogImage = new BlogImage
                {
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                    FileName = fileName,
                    Title = title,
                    DateCreated = DateTime.Now,
                };

                blogImage = await imageRepository.Upload(file, blogImage);

                //convert domain model to DTO
                var response = new BlogImageDto
                {
                    Id = blogImage.Id,
                    Title = blogImage.Title,
                    DateCreated = blogImage.DateCreated,
                    FileExtension=blogImage.FileExtension,
                    FileName = blogImage.FileName,
                    Url = blogImage.Url,
                };

                return Ok(response);
            }

            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(file.FileName);
            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "Unsupported file format");
            }

            if (file.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size cannot be more than 10MB");
            }
        }
    }
}
