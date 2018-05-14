using Project.Models.DTOs.OfferDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Models.DTOs.BillDTO
{
    public class AdminBillDTO : PublicBillDTO
    {
        public AdminBillDTO()
        { }

        public AdminBillDTO(BillModel bill) : base(bill)
        {
            Buyer = new AdminUserDTO(bill.User);
            Offer = new AdminOfferDTO(bill.Offer);
            PaymentMade = bill.PaymentMade;
            PaymentCanceled = bill.PaymentCanceled;
        }

        public bool PaymentMade { get; set; }     
        public bool PaymentCanceled { get; set; }
        public AdminUserDTO Buyer { get; set; }
        public AdminOfferDTO Offer { get; set; }
    }
}