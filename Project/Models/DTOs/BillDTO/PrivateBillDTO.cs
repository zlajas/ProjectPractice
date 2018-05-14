using Project.Models.DTOs.OfferDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Models.DTOs.BillDTO
{
    public class PrivateBillDTO : PublicBillDTO
    {
        public PrivateBillDTO()
        { }
        public PrivateBillDTO(BillModel bill) : base(bill)
        {
            Buyer = new PrivateUserDTO(bill.User);
            Offer = new PrivateOfferDTO(bill.Offer);
                    
        }

        public PrivateUserDTO Buyer { get; set; }
        public PrivateOfferDTO Offer { get; set; }
    }

}