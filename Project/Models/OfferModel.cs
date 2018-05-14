using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project.Models
{
    public enum OfferStatus { WAIT_FOR_APPROVING=1, APPROVED=2, DECLINED=3, EXPIRED=4}
    [Table("offers")]
    public class OfferModel
    {
       

        public int Id { get; set; }
        [Column("name")]
        public string OfferName { get; set; }
        [Column("description")]
        public string OfferDescription { get; set; }
        [Column("created_at")]
        public DateTime OfferCreated { get; set; }
        [Column("expires_at")]
        public DateTime OfferExpires { get; set; }
        [Column("regular_price")]
        public double RegularPrice { get; set; }
        [Column("action_price")]
        public double ActionPrice { get; set; }
        [Column("image_path")]
        public string ImagePath { get; set; }
        [Column("available_offers")]
        public int AvailableOffers { get; set; }
        [Column("bought_offers")]
        public int BoughtOffers { get; set; }
        [Column("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public OfferStatus OfferStatus { get; set; }
        [NotMapped]
        public int? CategoryId { get; set; }
        [NotMapped]
        public int? SellerId { get; set; }
        [Column("category_id")]
        public virtual CategoryModel Category { get; set; }
        [Column("seller_id")]
        public virtual UserModel User { get; set; }
        [JsonIgnore ]
        public virtual ICollection<BillModel> Bills { get; set; }
        [JsonIgnore]
        public virtual ICollection<VoucherModel> Vouchers { get; set; }
        public OfferModel ()
        {
            Bills = new List<BillModel>();
            Vouchers = new List<VoucherModel>();
        }       
    }
}