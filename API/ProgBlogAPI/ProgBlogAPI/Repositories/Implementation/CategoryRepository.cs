using ProgBlogAPI.Models.Domain;
using ProgBlogAPI.Repositories.Interface;
using ProgBlogAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace ProgBlogAPI.Repositories.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext dbContext;
        public CategoryRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Category> CreateAsync(Category category)
        {
            await this.dbContext.Categories.AddAsync(category);
            await this.dbContext.SaveChangesAsync();

            return category;

        }

        public async Task<Category?> DeleteCategory(Guid id)
        {
            var category=await this.dbContext.Categories.FirstOrDefaultAsync(x=>x.Id == id);
            if (category == null)
            {
                return null;
            }
            dbContext.Categories.Remove(category);
            await dbContext.SaveChangesAsync();
            return category;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await this.dbContext.Categories.ToListAsync();
            
        }

        public async Task<Category?> GetById(Guid id)
        {
            return await this.dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Category?> UpdateCategory(Category category)
        {
            var existingCategory=await dbContext.Categories.FirstOrDefaultAsync(x=>x.Id == category.Id);
            if (existingCategory != null)
            {
                dbContext.Entry(existingCategory).CurrentValues.SetValues(category);
                await dbContext.SaveChangesAsync();
                return category;
            }
            return null;
        }
    }
}
