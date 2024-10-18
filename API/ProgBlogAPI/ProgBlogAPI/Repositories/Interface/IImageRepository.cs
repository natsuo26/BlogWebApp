using ProgBlogAPI.Models.Domain;

namespace ProgBlogAPI.Repositories.Interface
{
    public interface IImageRepository
    {
        Task<BlogImage> Upload(IFormFile file, BlogImage blogImage);
    }
}
