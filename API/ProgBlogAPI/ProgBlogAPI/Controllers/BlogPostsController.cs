using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProgBlogAPI.Models.Domain;
using ProgBlogAPI.Models.DTO;
using ProgBlogAPI.Repositories.Implementation;
using ProgBlogAPI.Repositories.Interface;

namespace ProgBlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        public readonly IBlogPostRepository blogPostRepository;

        public ICategoryRepository categoryRepository;

        public BlogPostsController(IBlogPostRepository blogPostRepository, ICategoryRepository categoryRepository)
        {
            this.blogPostRepository = blogPostRepository;
            this.categoryRepository = categoryRepository;
        }


        [HttpPost]
        public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostRequestDto request)
        {
            var blogPost = new BlogPost
            {
                Author = request.Author,
                Title = request.Title,
                ShortDescription = request.ShortDescription,
                FeatureImageUrl = request.FeatureImageUrl,
                UrlHandle = request.UrlHandle,
                PublishedDate = request.PublishedDate,
                IsVisible = request.IsVisible,
                Content = request.Content,
                Categories = new List<Category>()

            };
            
            foreach(var categoryGuid in request.Categories)
            {
                var existingCategory = await categoryRepository.GetById(categoryGuid);
                if(existingCategory != null)
                {
                    blogPost.Categories.Add(existingCategory);
                }
            }
            
            blogPost = await blogPostRepository.CreateAsync(blogPost);
            //convert domain model back to DTO;
            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Title = blogPost.Title,
                ShortDescription = blogPost.ShortDescription,
                FeatureImageUrl = blogPost.FeatureImageUrl,
                UrlHandle = blogPost.UrlHandle,
                PublishedDate = blogPost.PublishedDate,
                IsVisible = blogPost.IsVisible,
                Content = blogPost.Content,
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,

                }).ToList()
            };
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetBlogPosts()
        {


            var blogPosts = await blogPostRepository.GetAllAsync();
            //convert domain model back to DTO;
            var response = new List<BlogPostDto>();
            foreach (var blogPost in blogPosts)
            {
                response.Add(new BlogPostDto
                {
                    Id = blogPost.Id,
                    Author = blogPost.Author,
                    Title = blogPost.Title,
                    ShortDescription = blogPost.ShortDescription,
                    FeatureImageUrl = blogPost.FeatureImageUrl,
                    UrlHandle = blogPost.UrlHandle,
                    PublishedDate = blogPost.PublishedDate,
                    IsVisible = blogPost.IsVisible,
                    Content = blogPost.Content,
                    Categories = blogPost.Categories.Select(x => new CategoryDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UrlHandle = x.UrlHandle,

                    }).ToList()
                });
                
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetBlogPostById([FromRoute] Guid id)
        {


            var blogPost = await blogPostRepository.GetByIdAsync(id);
            if(blogPost == null)
            {
                return NotFound();
            }
            //convert domain model back to DTO;
            var response = new BlogPostDto();
            response = new BlogPostDto
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Title = blogPost.Title,
                ShortDescription = blogPost.ShortDescription,
                FeatureImageUrl = blogPost.FeatureImageUrl,
                UrlHandle = blogPost.UrlHandle,
                PublishedDate = blogPost.PublishedDate,
                IsVisible = blogPost.IsVisible,
                Content = blogPost.Content,
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,

                }).ToList()
            };
            return Ok(response);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateBlogPost([FromRoute] Guid id, UpdateBlogPostRequestDto request)
        {
            //convert DTO to Domain Model
            var blogPost = new BlogPost
            {
                Id = id,
                Author = request.Author,
                Title = request.Title,
                ShortDescription = request.ShortDescription,
                FeatureImageUrl = request.FeatureImageUrl,
                UrlHandle = request.UrlHandle,
                PublishedDate = request.PublishedDate,
                IsVisible = request.IsVisible,
                Content = request.Content,
                Categories = new List<Category>()
            };

            foreach (var categoryGuid in request.Categories)
            {
                var existingCategory = await categoryRepository.GetById(categoryGuid);
                if (existingCategory != null)
                {
                    blogPost.Categories.Add(existingCategory);
                }
            }

            //call repository to update blogpost domain model
            var updatedBlogPost = await blogPostRepository.UpdateAsync(blogPost);

            if(updatedBlogPost == null)
            {
                return NotFound();
            }

            //convert Domain model back to DTO
            var response = new BlogPostDto();
            response = new BlogPostDto
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Title = blogPost.Title,
                ShortDescription = blogPost.ShortDescription,
                FeatureImageUrl = blogPost.FeatureImageUrl,
                UrlHandle = blogPost.UrlHandle,
                PublishedDate = blogPost.PublishedDate,
                IsVisible = blogPost.IsVisible,
                Content = blogPost.Content,
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,

                }).ToList()
            };
            return Ok(response);

        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteBlogPost([FromRoute] Guid id)
        {

            var deletedBlogPost = await blogPostRepository.DeleteAsync(id);
            if (deletedBlogPost is null)
            {
                return NotFound();
            }
            var response = new BlogPostDto
            {
                Id = deletedBlogPost.Id,
                Author = deletedBlogPost.Author,
                Title = deletedBlogPost.Title,
                ShortDescription = deletedBlogPost.ShortDescription,
                FeatureImageUrl = deletedBlogPost.FeatureImageUrl,
                UrlHandle = deletedBlogPost.UrlHandle,
                PublishedDate = deletedBlogPost.PublishedDate,
                IsVisible = deletedBlogPost.IsVisible,
                Content = deletedBlogPost.Content,
            };
            return Ok(response);
        }


    }
}
