using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project.Models
{
    [Table("bills")]
    public class BillModel
    {
        public int Id { get; set; }
        [Column("payment_made")]
        public bool PaymentMade { get; set; }
        [Column("payment_canceled")]
        public bool PaymentCanceled { get; set; }
        [Column("created_at")]
        public DateTime BillCreated {get; set;}
        [NotMapped]
        public int? OfferId { get; set; }
        [NotMapped]
        public int? BuyerId { get; set; }
        [Column("offer_id")]
        public virtual OfferModel Offer { get; set; }
        [Column("buyer_id")]
        public virtual UserModel User { get; set; }

        public BillModel ()
        {
        }

    }
}