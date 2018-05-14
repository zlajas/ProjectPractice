using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Models.DTOs
{
    public class PublicUserDTO
    {
        public PublicUserDTO ()
        { }
        public PublicUserDTO(UserModel user)
        {
            Id = user.Id;
            Username = user.Username;
        }

        [JsonProperty("ID")]
        public int Id { get; set; }
        public string Username { get; set; }
    }
}