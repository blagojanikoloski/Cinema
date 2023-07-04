using CinemaApp.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaApp.Domain.DTO
{
    public class AdminViewTicketsDto
    {
        public DateTime Date { get; set; }
        public int Price { get; set; }
        public string Genre { get; set; }
        public string MovieName { get; set; }

    }
}
