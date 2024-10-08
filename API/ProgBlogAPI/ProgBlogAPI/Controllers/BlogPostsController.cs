using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProgBlogAPI.Models.Domain;
using ProgBlogAPI.Models.DTO;
using ProgBlogAPI.Repositories.Interface;

namespace ProgBlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        public readonly IBlogPostRepository BlogPostRepository;
        public BlogPostsController(IBlogPostRepository blogPostRepository)
        {
            BlogPostRepository = blogPostRepository;
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

            };
            
            blogPost = await BlogPostRepository.CreateAsync(blogPost);
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
            };
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetBlogPosts()
        {


            var blogPosts = await BlogPostRepository.GetAllAsync();
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
                });
                
            }
            return Ok(response);
        }
    }
}
