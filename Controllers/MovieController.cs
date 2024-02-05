using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Workintech02RestApiDemo.Business.Movie;
using Workintech02RestApiDemo.Domain.Entities;

namespace Workintech02RestApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            var movies = await _movieService.GetMoviesAsync();
            return Ok(movies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            var movie = await _movieService.GetMovieAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }

        [HttpPost]
        public async Task<ActionResult<Movie>> CreateMovie(Movie movie)
        {
            await _movieService.CreateMovieAsync(movie);
            return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, Movie movie)
        {
            if (id != movie.Id)
            {
                return BadRequest();
            }

            await _movieService.UpdateMovieAsync(movie);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _movieService.GetMovieAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            await _movieService.DeleteMovieAsync(movie.Id);

            return NoContent();
        }
    }
}
