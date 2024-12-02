using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SFMoviesApi.Data;
using SFMoviesApi.Dto;

[ApiController]
[Route("api/movies")]
public class MoviesController : ControllerBase
{
    private readonly MoviesDbContext _context;

    public MoviesController(MoviesDbContext context)
    {
        _context = context;
    }


    [AllowAnonymous]
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string query)
    {
        var movies = await _context.Movies
            .Where(m => m.Title.Contains(query))
            .Include(m => m.Locations)
            .ToListAsync();

        var movieDtos = movies.Select(m => new MovieDto
        {
            Title = m.Title,
            Actor1 = m.Actor1,
            Actor2 = m.Actor2,
            Actor3 = m.Actor3,
            City = m.City,
            Director = m.Director,
            Distributor = m.Distributor,
            FunFacts = m.FunFacts,
            ProductionCompany = m.ProductionCompany,
            State = m.State,
            Writer = m.Writer,
            ReleaseYear = m.ReleaseYear,
            Locations = m.Locations != null ? m.Locations.Select(l => new LocationDto
            {
                Address = l.Address,
                Latitude = l.Latitude,
                Longitude = l.Longitude
            }).ToList() : new List<LocationDto>()
        }).ToList();

        return Ok(movieDtos);
    }

    [HttpGet("search-titles")]
    public async Task<IActionResult> SearchMovieTitles([FromQuery] string query)
    {
        if (string.IsNullOrEmpty(query))
        {
            return BadRequest("Query cannot be empty");
        }

        var movieTitles = await _context.Movies
            .Where(m => m.Title.Contains(query))
            .Select(m => m.Title)
            .Take(10) 
            .ToListAsync();

        return Ok(movieTitles);
    }
}
