using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Domain.DataTransferObjects
{
    public class LoginDTO
    {
        [DataType(DataType.EmailAddress,ErrorMessage ="Invalid Email")]
        public string Email { get; set; }
        [DataType(DataType.Password,ErrorMessage ="Invalid Password")]
        public string Password { get; set; }
    }
}
