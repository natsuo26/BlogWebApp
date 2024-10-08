using ProgBlogAPI.Models.Domain;

namespace ProgBlogAPI.Repositories.Interface
{
    public interface IBlogPostRepository
    {
        Task<BlogPost> CreateAsync(BlogPost blogPost);
    }
}
