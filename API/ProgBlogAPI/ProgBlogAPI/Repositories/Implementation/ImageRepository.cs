using ProgBlogAPI.Data;
using ProgBlogAPI.Models.Domain;
using ProgBlogAPI.Repositories.Interface;

namespace ProgBlogAPI.Repositories.Implementation
{
    public class ImageRepository : IImageRepository
    {
        public readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ApplicationDbContext dbContext;

        public ImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, ApplicationDbContext applicationDbContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = applicationDbContext;
        }


        public async Task<BlogImage> Upload(IFormFile file, BlogImage blogImage)
        {
            //1-upload the image to API/Images
            var localPath = Path.Combine(webHostEnvironment.ContentRootPath, "Images",$"{blogImage.FileName}{blogImage.FileExtension}");
            using var stream = new FileStream(localPath,FileMode.Create);
            await file.CopyToAsync(stream);


            //2-update the databse
            var httpRequest = httpContextAccessor.HttpContext.Request;
            var urlPath = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{blogImage.FileName}{blogImage.FileExtension}";

            blogImage.Url= urlPath;

            dbContext.BlogImages.AddAsync(blogImage);
            await dbContext.SaveChangesAsync();

            return blogImage;


        }
    }
}
