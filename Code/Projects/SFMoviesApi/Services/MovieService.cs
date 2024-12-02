using Microsoft.EntityFrameworkCore;
using SFMoviesApi.Data;
using SFMoviesApi.Entities;

namespace SFMoviesApi.Services
{
    public class MovieService : IMovieService
    {
        private readonly MoviesDbContext _context;

        public MovieService(MoviesDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Movie>> GetMoviesAsync()
        {
            return await _context.Movies.ToListAsync();
        }



        public async Task<IEnumerable<Movie>> SearchMoviesAsync(string query)
        {
            return await _context.Movies
                .Where(m => m.Title.Contains(query))
                .Include(m => m.Locations)
                .ToListAsync();
        }
    }
}
