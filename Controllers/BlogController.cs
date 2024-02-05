using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Workintech02RestApiDemo.Business.Blog;
using Workintech02RestApiDemo.Domain.Entities;

namespace Workintech02RestApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [Authorize]
        // GET: api/Blog
        [HttpGet]
        public async Task<IActionResult> GetBlogs()
        {
            return Ok(await _blogService.GetBlogsAsync());
        }

        // GET: api/Blog/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlog(int id)
        {
            var blog = await _blogService.GetBlogAsync(id);
            if (blog == null)
            {
                return NotFound();
            }

            return Ok(blog);
        }

        // POST: api/Blog
        [HttpPost]
        public async Task<IActionResult> CreateBlog(Blog blog)
        {
            await _blogService.CreateBlogAsync(blog);
            return CreatedAtAction(nameof(GetBlog), new { id = blog.Id }, blog);
        }

        // PUT: api/Blog/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBlog(int id, Blog blog)
        {
            if (id != blog.Id)
            {
                return BadRequest();
            }

            await _blogService.UpdateBlogAsync(blog);
            return NoContent();
        }

        // DELETE: api/Blog/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            await _blogService.DeleteBlogAsync(id);
            return NoContent();
        }
    }
}
