using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    [Table("vouchers")]
    public class VoucherModel
    {
        public int Id { get; set; }
        [Column("expires_at")]
        public DateTime ExpirationDate { get; set; }
        [Column("is_used")]
        public bool IsUsed { get; set; }
        [NotMapped]
        public int? OfferId { get; set; }
        [NotMapped]
        public int? BuyerId { get; set; }
        [Column("offer_id")]
        public virtual OfferModel Offer { get; set; }
        [Column("buyer_id")]
        public virtual UserModel User { get; set; }

        public VoucherModel ()
        { }
    }
}