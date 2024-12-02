namespace SFMoviesApi.Dto
{
    public class MovieDto
    {
        
        public string Title { get; set; }
        public int ReleaseYear { get; set; }
        public string FunFacts { get; set; }
        public string ProductionCompany { get; set; }
        public string Distributor { get; set; }
        public string Director { get; set; }
        public string Writer { get; set; }
        public string Actor1 { get; set; }
        public string Actor2 { get; set; }
        public string Actor3 { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public List<LocationDto> Locations { get; set; }
    }

   

}
