using CinemaApp.Domain.DTO;
using CinemaApp.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Repository.Interface
{
    public interface IUserRepository
    {
        IEnumerable<CinemaAppApplicationUser> GetAll();
        void Insert(CinemaAppApplicationUser entity);
        void Update(CinemaAppApplicationUser entity);
        void Delete(CinemaAppApplicationUser entity);



    }
}
