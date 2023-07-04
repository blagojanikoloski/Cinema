namespace CinemaApp.Domain.DomainModels
{
    public class Movie
    {
        public int MovieID { get; set; }
        public string MovieName { get; set; }
        public string MovieDescription { get; set; }

        public System.DateTime MovieReleaseDate { get; set; }
        public string Genre { get; set; }



        public Movie()
        {

        }
    }
}
