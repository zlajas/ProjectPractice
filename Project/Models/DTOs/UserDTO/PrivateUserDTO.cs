using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Models.DTOs
{
    public class PrivateUserDTO : PublicUserDTO
    {
        public PrivateUserDTO ()
        { }

        public PrivateUserDTO(UserModel user) : base(user)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            Password = user.Password;
            Email = user.Email;
        }
        [JsonProperty(Order = 4)]
        public string FirstName { get; set; }
        [JsonProperty(Order = 4)]
        public string LastName { get; set; }
        [JsonIgnore]
        [JsonProperty(Order = 4)]
        public string Password { get; set; }
        [JsonProperty(Order = 4)]
        public string Email { get; set; }
    }
}