using ProgBlogAPI.Models.Domain;

namespace ProgBlogAPI.Repositories.Interface
{
    public interface ICategoryRepository
    {
        Task<Category> CreateAsync(Category category);

        Task<IEnumerable<Category>> GetAllAsync();

        Task<Category?> GetById(Guid id);

        Task<Category?> UpdateCategory(Category category);

        Task<Category?> DeleteCategory(Guid id);
    }

}
