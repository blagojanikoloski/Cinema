using System;

namespace CinemaApp.Domain.DomainModels
{
    public class MovieDates
    {
        public int MovieDatesID { get; set; }
        public int MovieID { get; set; }
        public DateTime Date { get; set; }
        public int Price { get; set; }

        public MovieDates()
        {

        }
    }
}
