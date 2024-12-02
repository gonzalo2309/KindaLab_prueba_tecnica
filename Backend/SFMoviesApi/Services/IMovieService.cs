using SFMoviesApi.Entities;

namespace SFMoviesApi.Services
{
    public interface IMovieService
    {
        Task<IEnumerable<Movie>> GetMoviesAsync();        
        Task<IEnumerable<Movie>> SearchMoviesAsync(string query);
    }
}
