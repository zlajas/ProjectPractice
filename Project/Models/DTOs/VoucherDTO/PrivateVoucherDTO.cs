using Newtonsoft.Json;
using Project.Models.DTOs.OfferDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Models.DTOs.VoucherDTO
{
    public class PrivateVoucherDTO : PublicVoucherDTO
    {
        public PrivateVoucherDTO()
        { }

        public PrivateVoucherDTO(VoucherModel voucher) : base(voucher)
        {
            Buyer = new PrivateUserDTO(voucher.User);
            Offer = new PrivateOfferDTO(voucher.Offer);

        }

        public PrivateUserDTO Buyer { get; set; }
        public PrivateOfferDTO Offer { get; set; }
    }
}