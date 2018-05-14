using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json.Converters;

namespace Project.Models
{
    public enum UserRole { ROLE_CUSTOMER=1, ROLE_ADMIN=2, ROLE_SELLER=3 }

    [Table("user")]
    public class UserModel
    {
        public int Id { get; set; }
        [Column("first_name")]
        public string FirstName { get; set; }
        [Column("last_name")]
        public string LastName { get; set; }
        [Column("username")]
        public string Username { get; set; }
        [Column("password")]
        [JsonIgnore]
        public string Password { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("user_role")]
        [JsonConverter(typeof(StringEnumConverter))]
        public UserRole UserRole { get; set; }
        [JsonIgnore]
        public virtual ICollection<OfferModel> Offers { get; set; }
        [JsonIgnore]
        public virtual ICollection<BillModel> Bills { get; set; }
        [JsonIgnore]
        public virtual ICollection<VoucherModel> Vouchers { get; set; }
        public UserModel()
        {
            Offers = new List<OfferModel>();
            Bills = new List<BillModel>();
            Vouchers = new List<VoucherModel>();
        }      
    }
}