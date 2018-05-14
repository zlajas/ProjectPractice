using Newtonsoft.Json;
using Project.Models.DTOs.OfferDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Models.DTOs.VoucherDTO
{
    public class AdminVoucherDTO : PublicVoucherDTO
    {
        public AdminVoucherDTO()
        { }

        public AdminVoucherDTO(VoucherModel voucher) : base(voucher)
        {
            IsUsed = voucher.IsUsed;
            Buyer = new AdminUserDTO(voucher.User);
            Offer = new AdminOfferDTO(voucher.Offer);
        }
        public AdminUserDTO Buyer { get; set; }
        public AdminOfferDTO Offer { get; set; }
        public bool IsUsed { get; set; }
    }
}