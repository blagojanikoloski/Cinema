using CinemaApp.Domain.DomainModels;
using CinemaApp.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Service.Interface
{
    public interface IMovieDatesService
    {
        List<MovieDates> GetMovieDatesByMovieId(int movieId);

        int GetMovieDatesPriceById(int movieDatesID);

        MovieDates GetMovieDatesByID(int movieDatesID);

        Task<IEnumerable<AdminViewTicketsDto>> GetMovieDatesWithDetailsAsync(string movieName, string genre);
    }

}
