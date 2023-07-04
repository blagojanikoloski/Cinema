using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaApp.Domain.DomainModels
{
    public class UserExcelData
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }

        public string Role { get; set; }
    }
}
