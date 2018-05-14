using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Models.DTOs
{
    public class UserDTO
    {
        [JsonProperty("first name")]
        public string FirstName { get; set; }
        [JsonProperty("last name")]
        public string LastName { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("repeatedPassword")]
        public string RepeatedPassword { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }

        public UserDTO()
        { }
        


    }
}