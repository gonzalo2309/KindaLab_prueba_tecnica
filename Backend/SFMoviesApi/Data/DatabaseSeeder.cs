
using Newtonsoft.Json;
using SFMoviesApi.Entities;


namespace SFMoviesApi.Data
{

    public static class DatabaseSeeder
    {


        public static void SeedData(MoviesDbContext context)
        {
            if (!context.Movies.Any())
            {
                // Ruta del archivo JSON
                var jsonPath = Path.Combine(AppContext.BaseDirectory, "Data", "movies.json");

                if (!File.Exists(jsonPath))
                {
                    throw new FileNotFoundException($"No se encontró el archivo JSON en la ruta: {jsonPath}");
                }

                // Leer y deserializar el JSON
                var jsonData = File.ReadAllText(jsonPath);
                var records = JsonConvert.DeserializeObject<List<MovieJson>>(jsonData);
                if (records == null)
                {
                    return;
                }

                // Mapeamos las películas y sus ubicaciones
                var movies = MapMoviesWithLocations(records);

                // Guardamos todas las películas en la base de datos
                context.Movies.AddRange(movies);
                context.SaveChanges();
            }
        }

        // Método para mapear películas y ubicaciones
        private static List<Movie> MapMoviesWithLocations(List<MovieJson> records)
        {
            var movies = new List<Movie>();
            var noaplicatext = string.Empty;

            foreach (var record in records)
            {
                // Crear la ubicación
                var location = new Location
                {
                    Address = record.Locations ?? noaplicatext,
                    Latitude = "0.0",
                    Longitude = "0.0"
                };

                // Si hay coordenadas válidas en Point, las parseamos
                if (!string.IsNullOrWhiteSpace(record.Point))
                {
                    var (latitude, longitude) = ParseCoordinates(record.Point);
                    location.Latitude = latitude;
                    location.Longitude = longitude;
                }

                // Verificar si la película ya existe en el listado
                var existingMovie = movies.FirstOrDefault(m =>
                    m.Title == (record.Title ?? noaplicatext) &&
                    m.ReleaseYear == record.ReleaseYear);

                if (existingMovie != null)
                {
                    // Si la película existe, añadimos la nueva ubicación si no está duplicada
                    if (!existingMovie.Locations.Any(l => l.Address == location.Address))
                    {
                        existingMovie.Locations.Add(location);
                    }
                }
                else
                {
                    // Si la película no existe, la creamos con la ubicación inicial
                    var movie = new Movie
                    {
                        Title = record.Title ?? noaplicatext,
                        ReleaseYear = record.ReleaseYear,
                        FunFacts = record.FunFacts ?? noaplicatext,
                        ProductionCompany = record.ProductionCompany ?? noaplicatext,
                        Distributor = record.Distributor ?? noaplicatext,
                        Director = record.Director ?? noaplicatext,
                        Writer = record.Writer ?? noaplicatext,
                        Actor1 = record.Actor1 ?? noaplicatext,
                        Actor2 = record.Actor2 ?? noaplicatext,
                        Actor3 = record.Actor3 ?? noaplicatext,
                        State = record.State ?? noaplicatext,
                        City = record.City ?? noaplicatext,
                        Locations = new List<Location> { location }
                    };

                    movies.Add(movie);
                }
            }

            return movies;
        }


        private static (string Latitude, string Longitude) ParseCoordinates(string point)
        {
            if (string.IsNullOrWhiteSpace(point))
            {
                return (Latitude: "0.0", Longitude: "0.0");
            }

            var coordinates = point.Replace("POINT (", "").Replace(")", "").Split(' ');
            return (Latitude: coordinates[1], Longitude: coordinates[0]);
        }
    }
}

