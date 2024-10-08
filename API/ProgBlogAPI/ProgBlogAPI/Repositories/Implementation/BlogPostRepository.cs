using ProgBlogAPI.Models.Domain;
using ProgBlogAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using ProgBlogAPI.Data;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ProgBlogAPI.Repositories.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly ApplicationDbContext dbContext;
        public BlogPostRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
        {
            await this.dbContext.AddAsync(blogPost);
            await this.dbContext.SaveChangesAsync();
            return blogPost;    
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await this.dbContext.BlogPosts.ToListAsync();
        }
    }
}
