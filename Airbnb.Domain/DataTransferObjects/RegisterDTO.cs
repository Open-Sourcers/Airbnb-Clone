using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Domain.DataTransferObjects
{
    public class RegisterDTO
    {
        public string FirstName { get; set; }
        public string MiddltName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public IFormFile? Image { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
