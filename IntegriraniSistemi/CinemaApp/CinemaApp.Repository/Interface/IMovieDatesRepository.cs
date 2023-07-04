using CinemaApp.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Repository.Interface
{
    public interface IMovieDatesRepository
    {

        List<MovieDates> GetMovieDatesByMovieId(int movieId);
        int GetMovieDatesPriceById(int movieDatesID);

        MovieDates GetMovieDatesByID(int movieDatesID);

        Task<List<MovieDates>> GetAllMovieDatesAsync();
    }
}
