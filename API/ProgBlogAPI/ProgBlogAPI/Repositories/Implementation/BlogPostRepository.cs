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
            return await this.dbContext.BlogPosts.Include(x=>x.Categories).ToListAsync();
        }

        public async Task<BlogPost?> GetByIdAsync(Guid id)
        {
            return await this.dbContext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x=>x.Id==id);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingBlogPost=await this.dbContext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == blogPost.Id);
            if(existingBlogPost == null)
            {
                return null;
            }
            //update blogpost
            dbContext.Entry(existingBlogPost).CurrentValues.SetValues(blogPost);
            //update categories
            existingBlogPost.Categories = blogPost.Categories;

            await this.dbContext.SaveChangesAsync();
            return blogPost;

        }
    }
}
