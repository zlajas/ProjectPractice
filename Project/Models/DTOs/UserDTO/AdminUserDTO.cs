using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Models.DTOs
{
    public class AdminUserDTO : PrivateUserDTO
    {
        public AdminUserDTO ()
        { }

        public AdminUserDTO(UserModel user) : base(user)
        {
            UserRole = user.UserRole;
        }
        [JsonProperty(Order = 6)]
        [JsonConverter(typeof(StringEnumConverter))]
        public UserRole UserRole { get; set; }
    }
}