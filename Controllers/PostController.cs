using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Workintech02RestApiDemo.Business.Post;
using Workintech02RestApiDemo.Domain.Entities;

namespace Workintech02RestApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        // GET: api/Post
        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            return Ok(await _postService.GetPostsAsync());
        }

        // GET: api/Post/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            var post = await _postService.GetPostAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        // POST: api/Post
        [HttpPost]
        public async Task<IActionResult> CreatePost(Post post)
        {
            await _postService.CreatePostAsync(post);
            return CreatedAtAction(nameof(GetPost), new { id = post.Id }, post);
        }

        // PUT: api/Post/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(int id, Post post)
        {
            if (id != post.Id)
            {
                return BadRequest();
            }

            await _postService.UpdatePostAsync(post);
            return NoContent();
        }

        // DELETE: api/Post/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            await _postService.DeletePostAsync(id);
            return NoContent();
        }
    }
}
