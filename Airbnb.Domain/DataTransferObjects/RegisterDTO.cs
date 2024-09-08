﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Airbnb.Domain.DataTransferObjects
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Role
    {
        Customer,
        Owner
    }
    public class RegisterDTO
    {
        public string FirstName { get; set; }
        public string MiddlName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public IFormFile? Image { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Role role { get; set; }

    }
}
