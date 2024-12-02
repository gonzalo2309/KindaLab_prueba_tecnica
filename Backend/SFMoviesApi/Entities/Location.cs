namespace SFMoviesApi.Entities
{
    public class Location
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }



        public virtual Movie? Movie { get; set; }
        public int? MovieId { get; set; }


    }
}
